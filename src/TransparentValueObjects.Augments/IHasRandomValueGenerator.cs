using System;

namespace TransparentValueObjects.Augments;

public interface IHasRandomValueGenerator<out TValueObject, out TValue, out TRandom>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
    where TRandom : Random
{
    public static abstract TRandom GetRandom();

    public static abstract TValueObject NewRandomValue();
}
