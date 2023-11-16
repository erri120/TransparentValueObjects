
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

