#!/usr/bin/env pwsh
# Copyright (c) 2025 Roger Brown.
# Licensed under the MIT License.

$ErrorActionPreference = 'Stop'

dotnet publish TestModule/TestModule.csproj --configuration Release

$dll = 'TestModule/bin/Release/netstandard2.0/publish/TestModule.dll'

Get-Content -LiteralPath $dll -AsByteStream -Raw | Import-AssemblyMetadata | ForEach-Object {
	$_.GetTypes() | ForEach-Object {
		if ($_.IsEnum)
		{
			[pscustomobject]@{
				Name = $_.Name
				Value = $_.GetEnumNames()
			}
		}
		else
		{
			$_.GetFields() | ForEach-Object {
				if ($_.IsLiteral)
				{
					[pscustomobject]@{
						Name = $_.Name
						Value = $_.GetRawConstantValue()
					}
				}
			}
		}
	}
}

Remove-Item $dll
