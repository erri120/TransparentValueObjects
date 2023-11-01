using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<string>]
public readonly partial struct SampleValueObject : IHasDefaultValue<SampleValueObject>
{
    public static SampleValueObject GetDefaultValue() => From("Hello World!");
}
