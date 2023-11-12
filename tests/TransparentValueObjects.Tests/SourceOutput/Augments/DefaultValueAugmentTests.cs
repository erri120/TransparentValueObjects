using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

public class DefaultValueAugmentTests
{
    [Fact]
    public void TestAugment()
    {
        const string input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject : IAugmentWith<DefaultValueAugment> { }
""";

        const string expectedInterfaces = /*lang=csharp*/
"""
#region Augment Interfaces

	,global::TransparentValueObjects.IDefaultValue<TestValueObject, global::System.String>

#endregion Augment Interfaces
""";

        const string expectedConstructors = /*lang=csharp*/
"""
#region Constructors

	private TestValueObject(global::System.String value)
	{
		Value = value;
	}

	/// <summary>
	/// Default constructor using <see cref="DefaultValue"/> as the underlying value.
	/// </summary>
	public TestValueObject()
	{
		Value = DefaultValue.Value;
	}

#endregion Constructors
""";

        var output = TestHelpers.RunGenerator("TestValueObject.g.cs", input);

        var interfaces = TestHelpers.GetRegion(output, "Augment Interfaces");
        interfaces.Should().Be(expectedInterfaces.SourceNormalize());

        var constructors = TestHelpers.GetRegion(output, "Constructors");
        constructors.Should().Be(expectedConstructors.SourceNormalize());
    }
}
