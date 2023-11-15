using System;
using System.Collections.Generic;
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
}
