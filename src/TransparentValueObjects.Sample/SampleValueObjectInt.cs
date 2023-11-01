using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<int>]
public readonly partial struct SampleValueObjectInt :
    IHasDefaultValue<SampleValueObjectInt, int>,
    IHasDefaultEqualityComparer<SampleValueObjectInt, int>,
    IHasRandomId<SampleValueObjectInt, int, Random>
{
    public static SampleValueObjectInt DefaultValue => From(0);
    public static IEqualityComparer<int> InnerValueDefaultEqualityComparer => EqualityComparer<int>.Default;
}
