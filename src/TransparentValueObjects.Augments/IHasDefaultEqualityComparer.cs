namespace TransparentValueObjects.Augments;

public interface IHasDefaultEqualityComparer<TValueObject, in TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{
    public static abstract IEqualityComparer<TValue> InnerValueDefaultEqualityComparer { get; }
}
