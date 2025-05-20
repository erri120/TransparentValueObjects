using System;
using JetBrains.Annotations;

namespace TransparentValueObjects;

/// <summary>
/// Represents a value object.
/// </summary>
/// <seealso cref="IValueObject{TInnerType}"/>
[PublicAPI]
public interface IValueObject
{
    /// <summary>
    /// Gets the inner value type.
    /// </summary>
    static abstract Type InnerValueType { get; }
}

/// <summary>
/// Represents a value object.
/// </summary>
/// <seealso cref="IValueObject"/>
[PublicAPI]
public interface IValueObject<TInnerType> : IValueObject
    where TInnerType : notnull
{
    /// <inheritdoc/>
    static Type IValueObject.InnerValueType => typeof(TInnerType);
}
