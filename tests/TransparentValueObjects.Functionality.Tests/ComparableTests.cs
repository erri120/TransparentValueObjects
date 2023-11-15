using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests;

public partial class ComparableTests
{
    [ValueObject<string>]
    private readonly partial struct MyStringValueObject { }

    [Fact]
    public void Test_CompareTo()
    {
        const string s1 = "0";
        const string s2 = "1";
        const string s3 = "2";

        var vo1 = MyStringValueObject.From(s1);
        var vo2 = MyStringValueObject.From(s2);
        var vo3 = MyStringValueObject.From(s3);

        vo1.CompareTo(vo1).Should().Be(0);
        vo1.CompareTo(vo2).Should().BeNegative();
        vo1.CompareTo(vo3).Should().BeNegative();

        vo2.CompareTo(vo1).Should().BePositive();
        vo2.CompareTo(vo3).Should().BeNegative();

        vo3.CompareTo(vo1).Should().BePositive();
        vo3.CompareTo(vo2).Should().BePositive();
    }

    [Fact]
    public void Test_ComparisonOperators()
    {
        const string s1 = "0";
        const string s2 = "1";

        var vo1 = MyStringValueObject.From(s1);
        var vo2 = MyStringValueObject.From(s2);

        (vo1 < vo2).Should().BeTrue();
        (vo1 < s2).Should().BeTrue();
        (vo1 > vo2).Should().BeFalse();
        (vo1 > s2).Should().BeFalse();
    }

    [Fact]
    public void Test_Sorting()
    {
        var input = new[]
        {
            MyStringValueObject.From("2"),
            MyStringValueObject.From("0"),
            MyStringValueObject.From("3"),
            MyStringValueObject.From("1"),
        };

        var expected = new[]
        {
            MyStringValueObject.From("0"),
            MyStringValueObject.From("1"),
            MyStringValueObject.From("2"),
            MyStringValueObject.From("3"),
        };

        Array.Sort(input);
        input.Should().ContainInConsecutiveOrder(expected);
    }
}
