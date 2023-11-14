using Microsoft.CodeAnalysis;

namespace TransparentValueObjects;

public static class Diagnostics
{
    private const string UsageCategory = "Usage";

    public static readonly DiagnosticDescriptor MissingReadOnlyModified = new(
        id: "ERRI_TVO_0001",
        title: "Missing readonly modifier",
        messageFormat: "The Value Object '{0}' should be marked as readonly",
        category: UsageCategory,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: null,
        helpLinkUri: null
    );

    public static readonly DiagnosticDescriptor DuplicateAugment = new(
        id: "ERRI_TVO_0002",
        title: "Duplicate Augment",
        messageFormat: "The Value Object '{0}' is already augment with '{1}'. Duplicate Augments will be ignored.",
        category: UsageCategory,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: null,
        helpLinkUri: null
    );
}
