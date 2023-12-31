﻿
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
	public override global::System.Boolean Equals(TestValueObject left, TestValueObject right) => left.Equals(right);

	/// <inheritdoc/>
	public override TestValueObject Snapshot(TestValueObject instance) => From(instance.Value);

	/// <inheritdoc/>
	public override global::System.Int32 GetHashCode(TestValueObject instance) => instance.GetHashCode();

}

#endregion EF Core Augment

