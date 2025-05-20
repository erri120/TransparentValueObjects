using System.Collections.Generic;
using JetBrains.Annotations;

namespace TransparentValueObjects;

/// <summary>
/// Augment to enable support for a default equality comparer.
/// </summary>
[PublicAPI]
public sealed class DefaultEqualityComparerAugment : IAugment;

/// <summary>
/// Represents the interface for <see cref="DefaultEqualityComparerAugment"/>
/// with members that the consumer has to implement to support the augment.
/// </summary>
[PublicAPI]
public interface IDefaultEqualityComparer<out TValueObject, in TInnerValue>
    where TValueObject : IValueObject<TInnerValue>
    where TInnerValue : notnull
{
    /// <summary>
    /// Gets an equality comparer for the inner value type to be used by default.
    /// </summary>
    public static abstract IEqualityComparer<TInnerValue> InnerValueDefaultEqualityComparer { get; }
}
