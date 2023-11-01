namespace TransparentValueObjects.Augments;

public interface IHasDefaultEqualityComparer<in TValueObject, in TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{
    public static abstract IEqualityComparer<TValueObject> DefaultEqualityComparer { get; }
    public static abstract IEqualityComparer<TValue> InnerValueDefaultEqualityComparer { get; }
}
