using Xunit;

namespace TransparentValueObjects.Tests.ValueObjectIncrementalSourceGeneratorTests.Augments;

public class HasSystemTextJsonConverter_WithConverter
{
    private const string Input =
"""
using TransparentValueObjects.Generated;
using TransparentValueObjects.Augments;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct StringValueObject : IHasSystemTextJsonConverter
{
    public static global::System.Type SystemTextJsonConverterType => typeof(StringValueObject);
}
""";

    private const string Output =
"""
// <auto-generated/>
#nullable enable
namespace TestNamespace;

[global::System.Diagnostics.DebuggerDisplay("{Value}")]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Auto-generated.")]
[global::System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonConverter))]
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

	public global::System.Int32 CompareTo(StringValueObject other) => Value.CompareTo(other);
	public static bool operator <(StringValueObject left, StringValueObject right) => left.Value.CompareTo(right.Value) < 0;
	public static bool operator >(StringValueObject left, StringValueObject right) => left.Value.CompareTo(right.Value) > 0;
	public static bool operator <=(StringValueObject left, StringValueObject right) => left.Value.CompareTo(right.Value) <= 0;
	public static bool operator >=(StringValueObject left, StringValueObject right) => left.Value.CompareTo(right.Value) >= 0;

	public static bool operator <(global::System.String left, StringValueObject right) => left.CompareTo(right.Value) < 0;
	public static bool operator >(global::System.String left, StringValueObject right) => left.CompareTo(right.Value) > 0;
	public static bool operator <=(global::System.String left, StringValueObject right) => left.CompareTo(right.Value) <= 0;
	public static bool operator >=(global::System.String left, StringValueObject right) => left.CompareTo(right.Value) >= 0;

	public static bool operator <(StringValueObject left, global::System.String right) => left.Value.CompareTo(right) < 0;
	public static bool operator >(StringValueObject left, global::System.String right) => left.Value.CompareTo(right) > 0;
	public static bool operator <=(StringValueObject left, global::System.String right) => left.Value.CompareTo(right) <= 0;
	public static bool operator >=(StringValueObject left, global::System.String right) => left.Value.CompareTo(right) >= 0;

}
""";

    [Fact]
    public void TestAugment()
    {
        TestHelpers.TestGenerator(Input, "StringValueObject.g.cs", Output);
    }
}
