﻿// <auto-generated/>
#nullable enable

namespace TestNamespace;

[global::System.Diagnostics.DebuggerDisplay("{Value}")]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Auto-generated.")]

#region Augment Attributes

#endregion Augment Attributes

readonly partial struct TestValueObject :

#region Base Interfaces

	global::TransparentValueObjects.IValueObject<global::System.String>,
	global::System.IEquatable<TestValueObject>,
	global::System.IEquatable<global::System.String>

#endregion Base Interfaces

#region Forwarded Interfaces

	,global::System.IComparable<TestValueObject>
	,global::System.IComparable<global::System.String>

#endregion Forwarded Interfaces

#region Augment Interfaces

#endregion Augment Interfaces

{
	/// <summary>
	/// The underlying data of the value object.
	/// </summary>
	public readonly global::System.String Value;

	/// <inheritdoc/>
	public static global::System.Type InnerValueType => typeof(global::System.String);

	/// <summary>
	/// Creates a new instance of the value object using the provided inner value.
	/// </summary>
	/// <param name="value">The value that the value object should wrap.</param>
	/// <returns>A new instance of this value object.</returns>
	public static TestValueObject From(global::System.String value) => new(value);

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

#region Base Methods

	/// <inheritdoc/>
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => Value.GetHashCode();

	/// <inheritdoc/>
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public override string ToString() => Value.ToString();

#endregion Base Methods

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

#region Equality Operators

	/// <summary>Equality Operator</summary>
	public static bool operator ==(TestValueObject left, TestValueObject right) => left.Equals(right);
	/// <summary>Equality Operator</summary>
	public static bool operator !=(TestValueObject left, TestValueObject right) => !left.Equals(right);

	/// <summary>Equality Operator</summary>
	public static bool operator ==(TestValueObject left, global::System.String right) => left.Equals(right);
	/// <summary>Equality Operator</summary>
	public static bool operator !=(TestValueObject left, global::System.String right) => !left.Equals(right);

	/// <summary>Equality Operator</summary>
	public static bool operator ==(global::System.String left, TestValueObject right) => right.Equals(left);
	/// <summary>Equality Operator</summary>
	public static bool operator !=(global::System.String left, TestValueObject right) => !right.Equals(left);

#endregion Equality Operators

#region Explicit Cast Operators

	/// <summary>Explicit Casting Operator</summary>
	public static explicit operator TestValueObject(global::System.String value) => From(value);
	/// <summary>Explicit Casting Operator</summary>
	public static explicit operator global::System.String(TestValueObject value) => value.Value;

#endregion Explicit Cast Operators

#region IComparable Implementation

	/// <inheritdoc/>
	public global::System.Int32 CompareTo(TestValueObject other) => Value.CompareTo(other.Value);
	/// <inheritdoc/>
	public global::System.Int32 CompareTo(global::System.String? other) => Value.CompareTo(other);

#endregion IComparable Implementation

#region Comparison Operators

	/// <summary>Comparison Operator</summary>
	public static bool operator <(TestValueObject left, TestValueObject right) => left.Value.CompareTo(right.Value) < 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator >(TestValueObject left, TestValueObject right) => left.Value.CompareTo(right.Value) > 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator <=(TestValueObject left, TestValueObject right) => left.Value.CompareTo(right.Value) <= 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator >=(TestValueObject left, TestValueObject right) => left.Value.CompareTo(right.Value) >= 0;

	/// <summary>Comparison Operator</summary>
	public static bool operator <(global::System.String left, TestValueObject right) => left.CompareTo(right.Value) < 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator >(global::System.String left, TestValueObject right) => left.CompareTo(right.Value) > 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator <=(global::System.String left, TestValueObject right) => left.CompareTo(right.Value) <= 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator >=(global::System.String left, TestValueObject right) => left.CompareTo(right.Value) >= 0;

	/// <summary>Comparison Operator</summary>
	public static bool operator <(TestValueObject left, global::System.String right) => left.Value.CompareTo(right) < 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator >(TestValueObject left, global::System.String right) => left.Value.CompareTo(right) > 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator <=(TestValueObject left, global::System.String right) => left.Value.CompareTo(right) <= 0;
	/// <summary>Comparison Operator</summary>
	public static bool operator >=(TestValueObject left, global::System.String right) => left.Value.CompareTo(right) >= 0;

#endregion Comparison Operators

}

