using System.Collections.Generic;
using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<int>]
public readonly partial struct SampleValueObjectInt :
    IHasDefaultValue<SampleValueObjectInt, int>,
    IHasDefaultEqualityComparer<SampleValueObjectInt, int>,
    IHasSystemTextJsonConverter,
    IHasEfCore<SampleValueObjectInt, int>
{
    public static SampleValueObjectInt DefaultValue => From(0);
    public static IEqualityComparer<int> InnerValueDefaultEqualityComparer => EqualityComparer<int>.Default;
}
