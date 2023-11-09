using System;

namespace TransparentValueObjects.Augments;

/// <summary>
/// Augment to enable support for random value generation via <see cref="Random"/>.
/// </summary>
/// <typeparam name="TValueObject"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TRandom"></typeparam>
public interface IHasRandomValueGenerator<out TValueObject, out TValue, out TRandom>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
    where TRandom : Random
{
    /// <summary>
    /// Gets the random source. <see cref="Random.Shared"/> can be used for better performance.
    /// </summary>
    /// <returns></returns>
    public static abstract TRandom GetRandom();

    /// <summary>
    /// Gets the random object.
    /// </summary>
    /// <returns></returns>
    public static abstract TValueObject NewRandomValue();
}
