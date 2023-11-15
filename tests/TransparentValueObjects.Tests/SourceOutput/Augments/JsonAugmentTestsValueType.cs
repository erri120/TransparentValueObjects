using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

[UsesVerify]
public class JsonAugmentTestsValueType
{
    private const string Input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<int>]
public readonly partial struct TestValueObject : IAugmentWith<JsonAugment> { }
""";

    [Fact]
    public Task Test_AugmentAttributes()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "Augment Attributes");
    }

    [Fact]
    public Task Test_JsonAugment()
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", Input);
        return TestHelpers.VerifyRegion(output, "JSON Augment");
    }
}
