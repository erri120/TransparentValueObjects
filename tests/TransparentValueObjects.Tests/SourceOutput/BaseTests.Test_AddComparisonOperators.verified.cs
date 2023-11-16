
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

