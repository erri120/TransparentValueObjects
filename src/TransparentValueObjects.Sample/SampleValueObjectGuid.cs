using System;
using System.Collections.Generic;
using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<Guid>]
public readonly partial struct SampleValueObjectGuid :
    IHasDefaultValue<SampleValueObjectGuid, Guid>,
    IHasDefaultEqualityComparer<SampleValueObjectGuid, Guid>,
    IHasRandomValueGenerator<SampleValueObjectGuid, Guid, Random>
{
    public static SampleValueObjectGuid DefaultValue => From(Guid.Empty);
    public static IEqualityComparer<Guid> InnerValueDefaultEqualityComparer => EqualityComparer<Guid>.Default;
    public static Func<Random?, SampleValueObjectGuid> GenerateRandomValue => _ => From(Guid.NewGuid());
}
