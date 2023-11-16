
#region Base Methods

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public override int GetHashCode() => InnerValueDefaultEqualityComparer.GetHashCode(Value);

public int GetHashCode(global::System.Collections.Generic.IEqualityComparer<global::System.String> equalityComparer) => equalityComparer.GetHashCode(Value);

/// <inheritdoc/>
[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public override string ToString() => Value.ToString();

#endregion Base Methods

