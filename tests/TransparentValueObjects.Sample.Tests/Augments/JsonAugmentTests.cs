using System;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Sample.Tests.Augments;

public class JsonAugmentTests
{
    [Fact]
    public void Test_ReadWrite()
    {
        var vo = SampleGuidValueObject.From(Guid.Parse("803dba68-87ca-4dec-b003-3792a228545e"));

        var json = JsonSerializer.Serialize(vo);
        json.Should().Be("\"803dba68-87ca-4dec-b003-3792a228545e\"");

        var res = JsonSerializer.Deserialize<SampleGuidValueObject>(json);
        res.Should().Be(vo);
    }

    [Fact]
    public void Test_ReadWriteAsPropertyName()
    {
        // TODO: issue #8
    }
}
