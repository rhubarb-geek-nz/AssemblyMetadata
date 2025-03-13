// Copyright (c) 2025 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace RhubarbGeekNz.AssemblyMetadata
{
    public class MetadataLoadContext : IDisposable
    {
        private readonly System.Reflection.MetadataLoadContext _context;

        public MetadataLoadContext(IEnumerable<string> assemblyPaths)
        {
            _context = new System.Reflection.MetadataLoadContext(new PathAssemblyResolver(assemblyPaths));
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Assembly LoadFromAssemblyPath(string path)
        {
            return _context.LoadFromAssemblyPath(path);
        }

        public Assembly LoadFromByteArray(byte[] assembly)
        {
            return _context.LoadFromByteArray(assembly);
        }
    }
}
