using System.Diagnostics.CodeAnalysis;

namespace TransparentValueObjects.PostInitializationOutput;

public static partial class AugmentInterfaces
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IDefaultValue
    {
        public const string Name = nameof(IDefaultValue);
        public const string GlobalName = $"global::{Constants.Namespace}.{Name}";
        public const string HintName = $"{Name}.g.cs";

        public const string SourceCode = /*lang=csharp*/
$$"""
{{Constants.AutoGeneratedHeader}}

namespace {{Constants.Namespace}}
{
    /// <summary>
    /// Represents the interface for <see cref="{{Augments.DefaultValueAugment.GlobalName}}"/>
    /// with members that the consumer has to implement to support the augment.
    /// </summary>
    internal interface {{Name}}<out TValueObject, TInnerValue>
        where TValueObject : {{IValueObject.GlobalName}}<TInnerValue>
        where TInnerValue : notnull
    {
        /// <summary>
        /// Gets the default value for this value object.
        /// </summary>
        public static abstract TValueObject DefaultValue { get; }
    }
}
""";
    }
}
