using JetBrains.Annotations;

namespace TransparentValueObjects;

/// <summary>
/// Augment to enable support for a default value.
/// </summary>
[PublicAPI]
public sealed class DefaultValueAugment : IAugment;

/// <summary>
/// Represents the interface for <see cref="DefaultValueAugment"/>
/// with members that the consumer has to implement to support the augment.
/// </summary>
[PublicAPI]
public interface IDefaultValue<out TValueObject, TInnerValue>
    where TValueObject : IValueObject<TInnerValue>
    where TInnerValue : notnull
{
    /// <summary>
    /// Gets the default value for this value object.
    /// </summary>
    public static abstract TValueObject DefaultValue { get; }
}
