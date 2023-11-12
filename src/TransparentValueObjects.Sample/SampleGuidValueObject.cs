using System;

namespace TransparentValueObjects.Sample;

[ValueObject<Guid>]
public readonly partial struct SampleGuidValueObject : IAugmentWith<
    DefaultValueAugment,
    JsonAugment,
    EfCoreAugment>
{
    /// <inheritdoc/>
    public static SampleGuidValueObject DefaultValue => From(Guid.Empty);
}
