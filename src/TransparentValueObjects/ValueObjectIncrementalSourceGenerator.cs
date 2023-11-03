using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TransparentValueObjects;

[Generator]
public class ValueObjectIncrementalSourceGenerator : IIncrementalGenerator
{
    private const string GeneratedNamespace = "TransparentValueObjects.Generated";
    private const string AttributeClassName = "ValueObjectAttribute";

    private const string AugmentedNamespace = "TransparentValueObjects.Augments";
    private const string ValueObjectInterfaceName = "IValueObject";
    private const string HasDefaultValueInterfaceName = "IHasDefaultValue";
    private const string HasDefaultEqualityComparerInterfaceName = "IHasDefaultEqualityComparer";

    private const string AttributeSourceCode =
$$"""
// <auto-generated/>

namespace {{GeneratedNamespace}}
{
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Auto-generated")]
    [global::System.AttributeUsage(global::System.AttributeTargets.Struct)]
    public class {{AttributeClassName}}<T> : global::System.Attribute { }
}
""";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            $"{AttributeClassName}.g.cs",
            SourceText.From(AttributeSourceCode, Encoding.UTF8))
        );

        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                Predicate,
                Transform
            )
            .Where(t => !t.Equals(default));

        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(provider.Collect()),
            static (ctx, tuple) => Generate(ctx, tuple.Left, tuple.Right)
        );
    }

    private static bool Predicate(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode is StructDeclarationSyntax { AttributeLists.Count: > 0 };
    }

    private static Target Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var syntaxNode = (StructDeclarationSyntax) context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(syntaxNode, cancellationToken);

        if (symbol is null) return default;

        var attributes = symbol.GetAttributes();
        if (attributes.Length == 0) return default;

        var attributeData = attributes.FirstOrDefault(static a => a?.AttributeClass?.Name == $"{AttributeClassName}");
        if (attributeData is null) return default;

        return new Target(syntaxNode, attributeData);
    }

    private static void Generate(SourceProductionContext context, Compilation compilation, ImmutableArray<Target> targets)
    {
        foreach (var target in targets)
        {
            var valueObjectDeclarationSyntax = target.Syntax;
            var attributeData = target.AttributeData;

            var semanticModel = compilation.GetSemanticModel(valueObjectDeclarationSyntax.SyntaxTree);
            var valueObjectTypeSymbol = semanticModel.GetDeclaredSymbol(valueObjectDeclarationSyntax);

            if (valueObjectTypeSymbol is not INamedTypeSymbol valueObjectNamedTypeSymbol) continue;

            var namespaceName = valueObjectNamedTypeSymbol.ContainingNamespace.ToDisplayString();
            var valueObjectTypeName = valueObjectNamedTypeSymbol.Name;

            var innerValueTypeSymbol = attributeData.AttributeClass?.TypeArguments.FirstOrDefault();
            if (innerValueTypeSymbol is not INamedTypeSymbol innerValueNamedTypeSymbol) continue;

            var innerValueTypeName = innerValueNamedTypeSymbol.ToDisplayString(CustomSymbolDisplayFormats.GlobalFormat);
            var innerValueTypeNullableAnnotation = innerValueNamedTypeSymbol.IsReferenceType ? "?" : "";

            var innerValueInterfaces = innerValueNamedTypeSymbol.Interfaces;
            var valueObjectInterfaces = valueObjectNamedTypeSymbol.Interfaces;

            var cw = new CodeWriter();

            // header, namespace and type definition
            cw.AppendLine("// <auto-generated/>");
            cw.AppendLine("#nullable enable");
            cw.AppendLine($"namespace {namespaceName};");
            cw.AppendLine();

            cw.AppendLine("[global::System.Diagnostics.DebuggerDisplay(\"{Value}\")]");
            cw.AppendLine("[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = \"Auto-generated.\")]");
            cw.AppendLine($"readonly partial struct {valueObjectTypeName} :");

            // interfaces
            cw.AppendLine($"\tglobal::{AugmentedNamespace}.{ValueObjectInterfaceName}<{innerValueTypeName}>,");
            cw.AppendLine($"\tglobal::System.IEquatable<{valueObjectTypeName}>,");
            cw.Append($"\tglobal::System.IEquatable<{innerValueTypeName}>");

            var comparableInterfaceTypeSymbol = GetInterfaceWithInnerTypeArgument(innerValueInterfaces, innerValueTypeSymbol, typeof(IComparable<>));
            if (comparableInterfaceTypeSymbol is not null)
            {
                cw.AppendLine(",");
                cw.Append($"\tglobal::System.IComparable<{valueObjectTypeName}>");
            }

            cw.AppendLine();

            using (cw.AddBlock())
            {
                // backing field
                cw.AppendLine($"public readonly {innerValueTypeName} Value;");
                cw.AppendLine();

                // public default constructor
                var hasDefaultValue = HasAugment(valueObjectInterfaces, HasDefaultValueInterfaceName);
                AddPublicConstructor(cw, valueObjectTypeName, hasDefaultValue);

                // private constructor with value
                AddPrivateConstructor(cw, valueObjectTypeName, innerValueTypeName);

                // public From method
                cw.AppendLine($"public static {valueObjectTypeName} From({innerValueTypeName} value) => new(value);");
                cw.AppendLine();

                // ToString and GetHashCode
                OverrideBaseMethods(cw);

                // Equals methods from interfaces and base object
                var hasDefaultEqualityComparer = HasAugment(valueObjectInterfaces, HasDefaultEqualityComparerInterfaceName);
                ImplementEqualsMethods(cw, valueObjectTypeName, innerValueTypeName, innerValueTypeNullableAnnotation, hasDefaultEqualityComparer);

                // equality operators
                AddEqualityOperators(cw, valueObjectTypeName, innerValueTypeName);

                // explicit cast operators
                AddExplicitCastOperators(cw, valueObjectTypeName, innerValueTypeName);

                if (comparableInterfaceTypeSymbol is not null)
                    ForwardInterface(cw, valueObjectTypeName, comparableInterfaceTypeSymbol);

                if (innerValueTypeName == "global::System.Guid")
                    AddGuidSpecificCode(cw, valueObjectTypeName, innerValueTypeName);
            }

            context.AddSource($"{valueObjectTypeName}.g.cs", SourceText.From(cw.ToString(), Encoding.UTF8));
        }
    }

    private static INamedTypeSymbol? GetInterfaceWithInnerTypeArgument(
        ImmutableArray<INamedTypeSymbol> interfaces,
        ISymbol innerValueTypeSymbol,
        Type interfaceType)
    {
        var typeNameWithoutType = interfaceType.Name.Substring(0, interfaceType.Name.IndexOf('`'));
        return interfaces.FirstOrDefault(
            namedSymbol => namedSymbol.ContainingNamespace.Name == interfaceType.Namespace &&
                           namedSymbol.Name == typeNameWithoutType &&
                           namedSymbol.TypeArguments.Length == 1 &&
                           namedSymbol.TypeArguments[0].Name == innerValueTypeSymbol.Name
        );
    }

    private static bool HasAugment(ImmutableArray<INamedTypeSymbol> existingInterfaces, string augmentName)
    {
        return existingInterfaces.Any(x => x.Name == augmentName && x.ContainingNamespace.ToDisplayString() == AugmentedNamespace);
    }

    public static void AddPublicConstructor(CodeWriter cw, string valueObjectTypeName, bool hasDefaultValue)
    {
        if (hasDefaultValue)
        {
            cw.AppendLine($"public {valueObjectTypeName}()");
            using (cw.AddBlock())
            {
                cw.AppendLine("Value = DefaultValue.Value;");
            }
        }
        else
        {
            cw.AppendLine($"[global::System.Obsolete($\"Use {valueObjectTypeName}.{{nameof(From)}} instead.\", error: true)]");
            cw.AppendLine($"public {valueObjectTypeName}()");

            using (cw.AddBlock())
            {
                cw.AppendLine($"throw new global::System.InvalidOperationException($\"Use {valueObjectTypeName}.{{nameof(From)}} instead.\");");
            }
        }
    }

    public static void AddPrivateConstructor(CodeWriter cw, string valueObjectTypeName, string innerValueTypeName)
    {
        cw.AppendLine($"private {valueObjectTypeName}({innerValueTypeName} value)");
        using (cw.AddBlock())
        {
            cw.AppendLine("Value = value;");
        }
    }

    public static void OverrideBaseMethods(CodeWriter cw)
    {
        // hash code
        cw.AppendLine("public override int GetHashCode() => Value.GetHashCode();");
        cw.AppendLine();

        // ToString
        cw.AppendLine("public override string ToString() => Value.ToString();");
        cw.AppendLine();
    }

    public static void ImplementEqualsMethods(
        CodeWriter cw,
        string valueObjectTypeName,
        string innerValueTypeName,
        string innerValueTypeNullableAnnotation,
        bool hasDefaultEqualityComparer)
    {
        // IEquality<Self>
        cw.AppendLine($"public bool Equals({valueObjectTypeName} other) => Equals(other.Value);");

        // IEquality<Value>
        if (hasDefaultEqualityComparer)
        {
            cw.AppendLine($"public bool Equals({innerValueTypeName}{innerValueTypeNullableAnnotation} other) => InnerValueDefaultEqualityComparer.Equals(Value, other);");
        }
        else
        {
            cw.AppendLine($"public bool Equals({innerValueTypeName}{innerValueTypeNullableAnnotation} other) => Value.Equals(other);");
        }

        // with equality comparer
        cw.AppendLine($"public bool Equals({valueObjectTypeName} other, global::System.Collections.Generic.IEqualityComparer<{innerValueTypeName}> comparer) => comparer.Equals(Value, other.Value);");

        // object.Equals
        cw.AppendLine("public override bool Equals(object? obj)");
        using (cw.AddBlock())
        {
            cw.AppendLine("if (obj is null) return false;");
            cw.AppendLine($"if (obj is {valueObjectTypeName} value) return Equals(value);");
            cw.AppendLine($"if (obj is {innerValueTypeName} innerValue) return Equals(innerValue);");
            cw.AppendLine("return false;");
        }
    }

    public static void AddEqualityOperators(
        CodeWriter cw,
        string valueObjectTypeName,
        string innerValueTypeName)
    {
        cw.AppendLine($"public static bool operator ==({valueObjectTypeName} left, {valueObjectTypeName} right) => left.Equals(right);");
        cw.AppendLine($"public static bool operator !=({valueObjectTypeName} left, {valueObjectTypeName} right) => !left.Equals(right);");
        cw.AppendLine();

        cw.AppendLine($"public static bool operator ==({valueObjectTypeName} left, {innerValueTypeName} right) => left.Equals(right);");
        cw.AppendLine($"public static bool operator !=({valueObjectTypeName} left, {innerValueTypeName} right) => !left.Equals(right);");
        cw.AppendLine();

        cw.AppendLine($"public static bool operator ==({innerValueTypeName} left, {valueObjectTypeName} right) => right.Equals(left);");
        cw.AppendLine($"public static bool operator !=({innerValueTypeName} left, {valueObjectTypeName} right) => !right.Equals(left);");
        cw.AppendLine();
    }

    public static void AddExplicitCastOperators(
        CodeWriter cw,
        string valueObjectTypeName,
        string innerValueTypeName)
    {
        cw.AppendLine($"public static explicit operator {valueObjectTypeName}({innerValueTypeName} value) => From(value);");
        cw.AppendLine($"public static explicit operator {innerValueTypeName}({valueObjectTypeName} value) => value.Value;");
        cw.AppendLine();
    }

    private static void ForwardInterface(
        CodeWriter cw,
        string valueObjectTypeName,
        INamedTypeSymbol interfaceNamedTypeSymbol)
    {
        foreach (var memberSymbol in interfaceNamedTypeSymbol.GetMembers())
        {
            if (memberSymbol is not IMethodSymbol methodSymbol) continue;
            var originalSymbol = methodSymbol.OriginalDefinition;

            cw.Append("public ");
            cw.Append(originalSymbol.ReturnsVoid ? "void " : $"{TypeSymbolToString(originalSymbol.ReturnType)} ");
            cw.Append(originalSymbol.Name);

            cw.Append("(");

            var first = true;
            foreach (var parameterSymbol in originalSymbol.Parameters)
            {
                if (!first) cw.Append(", ");
                first = false;

                cw.Append($"{TypeSymbolToString(parameterSymbol.Type)} ");
                cw.Append(parameterSymbol.Name);
            }

            cw.Append($") => Value.{originalSymbol.Name}(");

            first = true;
            foreach (var parameterSymbol in originalSymbol.Parameters)
            {
                if (!first) cw.Append(", ");
                first = false;

                cw.Append(parameterSymbol.Name);
            }

            cw.Append(");");
            cw.AppendLine();
        }

        return;
        string TypeSymbolToString(ITypeSymbol typeSymbol)
        {
            return typeSymbol.TypeKind == TypeKind.TypeParameter
                ? valueObjectTypeName
                : typeSymbol.ToDisplayString(CustomSymbolDisplayFormats.GlobalFormat);
        }
    }

    public static void AddGuidSpecificCode(
        CodeWriter cw,
        string valueObjectTypeName,
        string innerValueTypeName)
    {
        cw.AppendLine($"public static {valueObjectTypeName} NewId() => From({innerValueTypeName}.NewGuid());");
        cw.AppendLine();
    }

    private readonly struct Target : IEquatable<Target>
    {
        public readonly StructDeclarationSyntax Syntax;
        public readonly AttributeData AttributeData;

        public Target(StructDeclarationSyntax syntax, AttributeData attributeData)
        {
            Syntax = syntax;
            AttributeData = attributeData;
        }

        public bool Equals(Target other)
        {
            return Syntax.Equals(other.Syntax) && AttributeData.Equals(other.AttributeData);
        }

        public override bool Equals(object? obj)
        {
            return obj is Target other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Syntax.GetHashCode() * 397) ^ AttributeData.GetHashCode();
            }
        }
    }
}
