using Xunit;

namespace TransparentValueObjects.Tests.ValueObjectIncrementalSourceGeneratorTests;

public class BaseTests
{
    [Fact]
    public void Test_AddPublicConstructor_WithoutDefaultValue()
    {
        const string valueObjectTypeName = "MyValueObject";
        const string output =
$$"""
[global::System.Obsolete($"Use {{valueObjectTypeName}}.{nameof(From)} instead.", error: true)]
public {{valueObjectTypeName}}()
{
    throw new global::System.InvalidOperationException($"Use {{valueObjectTypeName}}.{nameof(From)} instead.");
}
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddPublicConstructor(cw, valueObjectTypeName, hasDefaultValue: false);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_AddPublicConstructor_WithDefaultValue()
    {
        const string valueObjectTypeName = "MyValueObject";
        const string output =
$$"""
public {{valueObjectTypeName}}()
{
    Value = DefaultValue.Value;
}
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddPublicConstructor(cw, valueObjectTypeName, hasDefaultValue: true);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_AddPrivateConstructor()
    {
        const string valueObjectTypeName = "MyValueObject";
        const string innerValueTypeName = "string";
        const string output =
$$"""
private {{valueObjectTypeName}}({{innerValueTypeName}} value)
{
    Value = value;
}
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddPrivateConstructor(cw, valueObjectTypeName, innerValueTypeName);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_OverrideBaseMethods()
    {
        const string output =
"""
public override int GetHashCode() => Value.GetHashCode();

public override string ToString() => Value.ToString();

""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.OverrideBaseMethods(cw);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_ImplementEqualsMethods_WithoutDefaultEqualityComparer()
    {
        const string valueObjectTypeName = "MyValueObject";
        const string innerValueTypeName = "string";
        const string output =
$$"""
public bool Equals({{valueObjectTypeName}} other) => Equals(other.Value);
public bool Equals({{innerValueTypeName}}? other) => Value.Equals(other);
public bool Equals({{valueObjectTypeName}} other, global::System.Collections.Generic.IEqualityComparer<{{innerValueTypeName}}> comparer) => comparer.Equals(Value, other.Value);
public override bool Equals(object? obj)
{
	if (obj is null) return false;
	if (obj is {{valueObjectTypeName}} value) return Equals(value);
	if (obj is {{innerValueTypeName}} innerValue) return Equals(innerValue);
	return false;
}
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementEqualsMethods(cw, valueObjectTypeName, innerValueTypeName, "?", hasDefaultEqualityComparer: false);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_ImplementEqualsMethods_WithDefaultEqualityComparer()
    {
        const string valueObjectTypeName = "MyValueObject";
        const string innerValueTypeName = "string";
        const string output =
$$"""
public bool Equals({{valueObjectTypeName}} other) => Equals(other.Value);
public bool Equals({{innerValueTypeName}}? other) => InnerValueDefaultEqualityComparer.Equals(Value, other);
public bool Equals({{valueObjectTypeName}} other, global::System.Collections.Generic.IEqualityComparer<{{innerValueTypeName}}> comparer) => comparer.Equals(Value, other.Value);
public override bool Equals(object? obj)
{
	if (obj is null) return false;
	if (obj is {{valueObjectTypeName}} value) return Equals(value);
	if (obj is {{innerValueTypeName}} innerValue) return Equals(innerValue);
	return false;
}
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementEqualsMethods(cw, valueObjectTypeName, innerValueTypeName, "?", hasDefaultEqualityComparer: true);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_AddEqualityOperators()
    {
        const string valueObjectTypeName = "MyValueObject";
        const string innerValueTypeName = "string";
        const string output =
$$"""
public static bool operator ==({{valueObjectTypeName}} left, {{valueObjectTypeName}} right) => left.Equals(right);
public static bool operator !=({{valueObjectTypeName}} left, {{valueObjectTypeName}} right) => !left.Equals(right);

public static bool operator ==({{valueObjectTypeName}} left, {{innerValueTypeName}} right) => left.Equals(right);
public static bool operator !=({{valueObjectTypeName}} left, {{innerValueTypeName}} right) => !left.Equals(right);

public static bool operator ==({{innerValueTypeName}} left, {{valueObjectTypeName}} right) => right.Equals(left);
public static bool operator !=({{innerValueTypeName}} left, {{valueObjectTypeName}} right) => !right.Equals(left);
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddEqualityOperators(cw, valueObjectTypeName, innerValueTypeName);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_AddExplicitCastOperators()
    {
        const string valueObjectTypeName = "MyValueObject";
        const string innerValueTypeName = "string";
        const string output =
$$"""
public static explicit operator {{valueObjectTypeName}}({{innerValueTypeName}} value) => From(value);
public static explicit operator {{innerValueTypeName}}({{valueObjectTypeName}} value) => value.Value;
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddExplicitCastOperators(cw, valueObjectTypeName, innerValueTypeName);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }

    [Fact]
    public void Test_AddGuidSpecificCode()
    {
        const string valueObjectTypeName = "MyId";
        const string innerValueTypeName = "Guid";
        const string output =
$$"""
public static {{valueObjectTypeName}} NewId() => From(Guid.NewGuid());
""";

        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddGuidSpecificCode(cw, valueObjectTypeName, innerValueTypeName);

        TestHelpers.NormalizeEquals(cw.ToString(), output);
    }
}
