// Copyright (c) 2025 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace RhubarbGeekNz.AssemblyMetadata
{
    [Cmdlet(VerbsData.Import, "AssemblyMetadata")]
    [OutputType(typeof(Assembly))]
    public class ImportAssemblyMetadata : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string [] ReferencedAssemblies;

        [Parameter(Mandatory = true, ParameterSetName = "path", Position = 0)]
        public string[] Path;

        [Parameter(Mandatory = true, ParameterSetName = "literal", ValueFromPipeline = true)]
        public string[] LiteralPath;

        [Parameter(Mandatory = true, ParameterSetName = "bytes", ValueFromPipeline = true)]
        public byte[] Value;

        protected override void ProcessRecord()
        {
            List<string> list = new List<string>();

            if (ReferencedAssemblies != null)
            {
                list.AddRange(ReferencedAssemblies);
            }
            else
            {
                list.AddRange(Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll"));
            }

            using (var mlc = new MetadataLoadContext(list))
            {
                try
                {
                    if (Path != null)
                    {
                        foreach (string path in Path)
                        {
                            var paths = GetResolvedProviderPathFromPSPath(path, out var providerPath);

                            if ("FileSystem".Equals(providerPath.Name))
                            {
                                foreach (string item in paths)
                                {
                                    WriteObject(mlc.LoadFromAssemblyPath(item));
                                }
                            }
                            else
                            {
                                WriteError(new ErrorRecord(new Exception($"Provider {providerPath.Name} not handled"), "ProviderError", ErrorCategory.NotImplemented, providerPath));
                            }
                        }
                    }

                    if (LiteralPath != null)
                    {
                        foreach (string literalPath in LiteralPath)
                        {
                            WriteObject(mlc.LoadFromAssemblyPath(literalPath));
                        }
                    }

                    if (Value != null)
                    {
                        WriteObject(mlc.LoadFromByteArray(Value));
                    }
                }
                catch (Exception ex)
                {
                    WriteError(new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.MetadataError, null));
                }
            }
        }
    }

    internal class AlcModuleAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string dependencyDirPath;

        public AlcModuleAssemblyLoadContext(string dependencyDirPath)
        {
            this.dependencyDirPath = dependencyDirPath;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = Path.Combine(
                dependencyDirPath,
                $"{assemblyName.Name}.dll");

            if (File.Exists(assemblyPath))
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }

    public class AlcModuleResolveEventHandler : IModuleAssemblyInitializer, IModuleAssemblyCleanup
    {
        private static readonly string dependencyDirPath;
        private static readonly AlcModuleAssemblyLoadContext dependencyAlc;
        private static readonly Version alcVersion;
        private static readonly string alcName;

        static AlcModuleResolveEventHandler()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            dependencyDirPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(assembly.Location), "lib"));
            dependencyAlc = new AlcModuleAssemblyLoadContext(dependencyDirPath);
            AssemblyName name = assembly.GetName();
            alcVersion = name.Version;
            alcName = name.Name + ".Alc";
        }

        public void OnImport()
        {
            AssemblyLoadContext.Default.Resolving += ResolveAlcModule;
        }

        public void OnRemove(PSModuleInfo psModuleInfo)
        {
            AssemblyLoadContext.Default.Resolving -= ResolveAlcModule;
        }

        private static Assembly ResolveAlcModule(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve)
        {
            if (alcName.Equals(assemblyToResolve.Name) && alcVersion.Equals(assemblyToResolve.Version))
            {
                return dependencyAlc.LoadFromAssemblyName(assemblyToResolve);
            }

            return null;
        }
    }
}
