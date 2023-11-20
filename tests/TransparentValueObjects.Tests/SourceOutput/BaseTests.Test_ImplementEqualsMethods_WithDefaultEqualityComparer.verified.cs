
#region Equals Methods

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public global::System.Boolean Equals(TestValueObject other) => Equals(other.Value);

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public global::System.Boolean Equals(global::System.String? other) => InnerValueDefaultEqualityComparer.Equals(Value, other);

/// <summary>
/// Determines whether the specified value is equal to the current inner value using a custom equality comparer.
/// </summary>
/// <param name="other">The value to compare with the current inner value.</param>
/// <param name="comparer">The comparer to use.</param>
/// <returns></returns>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public global::System.Boolean Equals(global::System.String other, global::System.Collections.Generic.IEqualityComparer<global::System.String> comparer) => comparer.Equals(Value, other);

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public override global::System.Boolean Equals(global::System.Object? obj)
{
	if (obj is null) return false;
	if (obj is TestValueObject value) return Equals(value);
	if (obj is global::System.String innerValue) return Equals(innerValue);
	return false;
}

#endregion Equals Methods

