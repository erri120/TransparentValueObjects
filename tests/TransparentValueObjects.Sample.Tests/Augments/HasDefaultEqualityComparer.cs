using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Sample.Tests.Augments;

public class HasDefaultEqualityComparer
{
    [Fact]
    public void Test_DefaultEqualityComparer()
    {
        var left = SampleValueObjectString.From("foo");
        var right = SampleValueObjectString.From("FOO");
        left.Equals(right).Should().BeTrue();
    }
}
