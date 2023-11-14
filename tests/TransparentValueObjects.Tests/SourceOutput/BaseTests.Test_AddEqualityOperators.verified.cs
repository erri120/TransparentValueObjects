
#region Equality Operators

public static bool operator ==(TestValueObject left, TestValueObject right) => left.Equals(right);
public static bool operator !=(TestValueObject left, TestValueObject right) => !left.Equals(right);

public static bool operator ==(TestValueObject left, global::System.String right) => left.Equals(right);
public static bool operator !=(TestValueObject left, global::System.String right) => !left.Equals(right);

public static bool operator ==(global::System.String left, TestValueObject right) => right.Equals(left);
public static bool operator !=(global::System.String left, TestValueObject right) => !right.Equals(left);

#endregion Equality Operators

