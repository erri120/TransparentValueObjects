namespace TransparentValueObjects;

public static partial class Augments
{
    public static readonly Augment DefaultValueAugment = new(nameof(DefaultValueAugment));
    public static readonly Augment DefaultEqualityComparerAugment = new(nameof(DefaultEqualityComparerAugment));
    public static readonly Augment JsonAugment = new(nameof(JsonAugment));
    public static readonly Augment EfCoreAugment = new(nameof(EfCoreAugment));
}

public class Augment
{
    public readonly string Name;
    public readonly string GlobalName;

    public Augment(string name)
    {
        Name = name;
        GlobalName = $"global::{Constants.Namespace}.{Name}";
    }
}

