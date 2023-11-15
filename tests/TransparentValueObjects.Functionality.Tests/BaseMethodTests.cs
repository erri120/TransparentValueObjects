using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests;

public partial class BaseMethodTests
{
    [ValueObject<Guid>]
    private readonly partial struct MyGuidValueObject { }

    [Fact]
    public void Test_From()
    {
        var guid = Guid.NewGuid();
        var vo = MyGuidValueObject.From(guid);
        vo.Value.Should().Be(guid);
    }

    [Fact]
    public void Test_GetHashCode()
    {
        var guid = Guid.NewGuid();
        var vo = MyGuidValueObject.From(guid);
        vo.GetHashCode().Should().Be(guid.GetHashCode());
    }

    [Fact]
    public void Test_ToString()
    {
        var guid = Guid.NewGuid();
        var vo = MyGuidValueObject.From(guid);
        vo.ToString().Should().Be(guid.ToString());
    }
}
