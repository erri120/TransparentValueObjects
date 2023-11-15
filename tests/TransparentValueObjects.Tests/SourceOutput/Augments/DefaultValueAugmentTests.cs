using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

[UsesVerify]
public class DefaultValueAugmentTests
{
    private const string Input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject : IAugmentWith<DefaultValueAugment> { }
""";

    [Fact]
    public Task Test_AugmentInterfaces()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "Augment Interfaces");
    }

    [Fact]
    public Task Test_Constructors()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "Constructors");
    }
}
