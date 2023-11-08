namespace TransparentValueObjects.Augments;

public interface IHasSystemTextJsonConverter
{
    public static abstract Type JsonConverterType { get; }
}
