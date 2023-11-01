using System.Diagnostics.CodeAnalysis;

namespace TransparentValueObjects.Augments;

public interface IHasRandomValue<out TValueObject, TValue, [SuppressMessage("ReSharper", "UnusedTypeParameter")] TRandom>
    where TValueObject : IValueObject<TValue>
    where TValue : unmanaged
    where TRandom : Random
{
    public static abstract TValueObject NewRandomValue();
}
