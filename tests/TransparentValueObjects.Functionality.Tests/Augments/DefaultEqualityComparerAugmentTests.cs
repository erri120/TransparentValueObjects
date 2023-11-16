using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests.Augments;

public partial class DefaultEqualityComparerAugmentTests
{
    [ValueObject<string>]
    private readonly partial struct MyStringValueObject : IAugmentWith<DefaultEqualityComparerAugment>
    {
        public static IEqualityComparer<string> InnerValueDefaultEqualityComparer => StringComparer.OrdinalIgnoreCase;
    }

    [Fact]
    public void Test_InnerValueDefaultEqualityComparer()
    {
        MyStringValueObject.InnerValueDefaultEqualityComparer.Should().Be(StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Test_Equals()
    {
        var left = MyStringValueObject.From("foo");
        var right = MyStringValueObject.From("FOO");

        left.Equals(right).Should().BeTrue();
    }

    [Fact]
    [SuppressMessage("Usage", "MA0002:IEqualityComparer<string> or IComparer<string> is missing")]
    public void Test_GetHashCode()
    {
        var left = MyStringValueObject.From("foo");
        var right = MyStringValueObject.From("FOO");

        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    [Fact]
    public void Test_GetHashCode_WithEqualityComparer()
    {
        var left = MyStringValueObject.From("foo");
        var right = MyStringValueObject.From("FOO");

        left.GetHashCode(StringComparer.Ordinal).Should().NotBe(right.GetHashCode(StringComparer.Ordinal));
    }
}
