using System;
using System.Collections.Generic;

namespace TransparentValueObjects.Sample;

[ValueObject<string>]
public readonly partial struct SampleStringValueObject : IAugmentWith<
    DefaultValueAugment,
    DefaultEqualityComparerAugment,
    JsonAugment,
    EfCoreAugment>
{
    /// <inheritdoc/>
    public static SampleStringValueObject DefaultValue => From(string.Empty);

    /// <inheritdoc/>
    public static IEqualityComparer<string> InnerValueDefaultEqualityComparer => StringComparer.OrdinalIgnoreCase;
}
