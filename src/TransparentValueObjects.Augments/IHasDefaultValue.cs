namespace TransparentValueObjects.Augments;

public interface IHasDefaultValue<out TSelf>
{
    static abstract TSelf GetDefaultValue();
}
