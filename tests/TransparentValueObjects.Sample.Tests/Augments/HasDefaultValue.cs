using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Sample.Tests.Augments;

public class HasDefaultValue
{
    [Fact]
    public void Test_DefaultValue()
    {
        SampleValueObjectGuid.DefaultValue.Equals(Guid.Empty).Should().BeTrue();
    }
}
