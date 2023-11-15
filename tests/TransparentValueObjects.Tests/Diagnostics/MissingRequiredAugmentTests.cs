using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace TransparentValueObjects.Tests.Diagnostics;

public class MissingRequiredAugmentTests
{
    [Fact]
    public void TestDiagnostic()
    {
        const string input = /*lang=csharp*/
"""
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct MyValueObject : IAugmentWith<JsonAugment> { }
""";

        var diagnostics = TestHelpers.GetDiagnostics(input.SourceNormalize());
        diagnostics.Should().ContainSingle();

        var diagnostic = diagnostics[0];
        diagnostic.Id.Should().Be("ERRI_TVO_0003");
        diagnostic.Severity.Should().Be(DiagnosticSeverity.Error);
        diagnostic.GetMessage().Should().Be("The Value Object 'MyValueObject' is missing Augment 'DefaultValueAugment' which is required by Augment 'JsonAugment'");
        diagnostic.Location.ToString().Should().Be("SourceFile([111..124))");
    }
}
