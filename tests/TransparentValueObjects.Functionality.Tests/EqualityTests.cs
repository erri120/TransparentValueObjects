using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests;

public partial class EqualityTests
{
    [ValueObject<Guid>]
    private readonly partial struct MyGuidValueObject { }

    [ValueObject<string>]
    private readonly partial struct MyStringValueObject { }

    [Fact]
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public void Test_Equality_ValueType()
    {
        var guid = Guid.NewGuid();
        var left = MyGuidValueObject.From(guid);
        var right = MyGuidValueObject.From(guid);
        var empty = MyGuidValueObject.From(Guid.Empty);

        left.Equals(right).Should().BeTrue();
        left.Equals(guid).Should().BeTrue();
        left.Equals((object)guid).Should().BeTrue();

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
    [SuppressMessage("Usage", "MA0002:IEqualityComparer<string> or IComparer<string> is missing")]
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public void Test_Equality_ReferenceType()
    {
        const string s = "Hello World!";
        var left = MyStringValueObject.From(s);
        var right = MyStringValueObject.From(s);
        var empty = MyStringValueObject.From(string.Empty);

        left.Equals(right).Should().BeTrue();
        left.Equals(s).Should().BeTrue();
        left.Equals(s, StringComparer.Ordinal).Should().BeTrue();
        left.Equals((object?)s).Should().BeTrue();

        (left == right).Should().BeTrue();
        (left == s).Should().BeTrue();
        (left != right).Should().BeFalse();
        (left != s).Should().BeFalse();

        left.Equals(empty).Should().BeFalse();
        left.Equals(string.Empty).Should().BeFalse();

        (left == empty).Should().BeFalse();
        (left == string.Empty).Should().BeFalse();
        (left != empty).Should().BeTrue();
        (left != string.Empty).Should().BeTrue();
    }
}
