
#region Base Methods

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public override global::System.Int32 GetHashCode() => InnerValueDefaultEqualityComparer.GetHashCode(Value);

public global::System.Int32 GetHashCode(global::System.Collections.Generic.IEqualityComparer<global::System.String> equalityComparer) => equalityComparer.GetHashCode(Value);

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public override global::System.String ToString() => Value.ToString();

#endregion Base Methods

