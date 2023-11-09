using System;

namespace TransparentValueObjects.Augments;

/// <summary>
/// Augment to extend support for random value generation via <see cref="Random"/>.
/// Provides a high performance implementation for <see langword="unmanaged"/> structs.
/// </summary>
/// <typeparam name="TValueObject"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TRandom"></typeparam>
public interface IHasUnmanagedRandomValueGenerator<out TValueObject, out TValue, out TRandom> : IHasRandomValueGenerator<TValueObject, TValue, TRandom>
    where TValueObject : IValueObject<TValue>
    where TValue : unmanaged
    where TRandom : Random
{

}
