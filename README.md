# AssemblyMetadata

Tool to assist importing assembly metadata

Build the module with

```
$ dotnet publish AssemblyMetadata/AssemblyMetadata.csproj --configuration Release
```

Install by copying the module into a directory on the [PSModulePath](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_psmodulepath)

```
Import-AssemblyMetadata [-Path] <string[]> [-ReferencedAssemblies <string[]>]

Import-AssemblyMetadata -LiteralPath <string[]> [-ReferencedAssemblies <string[]>]

Import-AssemblyMetadata -Value <byte[]> [-ReferencedAssemblies <string[]>]
```

Run the [test.ps1](test.ps1) to confirm it works.

```
$ pwsh test.ps1
Restore complete (2.3s)
  TestModule succeeded (1.3s) → TestModule/bin/Release/netstandard2.0/publish/

Build succeeded in 5.4s

Name      Value
----      -----
TestEnum  {Foo, Bar}
Message   Hello World
TheAnswer 42
```
