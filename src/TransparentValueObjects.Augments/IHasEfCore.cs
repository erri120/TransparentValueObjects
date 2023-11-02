namespace TransparentValueObjects.Augments;

public interface IHasEfCore<out TValueObject, out TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{

}
