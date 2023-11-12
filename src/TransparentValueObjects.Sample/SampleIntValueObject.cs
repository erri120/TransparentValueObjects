namespace TransparentValueObjects.Sample;

[ValueObject<int>]
public readonly partial struct SampleIntValueObject : IAugmentWith<
    DefaultValueAugment,
    JsonAugment,
    EfCoreAugment>
{
    /// <inheritdoc/>
    public static SampleIntValueObject DefaultValue => From(0);
}
