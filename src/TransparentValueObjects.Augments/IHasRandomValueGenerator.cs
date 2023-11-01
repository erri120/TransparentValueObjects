namespace TransparentValueObjects.Augments;

public interface IHasRandomValueGenerator<out TValueObject, out TValue, in TRandom>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
    where TRandom : Random
{
    public static abstract Func<TRandom?, TValueObject> GenerateRandomValue { get; }

    public static abstract TValueObject NewRandomValue(TRandom? random);
}
