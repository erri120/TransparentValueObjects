using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace TransparentValueObjects.Tests.Diagnostics;

public class MissingReadOnlyModifiedTests
{
    [Fact]
    public void TestDiagnostic()
    {
        const string input = /*lang=csharp*/
"""
namespace TestNamespace;

[TransparentValueObjects.ValueObject<System.Guid>]
public partial struct MyValueObject { }
""";

        var diagnostics = TestHelpers.GetDiagnostics(input.SourceNormalize());
        diagnostics.Should().ContainSingle();

        var diagnostic = diagnostics[0];
        diagnostic.Id.Should().Be("ERRI_TVO_0001");
        diagnostic.Severity.Should().Be(DiagnosticSeverity.Warning);
        diagnostic.GetMessage().Should().Be("The Value Object 'MyValueObject' should be marked as readonly");
        diagnostic.Location.ToString().Should().Be("SourceFile([99..112))");
    }
}
