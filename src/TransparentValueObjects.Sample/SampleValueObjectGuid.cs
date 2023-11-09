using System;
using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<Guid>]
public readonly partial struct SampleValueObjectGuid :
    IHasDefaultValue<SampleValueObjectGuid, Guid>,
    IHasSystemTextJsonConverter
{
    public static SampleValueObjectGuid DefaultValue => From(Guid.Empty);
}
