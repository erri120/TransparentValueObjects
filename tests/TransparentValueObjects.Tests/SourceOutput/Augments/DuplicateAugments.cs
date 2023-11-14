using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

public class DuplicateAugments
{
    [Fact]
    public void TestDuplicateAugments()
    {
        const string input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject : IAugmentWith<DefaultValueAugment, DefaultValueAugment> { }
""";

        var diagnostics = TestHelpers.GetDiagnostics(input);
        diagnostics.Should().ContainSingle();

        var diagnostic = diagnostics[0];
        diagnostic.Id.Should().Be("ERRI_TVO_0002");
        diagnostic.Severity.Should().Be(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().Should().Be("The Value Object 'TestValueObject' is already augment with 'DefaultValueAugment'. Duplicate Augments will be ignored.");
        diagnostic.Location.ToString().Should().Be("SourceFile([125..140))");
    }
}
