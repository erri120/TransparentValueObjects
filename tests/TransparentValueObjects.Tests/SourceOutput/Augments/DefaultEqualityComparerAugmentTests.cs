using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

[UsesVerify]
public class DefaultEqualityComparerAugmentTests
{
    private const string Input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject : IAugmentWith<DefaultEqualityComparerAugment> { }
""";

    [Fact]
    public Task Test_AugmentInterfaces()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "Equals Methods");
    }

    [Fact]
    public Task Test_EqualsMethods()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "Equals Methods");
    }
}
