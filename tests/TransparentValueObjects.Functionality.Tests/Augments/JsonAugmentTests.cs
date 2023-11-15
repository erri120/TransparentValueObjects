using System;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests.Augments;

public partial class JsonAugmentTests
{
    [ValueObject<Guid>]
    private readonly partial struct MyGuidValueObject : IAugmentWith<JsonAugment> { }

    [Fact]
    public void Test_ReadWrite()
    {
        var vo = MyGuidValueObject.From(Guid.Parse("803dba68-87ca-4dec-b003-3792a228545e"));

        var json = JsonSerializer.Serialize(vo);
        json.Should().Be("\"803dba68-87ca-4dec-b003-3792a228545e\"");

        var res = JsonSerializer.Deserialize<MyGuidValueObject>(json);
        res.Should().Be(vo);
    }

    [Fact]
    public void Test_ReadWriteAsPropertyName()
    {
        // TODO: issue #8
    }
}
