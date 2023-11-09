using System.Collections.Generic;
using JetBrains.Annotations;

namespace TransparentValueObjects.Augments;

/// <summary>
/// Augment for specifying a default equality comparer.
/// </summary>
/// <remarks>
/// Use this augment to provide an equality comparer that is not <see cref="EqualityComparer{T}.Default"/>.
/// </remarks>
[PublicAPI]
public interface IHasDefaultEqualityComparer<TValueObject, in TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{
    /// <summary>
    /// Gets the default equality comparer.
    /// </summary>
    public static abstract IEqualityComparer<TValue> InnerValueDefaultEqualityComparer { get; }
}
