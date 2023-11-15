using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput;

public class NameCollisionTests
{
    [Fact]
    public void Test_NameCollision()
    {
        const string input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

public partial class ClassA
{
    [ValueObject<string>]
    public readonly partial struct TestValueObject { }
}

public partial class ClassB
{
    [ValueObject<string>]
    public readonly partial struct TestValueObject { }
}

""";

        var runResult = TestHelpers.RunGenerator(input);
        runResult.GeneratedSources.Should().Contain(res => string.Equals(res.HintName, "TestNamespace.ClassA.TestValueObject.g.cs", StringComparison.Ordinal));
        runResult.GeneratedSources.Should().Contain(res => string.Equals(res.HintName, "TestNamespace.ClassB.TestValueObject.g.cs", StringComparison.Ordinal));
    }
}
