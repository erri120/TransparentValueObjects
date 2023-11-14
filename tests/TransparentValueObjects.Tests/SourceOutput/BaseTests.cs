using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public class BaseTests
{
    [Fact]
    public void Test_AddConstructors_WithoutDefaultValue()
    {
        const string expected = /*lang=csharp*/
"""
#region Constructors

private TestValueObject(global::System.String value)
{
	Value = value;
}

/// <summary>
/// The default constructor is disabled and will throw an exception if called.
/// </summary>
/// <remarks>
/// The default constructor can be enabled by providing a default value
/// using the <see cref="global::TransparentValueObjects.DefaultValueAugment"/> augment.
/// </remarks>
/// <exception cref="global::System.InvalidOperationException">Thrown when called.</exception>
[global::System.Obsolete($"Use TestValueObject.{nameof(From)} instead.", error: true)]
public TestValueObject()
{
	throw new global::System.InvalidOperationException($"Use TestValueObject.{nameof(From)} instead.");
}

#endregion Constructors
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddConstructors(cw, "TestValueObject", "global::System.String", hasDefaultValue: false);
        cw.ToString().SourceNormalizeEquals(expected);
    }

    [Fact]
    public void Test_AddConstructors_WithDefaultValue()
    {
        const string expected = /*lang=csharp*/
"""
#region Constructors

private TestValueObject(global::System.String value)
{
	Value = value;
}

/// <summary>
/// Default constructor using <see cref="DefaultValue"/> as the underlying value.
/// </summary>
public TestValueObject()
{
	Value = DefaultValue.Value;
}

#endregion Constructors
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddConstructors(cw, "TestValueObject", "global::System.String", hasDefaultValue: true);
        cw.ToString().SourceNormalizeEquals(expected);
    }

    [Fact]
    public void Test_OverrideBaseMethods()
    {
        const string expected = /*lang=csharp*/
"""
#region Base Methods

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public override int GetHashCode() => Value.GetHashCode();

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public override string ToString() => Value.ToString();

#endregion Base Methods
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.OverrideBaseMethods(cw);
        cw.ToString().SourceNormalizeEquals(expected);
    }

    [Fact]
    public void Test_ImplementEqualsMethods_WithoutDefaultEqualityComparer()
    {
        const string expected = /*lang=csharp*/
"""
#region Equals Methods

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public bool Equals(TestValueObject other) => Equals(other.Value);

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public bool Equals(global::System.String? other) => Value.Equals(other);

/// <summary>
/// Determines whether the specified value is equal to the current inner value using a custom equality comparer.
/// </summary>
/// <param name="other">The value to compare with the current inner value.</param>
/// <param name="comparer">The comparer to use.</param>
/// <returns></returns>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public bool Equals(global::System.String other, global::System.Collections.Generic.IEqualityComparer<global::System.String> comparer) => comparer.Equals(Value, other);

/// <inheritdoc/>
public override bool Equals(object? obj)
{
	if (obj is null) return false;
	if (obj is TestValueObject value) return Equals(value);
	if (obj is global::System.String innerValue) return Equals(innerValue);
	return false;
}

#endregion Equals Methods
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementEqualsMethods(cw, "TestValueObject", "global::System.String", "?", hasDefaultEqualityComparer: false);
        cw.ToString().SourceNormalizeEquals(expected);
    }

    [Fact]
    public void Test_ImplementEqualsMethods_WithDefaultEqualityComparer()
    {
        const string expected = /*lang=csharp*/
"""
#region Equals Methods

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public bool Equals(TestValueObject other) => Equals(other.Value);

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public bool Equals(global::System.String? other) => InnerValueDefaultEqualityComparer.Equals(Value, other);

/// <summary>
/// Determines whether the specified value is equal to the current inner value using a custom equality comparer.
/// </summary>
/// <param name="other">The value to compare with the current inner value.</param>
/// <param name="comparer">The comparer to use.</param>
/// <returns></returns>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public bool Equals(global::System.String other, global::System.Collections.Generic.IEqualityComparer<global::System.String> comparer) => comparer.Equals(Value, other);

/// <inheritdoc/>
public override bool Equals(object? obj)
{
	if (obj is null) return false;
	if (obj is TestValueObject value) return Equals(value);
	if (obj is global::System.String innerValue) return Equals(innerValue);
	return false;
}

#endregion Equals Methods
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementEqualsMethods(cw, "TestValueObject", "global::System.String", "?", hasDefaultEqualityComparer: true);
        cw.ToString().SourceNormalizeEquals(expected);
    }

    [Fact]
    public void Test_AddEqualityOperators()
    {
        const string expected = /*lang=csharp*/
"""
#region Equality Operators

public static bool operator ==(TestValueObject left, TestValueObject right) => left.Equals(right);
public static bool operator !=(TestValueObject left, TestValueObject right) => !left.Equals(right);

public static bool operator ==(TestValueObject left, global::System.String right) => left.Equals(right);
public static bool operator !=(TestValueObject left, global::System.String right) => !left.Equals(right);

public static bool operator ==(global::System.String left, TestValueObject right) => right.Equals(left);
public static bool operator !=(global::System.String left, TestValueObject right) => !right.Equals(left);

#endregion Equality Operators
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddEqualityOperators(cw, "TestValueObject", "global::System.String");
        cw.ToString().SourceNormalizeEquals(expected);
    }

    [Fact]
    public void Test_AddExplicitCastOperators()
    {
        const string expected = /*lang=csharp*/
"""
#region Explicit Cast Operators

public static explicit operator TestValueObject(global::System.String value) => From(value);
public static explicit operator global::System.String(TestValueObject value) => value.Value;

#endregion Explicit Cast Operators
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddExplicitCastOperators(cw, "TestValueObject", "global::System.String");
        cw.ToString().SourceNormalizeEquals(expected);
    }
}
