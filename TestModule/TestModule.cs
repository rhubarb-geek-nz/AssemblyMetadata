// Copyright (c) 2025 Roger Brown.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Reflection;

namespace RhubarbGeekNz.AssemblyMetadata
{
    public enum SimpleEnum
    {
        Foo,
        Bar
    }

    [Flags]
    public enum FlaggedEnum
    {
        None = 0,
        Foo = 16,
        Bar = 32
    }

    public enum OutOfOrder
    {
        None = 0,
        Four = 4,
        Two = 2
    }

    public class TestModule
    {
        public const string Message = "Hello World";
        public const int TheAnswer = 42;
        public const SimpleEnum MyEnum = SimpleEnum.Bar;
        public const double MyPI = System.Math.PI;
        public const bool TheTruth = true;
        public const FieldAttributes MyAttributes = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.HasDefault;
        public const FileAttributes MyFiles = FileAttributes.SparseFile | FileAttributes.Archive | FileAttributes.ReadOnly;
    }
}
