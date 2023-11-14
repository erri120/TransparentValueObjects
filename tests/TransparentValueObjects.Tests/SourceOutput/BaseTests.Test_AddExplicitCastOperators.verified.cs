
#region Explicit Cast Operators

public static explicit operator TestValueObject(global::System.String value) => From(value);
public static explicit operator global::System.String(TestValueObject value) => value.Value;

#endregion Explicit Cast Operators

