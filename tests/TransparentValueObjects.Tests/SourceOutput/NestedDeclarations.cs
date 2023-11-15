using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput;

[UsesVerify]
public class NestedDeclarations
{
    private const string Input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace.Foo.Bar.Baz;

public class MyClass
{
    public class MyNestedClass
    {
        [ValueObject<string>]
        public readonly partial struct TestValueObject { }
    }
}
""";

    [Fact]
    public Task TestGenerator()
    {
        return TestHelpers.VerifyGenerator("TestNamespace.Foo.Bar.Baz.MyClass.MyNestedClass.TestValueObject.g.cs", Input);
    }
}
