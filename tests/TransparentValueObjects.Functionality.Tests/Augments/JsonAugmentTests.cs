using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests.Augments;

public partial class JsonAugmentTests
{
    [ValueObject<Guid>]
    private readonly partial struct MyGuidValueObject : IAugmentWith<JsonAugment> { }

    [ValueObject<string>]
    private readonly partial struct MyStringValueObject : IAugmentWith<DefaultValueAugment, JsonAugment>
    {
        public static MyStringValueObject DefaultValue { get; } = From(string.Empty);
    }

    [ValueObject<short>]
    private readonly partial struct MyInt16ValueObject : IAugmentWith<JsonAugment> { }

    [ValueObject<int>]
    private readonly partial struct MyInt32ValueObject : IAugmentWith<JsonAugment> { }

    [ValueObject<long>]
    private readonly partial struct MyInt64ValueObject : IAugmentWith<JsonAugment> { }

    [ValueObject<ushort>]
    private readonly partial struct MyUInt16ValueObject : IAugmentWith<JsonAugment> { }

    [ValueObject<uint>]
    private readonly partial struct MyUInt32ValueObject : IAugmentWith<JsonAugment> { }

    [ValueObject<ulong>]
    private readonly partial struct MyUInt64ValueObject : IAugmentWith<JsonAugment> { }

    [Fact]
    public void Test_Guid()
    {
        var vo = MyGuidValueObject.From(Guid.Parse("803dba68-87ca-4dec-b003-3792a228545e"));
        TestConverter(vo, vo.Value, "{\"803dba68-87ca-4dec-b003-3792a228545e\":\"803dba68-87ca-4dec-b003-3792a228545e\"}");
    }

    [Fact]
    public void Test_String()
    {
        var vo = MyStringValueObject.From("Hello World");
        TestConverter(vo, vo.Value, "{\"Hello World\":\"Hello World\"}");
    }

    [Fact]
    public void Test_Int16()
    {
        var vo = MyInt16ValueObject.From(1337);
        TestNumberConverter(vo, vo.Value, "{\"1337\":1337}", "{\"1337\":\"1337\"}");
    }

    [Fact]
    public void Test_Int32()
    {
        var vo = MyInt32ValueObject.From(1337);
        TestNumberConverter(vo, vo.Value, "{\"1337\":1337}", "{\"1337\":\"1337\"}");
    }

    [Fact]
    public void Test_Int64()
    {
        var vo = MyInt64ValueObject.From(1337);
        TestNumberConverter(vo, vo.Value, "{\"1337\":1337}", "{\"1337\":\"1337\"}");
    }

    [Fact]
    public void Test_UInt16()
    {
        var vo = MyUInt16ValueObject.From(1337);
        TestNumberConverter(vo, vo.Value, "{\"1337\":1337}", "{\"1337\":\"1337\"}");
    }

    [Fact]
    public void Test_UInt32()
    {
        var vo = MyUInt32ValueObject.From(1337);
        TestNumberConverter(vo, vo.Value, "{\"1337\":1337}", "{\"1337\":\"1337\"}");
    }

    [Fact]
    public void Test_UInt64()
    {
        var vo = MyUInt64ValueObject.From(1337);
        TestNumberConverter(vo, vo.Value, "{\"1337\":1337}", "{\"1337\":\"1337\"}");
    }

    private static void TestWithDict<TKey, TValue>(
        Dictionary<TKey, TValue> dict,
        [LanguageInjection("json")] string expected,
        TKey expectedKey,
        TValue expectedValue) where TKey : notnull
    {
        var json = JsonSerializer.Serialize(dict);
        json.Should().Be(expected);

        var res = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(json);
        res.Should().NotBeNull();
        res!.Should().HaveCount(dict.Count);
        res!.Should().ContainKey(expectedKey);
        res![expectedKey].Should().Be(expectedValue);
    }

    private static void TestConverter<TValueObject, TInnerValue>(
        TValueObject vo,
        TInnerValue innerValue,
        [LanguageInjection("json")] string expected)
        where TValueObject : IValueObject<TInnerValue>
        where TInnerValue : notnull
    {
        TestWithDict(new Dictionary<TValueObject, TValueObject>
        {
            { vo, vo },
        }, expected, vo, vo);

        TestWithDict(new Dictionary<TValueObject, TInnerValue>
        {
            { vo, innerValue },
        }, expected, vo, innerValue);
    }

    private static void TestNumberConverter<TValueObject, TInnerValue>(
        TValueObject vo,
        TInnerValue innerValue,
        [LanguageInjection("json")] string expected,
        [LanguageInjection("json")] string expectedAsString)
        where TValueObject : IValueObject<TInnerValue>
        where TInnerValue : notnull
    {
        TestConverter(vo, innerValue, expected);

        TestWithDict(new Dictionary<TValueObject, string>
        {
            { vo, innerValue.ToString()! },
        }!, expectedAsString, vo, innerValue.ToString());
    }
}


