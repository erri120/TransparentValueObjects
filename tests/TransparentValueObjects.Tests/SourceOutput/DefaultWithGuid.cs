using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput;

[UsesVerify]
public class DefaultWithGuid
{
    private const string Input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<Guid>]
public readonly partial struct TestValueObject { }
""";

    [Fact]
    public Task TestGenerator()
    {
        return TestHelpers.VerifyGenerator("TestNamespace.TestValueObject.g.cs", Input);
    }
}
