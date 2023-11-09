using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<int>]
public readonly partial struct SampleValueObjectInt :
    IHasDefaultValue<SampleValueObjectInt, int>,
    IHasSystemTextJsonConverter
{
    public static SampleValueObjectInt DefaultValue => From(0);
}
