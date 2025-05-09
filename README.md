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
Restore complete (0.2s)
  TestModule succeeded (0.1s) → TestModule\bin\Release\netstandard2.0\publish\

Build succeeded in 0.7s

Name         Value                                               Attributes
----         -----                                               ----------
SimpleEnum   Foo, Bar                                        Public, Sealed
FlaggedEnum  None=0, Foo=16, Bar=32                          Public, Sealed
OutOfOrder   None=0, Two=2, Four=4                           Public, Sealed
Message      Hello World                Public, Static, Literal, HasDefault
TheAnswer    42                         Public, Static, Literal, HasDefault
MyEnum       Bar                        Public, Static, Literal, HasDefault
MyPI         3.14                       Public, Static, Literal, HasDefault
TheTruth     True                       Public, Static, Literal, HasDefault
MyAttributes Public, Static, HasDefault Public, Static, Literal, HasDefault
```
