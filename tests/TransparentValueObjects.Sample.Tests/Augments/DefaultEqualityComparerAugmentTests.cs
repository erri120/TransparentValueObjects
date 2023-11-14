using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Sample.Tests.Augments;

public class DefaultEqualityComparerAugmentTests
{
    [Fact]
    public void Test_InnerValueDefaultEqualityComparer()
    {
        SampleStringValueObject.InnerValueDefaultEqualityComparer.Should().Be(StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Test_Equals()
    {
        var left = SampleStringValueObject.From("foo");
        var right = SampleStringValueObject.From("FOO");

        left.Equals(right).Should().BeTrue();
    }
}
