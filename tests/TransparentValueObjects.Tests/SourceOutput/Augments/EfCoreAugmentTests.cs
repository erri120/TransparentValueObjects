using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public class EfCoreAugmentTests
{
    [Fact]
    public void Test_AddEfCoreConverterComparer()
    {
        const string expected = /*lang=csharp*/
"""
#region EF Core Augment

/// <summary>
/// <see cref="global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter{TModel,TProvider}"/> between <see cref="TestValueObject"/> and <see cref="global::System.String"/>.
/// </summary>
public class EfCoreValueConverter : global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<TestValueObject, global::System.String>
{
	public EfCoreValueConverter() : this(mappingHints: null) { }

	public EfCoreValueConverter(global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ConverterMappingHints? mappingHints = null) : base(
		static value => value.Value,
		static innerValue => From(innerValue),
		mappingHints
	) { }
}

/// <summary>
/// <see cref="global::Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer{T}"/> for <see cref="TestValueObject"/>.
/// </summary>
public class EfCoreValueComparer : global::Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<TestValueObject>
{
	public EfCoreValueComparer() : base(
		static (left, right) => left.Equals(right),
		static value => value.GetHashCode(),
		static value => From(value.Value)
	) { }

	/// <inheritdoc/>
	public override bool Equals(TestValueObject left, TestValueObject right) => left.Equals(right);

	/// <inheritdoc/>
	public override TestValueObject Snapshot(TestValueObject instance) => From(instance.Value);

	/// <inheritdoc/>
	public override int GetHashCode(TestValueObject instance) => instance.GetHashCode();

}

#endregion EF Core Augment
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddEfCoreConverterComparer(cw, "TestValueObject", "global::System.String");
        cw.ToString().SourceNormalize().Should().Be(expected.SourceNormalize());
    }
}
