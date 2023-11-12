using Microsoft.CodeAnalysis;

namespace TransparentValueObjects;

public static class Diagnostics
{
    private const string UsageCategory = "Usage";

    public static readonly DiagnosticDescriptor MissingReadOnlyModified = new(
        id: "TV0001",
        title: "Missing readonly modifier",
        messageFormat: "The Value Object '{0}' should be marked as readonly",
        category: UsageCategory,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: null,
        helpLinkUri: null // TODO:
    );
}
