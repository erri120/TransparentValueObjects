
#region Constructors

private TestValueObject(global::System.String value)
{
	Value = value;
}

/// <summary>
/// Default constructor using <see cref="DefaultValue"/> as the underlying value.
/// </summary>
public TestValueObject()
{
	Value = DefaultValue.Value;
}

#endregion Constructors

