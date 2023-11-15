using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput;

[UsesVerify]
public class DefaultWithString
{
    private const string Input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject { }
""";

    [Fact]
    public Task TestGenerator()
    {
        return TestHelpers.VerifyGenerator("TestNamespace.TestValueObject.g.cs", Input);
    }
}

