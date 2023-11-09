using Xunit;

namespace TransparentValueObjects.Tests.ValueObjectIncrementalSourceGeneratorTests.Augments;

public class HasRandomValueGenerator_WithUnmanagedAndRandom
{
    private const string Input =
"""
using TransparentValueObjects.Generated;
using TransparentValueObjects.Augments;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct Int32ValueObject : IHasUnmanagedRandomValueGenerator<Int32ValueObject, int, Random>
{
    public static Random GetRandom() => Random.Shared;
}
""";

    private const string Output =
"""
// <auto-generated/>
#nullable enable
namespace TestNamespace;

[global::System.Diagnostics.DebuggerDisplay("{Value}")]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Auto-generated.")]
readonly partial struct Int32ValueObject :
	global::TransparentValueObjects.Augments.IValueObject<global::System.String>,
	global::System.IEquatable<Int32ValueObject>,
	global::System.IEquatable<global::System.String>,
	global::System.IComparable<Int32ValueObject>
{
	public readonly global::System.String Value;

	public static global::System.Type InnerValueType => typeof(global::System.String);

	[global::System.Obsolete($"Use Int32ValueObject.{nameof(From)} instead.", error: true)]
	public Int32ValueObject()
	{
		throw new global::System.InvalidOperationException($"Use Int32ValueObject.{nameof(From)} instead.");
	}

	private Int32ValueObject(global::System.String value)
	{
		Value = value;
	}

	public static Int32ValueObject From(global::System.String value) => new(value);

	public override int GetHashCode() => Value.GetHashCode();

	public override string ToString() => Value.ToString();

	public bool Equals(Int32ValueObject other) => Equals(other.Value);
	public bool Equals(global::System.String? other) => Value.Equals(other);
	public bool Equals(Int32ValueObject other, global::System.Collections.Generic.IEqualityComparer<global::System.String> comparer) => comparer.Equals(Value, other.Value);
	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (obj is Int32ValueObject value) return Equals(value);
		if (obj is global::System.String innerValue) return Equals(innerValue);
		return false;
	}

	public static bool operator ==(Int32ValueObject left, Int32ValueObject right) => left.Equals(right);
	public static bool operator !=(Int32ValueObject left, Int32ValueObject right) => !left.Equals(right);

	public static bool operator ==(Int32ValueObject left, global::System.String right) => left.Equals(right);
	public static bool operator !=(Int32ValueObject left, global::System.String right) => !left.Equals(right);

	public static bool operator ==(global::System.String left, Int32ValueObject right) => right.Equals(left);
	public static bool operator !=(global::System.String left, Int32ValueObject right) => !right.Equals(left);

	public static explicit operator Int32ValueObject(global::System.String value) => From(value);
	public static explicit operator global::System.String(Int32ValueObject value) => value.Value;

	public static Int32ValueObject NewRandomValue()
	{
		var random = GetRandom();
		var size = global::System.Runtime.CompilerServices.Unsafe.SizeOf<global::System.String>();
		global::System.Span<byte> bytes = stackalloc byte[size];
		random.NextBytes(bytes);
		var id = global::System.Runtime.InteropServices.MemoryMarshal.Cast<byte, global::System.String>(bytes)[0];
		return Int32ValueObject.From(id);
	}

	public global::System.Int32 CompareTo(Int32ValueObject other) => Value.CompareTo(other);
	public static bool operator <(Int32ValueObject left, Int32ValueObject right) => left.Value.CompareTo(right.Value) < 0;
	public static bool operator >(Int32ValueObject left, Int32ValueObject right) => left.Value.CompareTo(right.Value) > 0;
	public static bool operator <=(Int32ValueObject left, Int32ValueObject right) => left.Value.CompareTo(right.Value) <= 0;
	public static bool operator >=(Int32ValueObject left, Int32ValueObject right) => left.Value.CompareTo(right.Value) >= 0;

	public static bool operator <(global::System.String left, Int32ValueObject right) => left.CompareTo(right.Value) < 0;
	public static bool operator >(global::System.String left, Int32ValueObject right) => left.CompareTo(right.Value) > 0;
	public static bool operator <=(global::System.String left, Int32ValueObject right) => left.CompareTo(right.Value) <= 0;
	public static bool operator >=(global::System.String left, Int32ValueObject right) => left.CompareTo(right.Value) >= 0;

	public static bool operator <(Int32ValueObject left, global::System.String right) => left.Value.CompareTo(right) < 0;
	public static bool operator >(Int32ValueObject left, global::System.String right) => left.Value.CompareTo(right) > 0;
	public static bool operator <=(Int32ValueObject left, global::System.String right) => left.Value.CompareTo(right) <= 0;
	public static bool operator >=(Int32ValueObject left, global::System.String right) => left.Value.CompareTo(right) >= 0;

}
""";

    [Fact]
    public void TestAugment()
    {
        TestHelpers.TestGenerator(Input, "Int32ValueObject.g.cs", Output);
    }
}
