using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<string>]
public readonly partial struct SampleValueObject
{
    public static SampleValueObject Thing = new("asd");
}
