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
    IHasRandomValueGenerator<SampleValueObjectGuid, Guid, Random>
{
    public static SampleValueObjectGuid DefaultValue => From(Guid.Empty);
    public static IEqualityComparer<Guid> InnerValueDefaultEqualityComparer => EqualityComparer<Guid>.Default;
    public static Random GetRandom() => Random.Shared;
    public static SampleValueObjectGuid NewRandomValue() => From(Guid.NewGuid());
}
