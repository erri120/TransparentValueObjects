using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

[UsesVerify]
public class JsonAugmentTestsNumbers
{
    private static string GenerateCode(string typeName)
    {
        return
$$"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<{{typeName}}>]
public readonly partial struct TestValueObject : IAugmentWith<JsonAugment> { }
""";
    }

    private static Task Verify(string typeName)
    {
        var output = TestHelpers.RunGenerator("TestNamespace.TestValueObject.g.cs", GenerateCode(typeName));
        return TestHelpers.VerifyRegion(output, "JSON Augment");
    }

    [Fact]
    public Task Test_Int16() => Verify("short");

    [Fact]
    public Task Test_Int32() => Verify("int");

    [Fact]
    public Task Test_Int64() => Verify("long");

    [Fact]
    public Task Test_UInt16() => Verify("ushort");

    [Fact]
    public Task Test_UInt32() => Verify("uint");

    [Fact]
    public Task Test_UInt64() => Verify("ulong");
}
