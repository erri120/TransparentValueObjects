using System;
using System.Collections.Generic;
using TransparentValueObjects.Augments;
using TransparentValueObjects.Generated;

namespace TransparentValueObjects.Sample;

[ValueObject<string>]
public readonly partial struct SampleValueObject :
    IHasDefaultValue<SampleValueObject, string>,
    IHasDefaultEqualityComparer<SampleValueObject, string>
{
    public static SampleValueObject DefaultValue => From("Hello World!");
    public static IEqualityComparer<SampleValueObject> DefaultEqualityComparer => EqualityComparer<SampleValueObject>.Default;
    public static IEqualityComparer<string> InnerValueDefaultEqualityComparer => StringComparer.OrdinalIgnoreCase;
}
