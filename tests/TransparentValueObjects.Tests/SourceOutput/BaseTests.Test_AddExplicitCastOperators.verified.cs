
#region Explicit Cast Operators

/// <summary>Explicit Casting Operator</summary>
public static explicit operator TestValueObject(global::System.String value) => From(value);
/// <summary>Explicit Casting Operator</summary>
public static explicit operator global::System.String(TestValueObject value) => value.Value;

#endregion Explicit Cast Operators

