using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Sample.Tests.Augments;

public class HasSystemTextJsonConverter
{
    [Fact]
    public void Test_Converter()
    {
        var guid = Guid.NewGuid();
        var vo = SampleValueObjectGuid.From(guid);

        var json = JsonSerializer.Serialize(vo);
        json.Should().Be(JsonSerializer.Serialize(guid));
    }
}
