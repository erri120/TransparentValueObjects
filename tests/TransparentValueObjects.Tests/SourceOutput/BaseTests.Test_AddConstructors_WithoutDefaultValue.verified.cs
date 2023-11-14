
#region Constructors

private TestValueObject(global::System.String value)
{
	Value = value;
}

/// <summary>
/// The default constructor is disabled and will throw an exception if called.
/// </summary>
/// <remarks>
/// The default constructor can be enabled by providing a default value
/// using the <see cref="global::TransparentValueObjects.DefaultValueAugment"/> augment.
/// </remarks>
/// <exception cref="global::System.InvalidOperationException">Thrown when called.</exception>
[global::System.Obsolete($"Use TestValueObject.{nameof(From)} instead.", error: true)]
public TestValueObject()
{
	throw new global::System.InvalidOperationException($"Use TestValueObject.{nameof(From)} instead.");
}

#endregion Constructors

