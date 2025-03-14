// Copyright (c) 2025 Roger Brown.
// Licensed under the MIT License.

namespace RhubarbGeekNz.AssemblyMetadata
{
    public enum TestEnum
    {
        Foo,
        Bar
    }

    public class TestModule
    {
        public const string Message = "Hello World";
        public const int TheAnswer = 42;
        public const TestEnum MyEnum = TestEnum.Bar;
        public const double PI = System.Math.PI;
        public const bool TheTruth = true;
    }
}
