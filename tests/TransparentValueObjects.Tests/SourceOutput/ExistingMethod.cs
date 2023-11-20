using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput;

[UsesVerify]
public class ExistingMethod
{
    [Fact]
    public Task Test_WithExistingMethod()
    {
        const string input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject
{
    public override int GetHashCode() => 0;
    public override string ToString() => "nope";

    public bool Equals(string? other) => false;
    public bool Equals(string other, System.Collections.Generic.IEqualityComparer<global::System.String> comparer) => false;
    public override bool Equals(object? obj) => false;
}
""";

        return TestHelpers.VerifyGenerator("TestNamespace.TestValueObject.g.cs", input);
    }
}
