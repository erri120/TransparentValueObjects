using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput;

[UsesVerify]
public class BaseTests
{
    [Fact]
    public Task Test_AddConstructors_WithoutDefaultValue()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddConstructors(cw, "TestValueObject", "global::System.String", hasDefaultValue: false);
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_AddConstructors_WithDefaultValue()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddConstructors(cw, "TestValueObject", "global::System.String", hasDefaultValue: true);
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_OverrideBaseMethods()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.OverrideBaseMethods(cw, hasDefaultEqualityComparer: false);
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_OverrideBaseMethods_WithDefaultEqualityComparer()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.OverrideBaseMethods(cw, hasDefaultEqualityComparer: true);
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_ImplementEqualsMethods_WithoutDefaultEqualityComparer()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementEqualsMethods(cw, "TestValueObject", "global::System.String", "?", hasDefaultEqualityComparer: false);
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_ImplementEqualsMethods_WithDefaultEqualityComparer()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementEqualsMethods(cw, "TestValueObject", "global::System.String", "?", hasDefaultEqualityComparer: true);
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_AddEqualityOperators()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddEqualityOperators(cw, "TestValueObject", "global::System.String");
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_AddExplicitCastOperators()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddExplicitCastOperators(cw, "TestValueObject", "global::System.String");
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_ImplementComparable()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementComparable(cw, "TestValueObject", "global::System.Guid", "");
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_ImplementComparable_Nullable()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.ImplementComparable(cw, "TestValueObject", "global::System.String", "?");
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_AddComparisonOperators()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddComparisonOperators(cw, "TestValueObject", "global::System.String");
        return TestHelpers.Verify(cw.ToString());
    }

    [Fact]
    public Task Test_AddGuidSpecificCode()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddGuidSpecificCode(cw, "TestValueObject", "global::System.Guid");
        return TestHelpers.Verify(cw.ToString());
    }
}
