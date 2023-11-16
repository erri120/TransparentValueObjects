using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests;

public partial class CastingTests
{
    [ValueObject<Guid>]
    private readonly partial struct MyGuidValueObject { }

    [Fact]
    public void Test_ExplicitCastOperators()
    {
        var innerValue = Guid.NewGuid();
        var vo = MyGuidValueObject.From(innerValue);

        var voToInnerValue = (Guid)vo;
        voToInnerValue.Should().Be(innerValue);

        var innerValueToVo = (MyGuidValueObject)innerValue;
        innerValueToVo.Should().Be(vo);
    }
}
