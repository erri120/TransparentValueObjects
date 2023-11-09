using JetBrains.Annotations;

namespace TransparentValueObjects.Augments;

/// <summary>
/// Augment to provide a default value for the value object.
/// </summary>
/// <remarks>
/// This augment allows you to use the default constructor.
/// </remarks>
[PublicAPI]
public interface IHasDefaultValue<out TValueObject, TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{
    /// <summary>
    /// Gets the default value for the value object.
    /// </summary>
    public static abstract TValueObject DefaultValue { get; }
}
