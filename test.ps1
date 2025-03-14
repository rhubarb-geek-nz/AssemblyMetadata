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
			$values = @()
			$names = $_.GetEnumNames()
			foreach ($value in $_.GetEnumValuesAsUnderlyingType())
			{
				$values += $names[$i] + '=' + $value
				$i++
			}
			[pscustomobject]@{
				Name = $_.Name
				Value = $values | Join-String -Separator ', '
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
						foreach ($v in $_.FieldType.GetEnumValuesAsUnderlyingType())
						{
							if ($value -eq $v)
							{
								$value = $_.FieldType.GetEnumNames()[$i]
								break
							}
							$i++
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
