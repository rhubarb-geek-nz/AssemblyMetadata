#!/usr/bin/env pwsh
# Copyright (c) 2025 Roger Brown.
# Licensed under the MIT License.

$ErrorActionPreference = 'Stop'

dotnet publish TestModule/TestModule.csproj --configuration Release

$dll = 'TestModule/bin/Release/netstandard2.0/publish/TestModule.dll'

Import-AssemblyMetadata -LiteralPath $dll | ForEach-Object {
	$_.GetTypes() | ForEach-Object {
		if ($_.IsEnum)
		{
			$i = 0
			$list = @()
			$names = $_.GetEnumNames()
			$values = $_.GetEnumValuesAsUnderlyingType() | Sort-Object
			$sequential = $true
			while ($i -lt $names.Count)
			{
				if ($values[$i] -ne $i)
				{
					$sequential = $false
				}
				$i++
			}
			if ($sequential)
			{
				$list = $names
			}
			else
			{
				$i = 0
				foreach ($value in $values)
				{
					$list += $names[$i] + '=' + $value
					$i++
				}
			}
			[pscustomobject]@{
				Name = $_.Name
				Value = $list | Join-String -Separator ', '
				Attributes = $_.Attributes
			}
		}
		else
		{
			$_.GetFields() | ForEach-Object {
				if ($_.IsLiteral)
				{
					$value = $_.GetRawConstantValue()
					if ($_.FieldType.IsEnum)
					{
						$i = 0
						$values = $_.FieldType.GetEnumValuesAsUnderlyingType() | Sort-Object
						$names = $_.FieldType.GetEnumNames()
						$nomatch = $true
						foreach ($v in $values)
						{
							if ($value -eq $v)
							{
								$value = $names[$i]
								$nomatch = $false
								break
							}
							$i++
						}

						if ($nomatch -and ($_.FieldType.CustomAttributes | Where-Object { $_.AttributeType.Name -eq 'FlagsAttribute' }))
						{
							$i = 0
							$masks = @()
							$list = @()

							foreach ($n in $names)
							{
								if ($n.EndsWith('Mask'))
								{
									$masks += $values[$i]
								}

								$i++
							}

							$i = 0

							foreach ($v in $values)
							{
								if ($v)
								{
									$mask = $null

									foreach ($m in $masks)
									{
										if (($m -band $v) -eq $v)
										{
											$mask = $m
											break
										}
									}

									if (-not $mask)
									{
										$mask = $v
									}

									if (($value -band $mask) -eq $v)
									{
										$list += $names[$i]
									}
								}

								$i++
							}

							$value = $list | Join-String -Separator ', ' 
						}
					}
					[pscustomobject]@{
						Name = $_.Name
						Value = $value
						Attributes = $_.Attributes
					}
				}
			}
		}
	}
}

Remove-Item $dll
