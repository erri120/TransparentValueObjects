using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TransparentValueObjects.PostInitializationOutput;

namespace TransparentValueObjects;

[Generator(LanguageNames.CSharp)]
public partial class ValueObjectIncrementalSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        initContext.RegisterPostInitializationOutput(postInitContext =>
        {
            // Core
            postInitContext.AddSource(ValueObjectAttribute.HintName, ValueObjectAttribute.SourceCode);
            postInitContext.AddSource(IValueObject.HintName, IValueObject.SourceCode);
            postInitContext.AddSource(IAugment.HintName, IAugment.SourceCode);
            postInitContext.AddSource(IAugmentWith.HintName, IAugmentWith.Generate());

            // Augments
            postInitContext.AddSource(Augments.DefaultValueAugment.HintName, Augments.DefaultValueAugment.SourceCode);
            postInitContext.AddSource(Augments.DefaultEqualityComparerAugment.HintName, Augments.DefaultEqualityComparerAugment.SourceCode);
            postInitContext.AddSource(Augments.JsonAugment.HintName, Augments.JsonAugment.SourceCode);
            postInitContext.AddSource(Augments.EfCoreAugment.HintName, Augments.EfCoreAugment.SourceCode);

            // Augment interfaces
            postInitContext.AddSource(AugmentInterfaces.IDefaultValue.HintName, AugmentInterfaces.IDefaultValue.SourceCode);
            postInitContext.AddSource(AugmentInterfaces.IDefaultEqualityComparer.HintName, AugmentInterfaces.IDefaultEqualityComparer.SourceCode);
        });

        var provider = initContext.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: $"{Constants.Namespace}.{ValueObjectAttribute.Name}`1",
            predicate: static (syntaxNode, _) => syntaxNode is StructDeclarationSyntax,
            transform: Transform
        );

        initContext.RegisterSourceOutput(
            provider,
            static (ctx, target) => GenerateTarget(ctx, target)
        );
    }

    private static Target Transform(
        GeneratorAttributeSyntaxContext generatorAttributeSyntaxContext,
        CancellationToken cancellationToken)
    {
        var valueObjectSymbol = (INamedTypeSymbol)generatorAttributeSyntaxContext.TargetSymbol;
        var valueObjectAttributeData = generatorAttributeSyntaxContext.Attributes.First();

        var innerValueTypeSymbol = (INamedTypeSymbol)valueObjectAttributeData.AttributeClass!.TypeArguments.First();
        return new Target(valueObjectSymbol, innerValueTypeSymbol);
    }

    private static void GenerateTarget(SourceProductionContext context, Target target)
    {
        var (targetTypeSymbol, innerValueTypeSymbol) = target;
        if (!targetTypeSymbol.IsReadOnly)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                Diagnostics.MissingReadOnlyModified,
                targetTypeSymbol.Locations.First(),
                targetTypeSymbol.Locations.Skip(1),
                targetTypeSymbol.Name)
            );

            return;
        }

        // the namespace of the target, e.g. "TransparentValueObjects.Sample"
        var targetNamespace = targetTypeSymbol.ContainingNamespace.ToDisplayString(CustomSymbolDisplayFormats.NamespaceFormat);

        // the simple name of the target, e.g. "MyValueObject"
        var targetTypeSimpleName = targetTypeSymbol.Name;
        // the global name of the inner value type, e.g. "global::System.String"
        var innerValueTypeGlobalName = innerValueTypeSymbol.ToDisplayString(CustomSymbolDisplayFormats.GlobalFormat);
        // the nullable annotation of the inner value type. e.g. "?" for reference types and an empty string for value types
        var innerValueTypeNullableAnnotation = innerValueTypeSymbol.IsReferenceType ? "?" : "";

        // inner value type interfaces for interface forwarding
        var innerValueTypeInterfaces = innerValueTypeSymbol.Interfaces;
        var systemComparableInterfaceTypeSymbol = GetMatchingInterface(innerValueTypeInterfaces, "global::System.IComparable<T>");

        // get augments
        var augmentNames = GetAugments(context, targetTypeSymbol);
        var hasDefaultValueAugment = augmentNames.Contains(Augments.DefaultValueAugment.GlobalName);
        var hasDefaultEqualityComparerAugment = augmentNames.Contains(Augments.DefaultEqualityComparerAugment.GlobalName);
        var hasJsonAugment = augmentNames.Contains(Augments.JsonAugment.GlobalName);
        var hasEfCoreAugment = augmentNames.Contains(Augments.EfCoreAugment.GlobalName);

        // check augments
        if (hasJsonAugment && innerValueTypeSymbol.IsReferenceType && !hasDefaultValueAugment)
        {
            // if the inner value type is a reference type, the JSON augment REQUIRES the default value augment
            context.ReportDiagnostic(Diagnostic.Create(
                Diagnostics.MissingRequiredAugment,
                targetTypeSymbol.Locations.First(),
                targetTypeSymbol.Locations.Skip(1),
                targetTypeSymbol.Name,
                Augments.DefaultValueAugment.Name,
                Augments.JsonAugment.Name)
            );

            return;
        }

        // TODO: emit error when EF Core augment is used but Microsoft.EntityFrameworkCore isn't referenced

        var cw = new CodeWriter();

        // header
        cw.AppendLine(Constants.AutoGeneratedHeader);
        cw.AppendLine(Constants.NullableEnable);
        cw.AppendLine();

        // namespace
        cw.AppendLine($"namespace {targetNamespace};");
        cw.AppendLine();

        // containing symbols
        var containingSymbolStack = new Stack<INamedTypeSymbol>();
        var containingSymbol = targetTypeSymbol.ContainingSymbol;
        while (containingSymbol is not INamespaceSymbol && containingSymbol is INamedTypeSymbol containingTypeSymbol)
        {
            containingSymbolStack.Push(containingTypeSymbol);
            containingSymbol = containingSymbol.ContainingSymbol;
        }

        var codeBlockStackForContainingSymbols = new Stack<CodeWriter.CodeBlock>();
        while (containingSymbolStack.Count != 0)
        {
            var containingTypeSymbol = containingSymbolStack.Pop();
            cw.AppendLine($"partial {containingTypeSymbol.ToDisplayString(CustomSymbolDisplayFormats.ContainingSymbolFormat)}");
            codeBlockStackForContainingSymbols.Push(cw.AddBlock());
        }

        // struct declaration
        cw.AppendLine("[global::System.Diagnostics.DebuggerDisplay(\"{Value}\")]");
        cw.AppendLine("[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = \"Auto-generated.\")]");

        using (cw.AddRegionBlock("Augment Attributes"))
        {
            if (hasJsonAugment) cw.AppendLine("[global::System.Text.Json.Serialization.JsonConverter(typeof(JsonConverter))]");
        }

        cw.AppendLine($"readonly partial struct {targetTypeSimpleName} :");

        // interfaces
        using (cw.AddRegionBlock("Base Interfaces"))
        {
            cw.AppendLine($"\t{IValueObject.GlobalName}<{innerValueTypeGlobalName}>,");
            cw.AppendLine($"\tglobal::System.IEquatable<{targetTypeSimpleName}>,");
            cw.AppendLine($"\tglobal::System.IEquatable<{innerValueTypeGlobalName}>");
        }

        using (cw.AddRegionBlock("Forwarded Interfaces"))
        {
            if (systemComparableInterfaceTypeSymbol is not null)
            {
                cw.AppendLine();
                cw.Append($"\t,global::System.IComparable<{targetTypeSimpleName}>");

                cw.AppendLine();
                cw.Append($"\t,global::System.IComparable<{innerValueTypeGlobalName}>");
            }

            cw.AppendLine();
        }

        using (cw.AddRegionBlock("Augment Interfaces"))
        {
            if (hasDefaultValueAugment)
            {
                cw.AppendLine();
                cw.Append($"\t,{AugmentInterfaces.IDefaultValue.GlobalName}<{targetTypeSimpleName}, {innerValueTypeGlobalName}>");
            }

            if (hasDefaultEqualityComparerAugment)
            {
                cw.AppendLine();
                cw.Append($"\t,{AugmentInterfaces.IDefaultEqualityComparer.GlobalName}<{targetTypeSimpleName}, {innerValueTypeGlobalName}>");
            }

            cw.AppendLine();
        }

        using (cw.AddBlock())
        {
            // backing field
            cw.AppendLine("/// <summary>");
            cw.AppendLine("/// The underlying data of the value object.");
            cw.AppendLine("/// </summary>");
            cw.AppendLine($"public readonly {innerValueTypeGlobalName} Value;");
            cw.AppendLine();

            // IValueObject<TInnerValue> implementation
            cw.AppendLine(Constants.InheritDocumentation);
            cw.AppendLine($"public static global::System.Type InnerValueType => typeof({innerValueTypeGlobalName});");
            cw.AppendLine();

            // From method
            cw.AppendLine("/// <summary>");
            cw.AppendLine("/// Creates a new instance of the value object using the provided inner value.");
            cw.AppendLine("/// </summary>");
            cw.AppendLine("/// <param name=\"value\">The value that the value object should wrap.</param>");
            cw.AppendLine("/// <returns>A new instance of this value object.</returns>");
            cw.AppendLine($"public static {targetTypeSimpleName} From({innerValueTypeGlobalName} value) => new(value);");
            cw.AppendLine();

            AddConstructors(cw, targetTypeSimpleName, innerValueTypeGlobalName, hasDefaultValueAugment);

            OverrideBaseMethods(cw, hasDefaultEqualityComparerAugment);

            ImplementEqualsMethods(cw, targetTypeSimpleName, innerValueTypeGlobalName, innerValueTypeNullableAnnotation, hasDefaultEqualityComparerAugment);

            AddEqualityOperators(cw, targetTypeSimpleName, innerValueTypeGlobalName);
            AddExplicitCastOperators(cw, targetTypeSimpleName, innerValueTypeGlobalName);

            if (systemComparableInterfaceTypeSymbol is not null)
            {
                ImplementComparable(cw, targetTypeSimpleName, innerValueTypeGlobalName, innerValueTypeNullableAnnotation);
                AddComparisonOperators(cw, targetTypeSimpleName, innerValueTypeGlobalName);
            }

            // custom code
            if (string.Equals(innerValueTypeGlobalName, "global::System.Guid", StringComparison.Ordinal))
                AddGuidSpecificCode(cw, targetTypeSimpleName, innerValueTypeGlobalName);

            // augments
            if (hasJsonAugment) JsonAugmentWriter.AddJsonConverter(cw, targetTypeSimpleName, innerValueTypeGlobalName, isReferenceType: innerValueTypeSymbol.IsReferenceType);
            if (hasEfCoreAugment) AddEfCoreConverterComparer(cw, targetTypeSimpleName, innerValueTypeGlobalName);
        }

        while (codeBlockStackForContainingSymbols.Count != 0)
        {
            codeBlockStackForContainingSymbols.Pop().Dispose();
        }

        var hintName = targetTypeSymbol.ToDisplayString(CustomSymbolDisplayFormats.HintNameFormat);
        context.AddSource($"{hintName}.g.cs", cw.ToString());
    }

    private static HashSet<string> GetAugments(SourceProductionContext context, ITypeSymbol targetTypeSymbol)
    {
        var augments = new HashSet<string>(StringComparer.Ordinal);

        var interfaceTypeSymbols = targetTypeSymbol.AllInterfaces;
        foreach (var interfaceTypeSymbol in interfaceTypeSymbols)
        {
            var globalName = interfaceTypeSymbol.ToDisplayString(CustomSymbolDisplayFormats.GlobalFormat);
            if (!globalName.StartsWith($"{IAugmentWith.GlobalName}<", StringComparison.Ordinal)) continue;

            var typeArguments = interfaceTypeSymbol.TypeArguments;
            foreach (var typeArgument in typeArguments)
            {
                if (typeArgument is not INamedTypeSymbol typeArgumentSymbol) continue;
                var augmentName = typeArgumentSymbol.ToDisplayString(CustomSymbolDisplayFormats.GlobalFormat);

                if (augments.Contains(augmentName))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        Diagnostics.DuplicateAugment,
                        targetTypeSymbol.Locations.First(),
                        targetTypeSymbol.Locations.Skip(1),
                        targetTypeSymbol.Name,
                        typeArgumentSymbol.Name)
                    );

                    continue;
                }

                augments.Add(augmentName);
            }
        }

        return augments;
    }

    private static INamedTypeSymbol? GetMatchingInterface(
        ImmutableArray<INamedTypeSymbol> interfaceTypeSymbols,
        string targetInterfaceGlobalName)
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var interfaceTypeSymbol in interfaceTypeSymbols)
        {
            var globalName = interfaceTypeSymbol.ConstructedFrom.ToDisplayString(CustomSymbolDisplayFormats.GlobalFormat);
            if (string.Equals(globalName, targetInterfaceGlobalName, StringComparison.Ordinal))
            {
                return interfaceTypeSymbol;
            }
        }

        return null;
    }

    public static void AddConstructors(CodeWriter cw, string targetTypeSimpleName, string innerValueTypeGlobalName, bool hasDefaultValue)
    {
        using var _ = cw.AddRegionBlock("Constructors");

        // private constructor
        cw.AppendLine($"private {targetTypeSimpleName}({innerValueTypeGlobalName} value)");
        using (cw.AddBlock())
        {
            cw.AppendLine("Value = value;");
        }

        // public constructor
        if (hasDefaultValue)
        {
            cw.AppendLine("/// <summary>");
            cw.AppendLine("/// Default constructor using <see cref=\"DefaultValue\"/> as the underlying value.");
            cw.AppendLine("/// </summary>");
            cw.AppendLine($"public {targetTypeSimpleName}()");

            using (cw.AddBlock())
            {
                cw.AppendLine("Value = DefaultValue.Value;");
            }
        }
        else
        {
            var errorMessage = $"Use {targetTypeSimpleName}.{{nameof(From)}} instead.";

            cw.AppendLine("/// <summary>");
            cw.AppendLine("/// The default constructor is disabled and will throw an exception if called.");
            cw.AppendLine("/// </summary>");
            cw.AppendLine("/// <remarks>");
            cw.AppendLine("/// The default constructor can be enabled by providing a default value");
            cw.AppendLine($"/// using the <see cref=\"{Augments.DefaultValueAugment.GlobalName}\"/> augment.");
            cw.AppendLine("/// </remarks>");
            cw.AppendLine("/// <exception cref=\"global::System.InvalidOperationException\">Thrown when called.</exception>");
            cw.AppendLine($"[global::System.Obsolete($\"{errorMessage}\", error: true)]");
            cw.AppendLine($"public {targetTypeSimpleName}()");

            using (cw.AddBlock())
            {
                cw.AppendLine($"throw new global::System.InvalidOperationException($\"{errorMessage}\");");
            }
        }
    }

    public static void OverrideBaseMethods(CodeWriter cw, bool hasDefaultEqualityComparer)
    {
        using var _ = cw.AddRegionBlock("Base Methods");

        // GetHashCode
        cw.AppendLine(Constants.InheritDocumentation);
        cw.AppendLine(Constants.InlineAttribute);

        if (hasDefaultEqualityComparer)
        {
            cw.AppendLine("public override int GetHashCode() => InnerValueDefaultEqualityComparer.GetHashCode(Value);");
        }
        else
        {
            cw.AppendLine("public override int GetHashCode() => Value.GetHashCode();");
        }

        cw.AppendLine();

        // ToString
        cw.AppendLine(Constants.InheritDocumentation);
        cw.AppendLine(Constants.InlineAttribute);
        cw.AppendLine("public override string ToString() => Value.ToString();");
        cw.AppendLine();
    }

    public static void ImplementEqualsMethods(
        CodeWriter cw,
        string targetTypeSimpleName,
        string innerValueTypeGlobalName,
        string innerValueTypeNullableAnnotation,
        bool hasDefaultEqualityComparer)
    {
        using var _ = cw.AddRegionBlock("Equals Methods");

        // IEquality<TSelf>
        cw.AppendLine(Constants.InheritDocumentation);
        cw.AppendLine(Constants.InlineAttribute);
        cw.AppendLine($"public bool Equals({targetTypeSimpleName} other) => Equals(other.Value);");
        cw.AppendLine();

        // IEquality<TInnerValue>
        if (hasDefaultEqualityComparer)
        {
            cw.AppendLine(Constants.InheritDocumentation);
            cw.AppendLine(Constants.InlineAttribute);
            cw.AppendLine($"public bool Equals({innerValueTypeGlobalName}{innerValueTypeNullableAnnotation} other) => InnerValueDefaultEqualityComparer.Equals(Value, other);");
        }
        else
        {
            cw.AppendLine(Constants.InheritDocumentation);
            cw.AppendLine(Constants.InlineAttribute);
            cw.AppendLine($"public bool Equals({innerValueTypeGlobalName}{innerValueTypeNullableAnnotation} other) => Value.Equals(other);");
        }

        cw.AppendLine();

        // with any equality comparer
        cw.AppendLine("/// <summary>");
        cw.AppendLine("/// Determines whether the specified value is equal to the current inner value using a custom equality comparer.");
        cw.AppendLine("/// </summary>");
        cw.AppendLine("/// <param name=\"other\">The value to compare with the current inner value.</param>");
        cw.AppendLine("/// <param name=\"comparer\">The comparer to use.</param>");
        cw.AppendLine("/// <returns></returns>");
        cw.AppendLine(Constants.InlineAttribute);
        cw.AppendLine($"public bool Equals({innerValueTypeGlobalName} other, global::System.Collections.Generic.IEqualityComparer<{innerValueTypeGlobalName}> comparer) => comparer.Equals(Value, other);");
        cw.AppendLine();

        // object.Equals
        cw.AppendLine(Constants.InheritDocumentation);
        cw.AppendLine("public override bool Equals(object? obj)");
        using (cw.AddBlock())
        {
            cw.AppendLine("if (obj is null) return false;");
            cw.AppendLine($"if (obj is {targetTypeSimpleName} value) return Equals(value);");
            cw.AppendLine($"if (obj is {innerValueTypeGlobalName} innerValue) return Equals(innerValue);");
            cw.AppendLine("return false;");
        }
    }

    public static void AddEqualityOperators(CodeWriter cw, string targetTypeSimpleName, string innerValueTypeGlobalName)
    {
        using var _ = cw.AddRegionBlock("Equality Operators");

        cw.AppendLine($"public static bool operator ==({targetTypeSimpleName} left, {targetTypeSimpleName} right) => left.Equals(right);");
        cw.AppendLine($"public static bool operator !=({targetTypeSimpleName} left, {targetTypeSimpleName} right) => !left.Equals(right);");
        cw.AppendLine();

        cw.AppendLine($"public static bool operator ==({targetTypeSimpleName} left, {innerValueTypeGlobalName} right) => left.Equals(right);");
        cw.AppendLine($"public static bool operator !=({targetTypeSimpleName} left, {innerValueTypeGlobalName} right) => !left.Equals(right);");
        cw.AppendLine();

        cw.AppendLine($"public static bool operator ==({innerValueTypeGlobalName} left, {targetTypeSimpleName} right) => right.Equals(left);");
        cw.AppendLine($"public static bool operator !=({innerValueTypeGlobalName} left, {targetTypeSimpleName} right) => !right.Equals(left);");
        cw.AppendLine();
    }

    public static void AddExplicitCastOperators(CodeWriter cw, string targetTypeSimpleName, string innerValueTypeGlobalName)
    {
        using var _ = cw.AddRegionBlock("Explicit Cast Operators");

        cw.AppendLine($"public static explicit operator {targetTypeSimpleName}({innerValueTypeGlobalName} value) => From(value);");
        cw.AppendLine($"public static explicit operator {innerValueTypeGlobalName}({targetTypeSimpleName} value) => value.Value;");
        cw.AppendLine();
    }

    public static void ImplementComparable(
        CodeWriter cw,
        string targetTypeSimpleName,
        string innerValueTypeGlobalName,
        string innerValueTypeNullableAnnotation)
    {
        using var _ = cw.AddRegionBlock("IComparable Implementation");

        cw.AppendLine(Constants.InheritDocumentation);
        cw.AppendLine($"public global::System.Int32 CompareTo({targetTypeSimpleName} other) => Value.CompareTo(other.Value);");

        cw.AppendLine(Constants.InheritDocumentation);
        cw.AppendLine($"public global::System.Int32 CompareTo({innerValueTypeGlobalName}{innerValueTypeNullableAnnotation} other) => Value.CompareTo(other);");
    }

    public static void AddComparisonOperators(
        CodeWriter cw,
        string targetTypeSimpleName,
        string innerValueTypeGlobalName)
    {
        using var _ = cw.AddRegionBlock("Comparison Operators");

        cw.AppendLine($"public static bool operator <({targetTypeSimpleName} left, {targetTypeSimpleName} right) => left.Value.CompareTo(right.Value) < 0;");
        cw.AppendLine($"public static bool operator >({targetTypeSimpleName} left, {targetTypeSimpleName} right) => left.Value.CompareTo(right.Value) > 0;");
        cw.AppendLine($"public static bool operator <=({targetTypeSimpleName} left, {targetTypeSimpleName} right) => left.Value.CompareTo(right.Value) <= 0;");
        cw.AppendLine($"public static bool operator >=({targetTypeSimpleName} left, {targetTypeSimpleName} right) => left.Value.CompareTo(right.Value) >= 0;");
        cw.AppendLine();

        cw.AppendLine($"public static bool operator <({innerValueTypeGlobalName} left, {targetTypeSimpleName} right) => left.CompareTo(right.Value) < 0;");
        cw.AppendLine($"public static bool operator >({innerValueTypeGlobalName} left, {targetTypeSimpleName} right) => left.CompareTo(right.Value) > 0;");
        cw.AppendLine($"public static bool operator <=({innerValueTypeGlobalName} left, {targetTypeSimpleName} right) => left.CompareTo(right.Value) <= 0;");
        cw.AppendLine($"public static bool operator >=({innerValueTypeGlobalName} left, {targetTypeSimpleName} right) => left.CompareTo(right.Value) >= 0;");
        cw.AppendLine();

        cw.AppendLine($"public static bool operator <({targetTypeSimpleName} left, {innerValueTypeGlobalName} right) => left.Value.CompareTo(right) < 0;");
        cw.AppendLine($"public static bool operator >({targetTypeSimpleName} left, {innerValueTypeGlobalName} right) => left.Value.CompareTo(right) > 0;");
        cw.AppendLine($"public static bool operator <=({targetTypeSimpleName} left, {innerValueTypeGlobalName} right) => left.Value.CompareTo(right) <= 0;");
        cw.AppendLine($"public static bool operator >=({targetTypeSimpleName} left, {innerValueTypeGlobalName} right) => left.Value.CompareTo(right) >= 0;");
        cw.AppendLine();
    }

    public static void AddGuidSpecificCode(CodeWriter cw, string targetTypeSimpleName, string innerValueTypeGlobalName)
    {
        using var _ = cw.AddRegionBlock("GUID Specific Code");

        cw.AppendLine($"public static {targetTypeSimpleName} NewId() => From({innerValueTypeGlobalName}.NewGuid());");
        cw.AppendLine();
    }

    public static void AddEfCoreConverterComparer(
        CodeWriter cw,
        string targetTypeSimpleName,
        string innerValueTypeGlobalName)
    {
        using var _ = cw.AddRegionBlock("EF Core Augment");

        cw.AppendLine("/// <summary>");
        cw.AppendLine($"/// <see cref=\"global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter{{TModel,TProvider}}\"/> between <see cref=\"{targetTypeSimpleName}\"/> and <see cref=\"{innerValueTypeGlobalName}\"/>.");
        cw.AppendLine("/// </summary>");
        cw.AppendLine($"public class EfCoreValueConverter : global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<{targetTypeSimpleName}, {innerValueTypeGlobalName}>");
        using (cw.AddBlock())
        {
            cw.AppendLine("public EfCoreValueConverter() : this(mappingHints: null) { }");
            cw.AppendLine();

            cw.AppendLine("public EfCoreValueConverter(global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ConverterMappingHints? mappingHints = null) : base(");
            cw.AppendLine("\tstatic value => value.Value,");
            cw.AppendLine("\tstatic innerValue => From(innerValue),");
            cw.AppendLine("\tmappingHints");
            cw.AppendLine(") { }");
        }

        cw.AppendLine("/// <summary>");
        cw.AppendLine($"/// <see cref=\"global::Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer{{T}}\"/> for <see cref=\"{targetTypeSimpleName}\"/>.");
        cw.AppendLine("/// </summary>");
        cw.AppendLine($"public class EfCoreValueComparer : global::Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<{targetTypeSimpleName}>");
        using (cw.AddBlock())
        {
            cw.AppendLine("public EfCoreValueComparer() : base(");
            cw.AppendLine("\tstatic (left, right) => left.Equals(right),");
            cw.AppendLine("\tstatic value => value.GetHashCode(),");
            cw.AppendLine("\tstatic value => From(value.Value)");
            cw.AppendLine(") { }");
            cw.AppendLine();

            cw.AppendLine(Constants.InheritDocumentation);
            cw.AppendLine($"public override bool Equals({targetTypeSimpleName} left, {targetTypeSimpleName} right) => left.Equals(right);");
            cw.AppendLine();

            cw.AppendLine(Constants.InheritDocumentation);
            cw.AppendLine($"public override {targetTypeSimpleName} Snapshot({targetTypeSimpleName} instance) => From(instance.Value);");
            cw.AppendLine();

            cw.AppendLine(Constants.InheritDocumentation);
            cw.AppendLine($"public override int GetHashCode({targetTypeSimpleName} instance) => instance.GetHashCode();");
            cw.AppendLine();
        }
    }

    private record struct Target(INamedTypeSymbol ValueObjectSymbol, INamedTypeSymbol InnerValueTypeSymbol);
}
