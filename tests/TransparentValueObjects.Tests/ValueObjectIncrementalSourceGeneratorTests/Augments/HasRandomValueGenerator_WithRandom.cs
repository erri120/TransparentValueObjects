using Xunit;

namespace TransparentValueObjects.Tests.ValueObjectIncrementalSourceGeneratorTests.Augments;

public class HasRandomValueGenerator_WithRandom
{
    private const string Input =
"""
using TransparentValueObjects.Generated;
using TransparentValueObjects.Augments;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct StringValueObject : IHasRandomValueGenerator<SampleValueObjectGuid, Guid, Random>
{
    public static StringValueObject DefaultValue => From("Hello World!");

    public static Random GetRandom() => new();
}
""";

    private const string Output =
"""
// <auto-generated/>
#nullable enable
namespace TestNamespace;

[global::System.Diagnostics.DebuggerDisplay("{Value}")]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Auto-generated.")]
readonly partial struct StringValueObject :
	global::TransparentValueObjects.Augments.IValueObject<global::System.String>,
	global::System.IEquatable<StringValueObject>,
	global::System.IEquatable<global::System.String>,
	global::System.IComparable<StringValueObject>
{
	public readonly global::System.String Value;

	[global::System.Obsolete($"Use StringValueObject.{nameof(From)} instead.", error: true)]
	public StringValueObject()
	{
		throw new global::System.InvalidOperationException($"Use StringValueObject.{nameof(From)} instead.");
	}

	private StringValueObject(global::System.String value)
	{
		Value = value;
	}

	public static StringValueObject From(global::System.String value) => new(value);

	public override int GetHashCode() => Value.GetHashCode();

	public override string ToString() => Value.ToString();

	public bool Equals(StringValueObject other) => Equals(other.Value);
	public bool Equals(global::System.String? other) => Value.Equals(other);
	public bool Equals(StringValueObject other, global::System.Collections.Generic.IEqualityComparer<global::System.String> comparer) => comparer.Equals(Value, other.Value);
	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (obj is StringValueObject value) return Equals(value);
		if (obj is global::System.String innerValue) return Equals(innerValue);
		return false;
	}

	public static bool operator ==(StringValueObject left, StringValueObject right) => left.Equals(right);
	public static bool operator !=(StringValueObject left, StringValueObject right) => !left.Equals(right);

	public static bool operator ==(StringValueObject left, global::System.String right) => left.Equals(right);
	public static bool operator !=(StringValueObject left, global::System.String right) => !left.Equals(right);

	public static bool operator ==(global::System.String left, StringValueObject right) => right.Equals(left);
	public static bool operator !=(global::System.String left, StringValueObject right) => !right.Equals(left);

	public static explicit operator StringValueObject(global::System.String value) => From(value);
	public static explicit operator global::System.String(StringValueObject value) => value.Value;

	public static StringValueObject NewRandomValue()
	{
		var randomValue = GenerateRandomValue(GetRandom());
		return randomValue;
	}

	public global::System.Int32 CompareTo(StringValueObject other) => Value.CompareTo(other);
}
""";

    [Fact]
    public void TestAugment()
    {
        TestHelpers.TestGenerator(Input, "StringValueObject.g.cs", Output);
    }
}
