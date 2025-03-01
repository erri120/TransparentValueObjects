using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

public class JsonAugmentTestsReferenceType
{
    private const string Input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject : IAugmentWith<DefaultValueAugment, JsonAugment> { }
""";

    [Fact]
    public Task Test_AugmentAttributes()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "Augment Attributes");
    }

    [Fact]
    public Task Test_AugmentInterfaces()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "Augment Interfaces");
    }

    [Fact]
    public Task Test_JsonAugment()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "JSON Augment");
    }
}
