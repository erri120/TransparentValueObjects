namespace TransparentValueObjects.PostInitializationOutput;

public static partial class Augments
{
    public static class DefaultValueAugment
    {
        public const string Name = nameof(DefaultValueAugment);
        public const string GlobalName = $"global::{Constants.Namespace}.{Name}";
        public const string HintName = $"{Name}.g.cs";

        public const string SourceCode = /*lang=csharp*/
$$"""
{{Constants.AutoGeneratedHeader}}

namespace {{Constants.Namespace}}
{
    /// <summary>
    /// Augment to enable support for a default value.
    /// </summary>
    {{Constants.CodeCoverageAttribute}}
    internal sealed class {{Name}} : {{IAugment.GlobalName}} { }
}
""";
    }
}
