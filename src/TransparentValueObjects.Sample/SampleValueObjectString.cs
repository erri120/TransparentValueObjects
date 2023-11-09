using System;
using System.Collections.Generic;
using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<string>]
public readonly partial struct SampleValueObjectString :
    IHasDefaultValue<SampleValueObjectString, string>,
    IHasDefaultEqualityComparer<SampleValueObjectString, string>,
    IHasSystemTextJsonConverter,
    IHasEfCore<SampleValueObjectString, string>
{
    public static SampleValueObjectString DefaultValue => From("Hello World!");
    public static IEqualityComparer<string> InnerValueDefaultEqualityComparer => StringComparer.OrdinalIgnoreCase;
}
