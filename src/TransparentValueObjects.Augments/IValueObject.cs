using System;
using JetBrains.Annotations;

namespace TransparentValueObjects.Augments;

/// <summary>
/// Represents a value object.
/// </summary>
/// <remarks>
/// This is the non-generic interface.
/// </remarks>
/// <seealso cref="IValueObject{TValue}"/>
[PublicAPI]
public interface IValueObject
{
    /// <summary>
    /// Gets the type of the inner value.
    /// </summary>
    public static abstract Type InnerValueType { get; }
}

/// <summary>
/// Represents a value object.
/// </summary>
/// <remarks>
/// This is the generic interface.
/// </remarks>
/// <seealso cref="IValueObject"/>
/// <typeparam name="TValue"></typeparam>
[PublicAPI]
public interface IValueObject<TValue> : IValueObject
    where TValue : notnull { }
