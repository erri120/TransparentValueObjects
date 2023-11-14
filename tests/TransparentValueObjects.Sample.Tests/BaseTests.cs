using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Sample.Tests;

public class BaseTests
{
    [Fact]
    public void Test_From()
    {
        var guid = Guid.NewGuid();
        var vo = SampleGuidValueObject.From(guid);
        vo.Value.Should().Be(guid);
    }

    [Fact]
    public void Test_Equality()
    {
        var guid = Guid.NewGuid();
        var left = SampleGuidValueObject.From(guid);
        var right = SampleGuidValueObject.From(guid);
        var empty = SampleGuidValueObject.From(Guid.Empty);

        left.Equals(right).Should().BeTrue();
        left.Equals(guid).Should().BeTrue();
        (left == right).Should().BeTrue();
        (left == guid).Should().BeTrue();
        (left != right).Should().BeFalse();
        (left != guid).Should().BeFalse();

        left.Equals(empty).Should().BeFalse();
        left.Equals(Guid.Empty).Should().BeFalse();
        (left == empty).Should().BeFalse();
        (left == Guid.Empty).Should().BeFalse();
        (left != empty).Should().BeTrue();
        (left != Guid.Empty).Should().BeTrue();
    }

    [Fact]
    public void Test_GetHashCode()
    {
        var guid = Guid.NewGuid();
        var vo = SampleGuidValueObject.From(guid);

        vo.GetHashCode().Should().Be(guid.GetHashCode());
        vo.GetHashCode().Should().NotBe(Guid.Empty.GetHashCode());
    }

    [Fact]
    public void Test_ToString()
    {
        var guid = Guid.NewGuid();
        var vo = SampleGuidValueObject.From(guid);

        vo.ToString().Should().Be(guid.ToString());
    }
}
