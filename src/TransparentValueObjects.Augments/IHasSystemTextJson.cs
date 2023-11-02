namespace TransparentValueObjects.Augments;

public interface IHasSystemTextJson<out TValueObject, out TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{

}
