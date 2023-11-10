using System;
using System.Collections.Generic;
using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<Guid>]
public readonly partial struct SampleValueObjectGuid :
    IHasDefaultValue<SampleValueObjectGuid, Guid>,
    IHasDefaultEqualityComparer<SampleValueObjectGuid, Guid>,
    IHasSystemTextJsonConverter,
    IHasEfCore<SampleValueObjectGuid, Guid>
{
    public static SampleValueObjectGuid DefaultValue => From(Guid.Empty);
    public static IEqualityComparer<Guid> InnerValueDefaultEqualityComparer => EqualityComparer<Guid>.Default;
}
