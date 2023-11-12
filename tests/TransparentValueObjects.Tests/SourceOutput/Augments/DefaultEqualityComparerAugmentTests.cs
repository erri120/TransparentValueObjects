using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public class DefaultEqualityComparerAugmentTests
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
public readonly partial struct TestValueObject : IAugmentWith<DefaultEqualityComparerAugment> { }
""";

        const string expectedInterfaces = /*lang=csharp*/
"""
#region Augment Interfaces

	,global::TransparentValueObjects.IDefaultEqualityComparer<TestValueObject, global::System.String>

#endregion Augment Interfaces
""";

        const string expectedEqualsMethods = /*lang=csharp*/
"""
#region Equals Methods

	/// <inheritdoc/>
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public bool Equals(TestValueObject other) => Equals(other.Value);

	/// <inheritdoc/>
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public bool Equals(global::System.String? other) => InnerValueDefaultEqualityComparer.Equals(Value, other);

	/// <summary>
	/// Determines whether the specified value is equal to the current inner value using a custom equality comparer.
	/// </summary>
	/// <param name="other">The value to compare with the current inner value.</param>
	/// <param name="comparer">The comparer to use.</param>
	/// <returns></returns>
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public bool Equals(global::System.String other, global::System.Collections.Generic.IEqualityComparer<global::System.String> comparer) => comparer.Equals(Value, other);

	/// <inheritdoc/>
	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (obj is TestValueObject value) return Equals(value);
		if (obj is global::System.String innerValue) return Equals(innerValue);
		return false;
	}

#endregion Equals Methods
""";

        var output = TestHelpers.RunGenerator("TestValueObject.g.cs", input);

        var interfaces = TestHelpers.GetRegion(output, "Augment Interfaces");
        interfaces.Should().Be(expectedInterfaces.SourceNormalize());

        var equalsMethods = TestHelpers.GetRegion(output, "Equals Methods");
        equalsMethods.Should().Be(expectedEqualsMethods.SourceNormalize());
    }
}
