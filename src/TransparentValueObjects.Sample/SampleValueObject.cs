using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<string>]
public readonly partial struct SampleValueObject : IHasDefaultValue<SampleValueObject, string>
{
    public static SampleValueObject DefaultValue => From("Hello World!");
}
