namespace TransparentValueObjects.Augments;

public interface IHasUnmanagedRandomValueGenerator<out TValueObject, out TValue, out TRandom> : IHasRandomValueGenerator<TValueObject, TValue, TRandom>
    where TValueObject : IValueObject<TValue>
    where TValue : unmanaged
    where TRandom : Random
{

}
