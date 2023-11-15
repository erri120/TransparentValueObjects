
#region IComparable Implementation

/// <inheritdoc/>
public global::System.Int32 CompareTo(TestValueObject other) => Value.CompareTo(other.Value);
/// <inheritdoc/>
public global::System.Int32 CompareTo(global::System.Guid other) => Value.CompareTo(other);

#endregion IComparable Implementation

