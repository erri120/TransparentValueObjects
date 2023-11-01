namespace TransparentValueObjects.Augments;

public interface IValueObject<TValue>
    where TValue : notnull
{
    public static Type InnerValueType => typeof(TValue);
}

