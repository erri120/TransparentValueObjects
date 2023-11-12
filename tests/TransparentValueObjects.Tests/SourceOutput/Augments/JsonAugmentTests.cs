using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

public class JsonAugmentTests
{
    [Fact]
    public void TestAugment_WithReferenceType()
    {
        const string input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<string>]
public readonly partial struct TestValueObject : IAugmentWith<DefaultValueAugment, JsonAugment> { }
""";

        const string expectedAttributes = /*lang=csharp*/
"""
#region Augment Attributes

[global::System.Text.Json.Serialization.JsonConverter(typeof(JsonConverter))]

#endregion Augment Attributes
""";

        const string expectedInterfaces = /*lang=csharp*/
"""
#region Augment Interfaces

	,global::TransparentValueObjects.IDefaultValue<TestValueObject, global::System.String>

#endregion Augment Interfaces
""";

        const string expectedJsonAugment = /*lang=csharp*/
"""
#region JSON Augment

    /// <summary>
    /// Custom JSON converter for the Value Object.
    /// </summary>
	public class JsonConverter : global::System.Text.Json.Serialization.JsonConverter<TestValueObject>
	{
		private static global::System.Text.Json.Serialization.JsonConverter<global::System.String> GetInnerValueConverter(global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = options.GetConverter(typeof(global::System.String));
			if (innerValueConverter is not global::System.Text.Json.Serialization.JsonConverter<global::System.String> converter)
				throw new global::System.Text.Json.JsonException($"Unable to find converter for type {typeof(global::System.String)}");
			return converter;
		}

		public override TestValueObject Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			var innerValue = innerValueConverter.Read(ref reader, typeof(global::System.String), options);
			return innerValue is null ? DefaultValue : From(innerValue);
		}

		public override void Write(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			innerValueConverter.Write(writer, value.Value, options);
		}

		public override TestValueObject ReadAsPropertyName(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			var innerValue = innerValueConverter.ReadAsPropertyName(ref reader, typeof(global::System.String), options);
			return From(innerValue);
		}

		public override void WriteAsPropertyName(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			innerValueConverter.WriteAsPropertyName(writer, value.Value, options);
		}

	}

#endregion JSON Augment
""";

        var output = TestHelpers.RunGenerator("TestValueObject.g.cs", input);
        TestHelpers.GetRegion(output, "Augment Attributes").Should().Be(expectedAttributes.SourceNormalize());
        TestHelpers.GetRegion(output, "Augment Interfaces").Should().Be(expectedInterfaces.SourceNormalize());
        TestHelpers.GetRegion(output, "JSON Augment").Should().Be(expectedJsonAugment.SourceNormalize());
    }

    [Fact]
    public void TestAugment_WithValueType()
    {
        const string input = /*lang=csharp*/
"""
using System;
using TransparentValueObjects;

namespace TestNamespace;

[ValueObject<int>]
public readonly partial struct TestValueObject : IAugmentWith<JsonAugment> { }
""";

        const string expectedAttributes = /*lang=csharp*/
"""
#region Augment Attributes

[global::System.Text.Json.Serialization.JsonConverter(typeof(JsonConverter))]

#endregion Augment Attributes
""";

        const string expectedJsonAugment = /*lang=csharp*/
"""
#region JSON Augment

    /// <summary>
    /// Custom JSON converter for the Value Object.
    /// </summary>
	public class JsonConverter : global::System.Text.Json.Serialization.JsonConverter<TestValueObject>
	{
		private static global::System.Text.Json.Serialization.JsonConverter<global::System.Int32> GetInnerValueConverter(global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = options.GetConverter(typeof(global::System.Int32));
			if (innerValueConverter is not global::System.Text.Json.Serialization.JsonConverter<global::System.Int32> converter)
				throw new global::System.Text.Json.JsonException($"Unable to find converter for type {typeof(global::System.Int32)}");
			return converter;
		}

		public override TestValueObject Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			var innerValue = innerValueConverter.Read(ref reader, typeof(global::System.Int32), options);
			return From(innerValue);
		}

		public override void Write(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			innerValueConverter.Write(writer, value.Value, options);
		}

		public override TestValueObject ReadAsPropertyName(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			var innerValue = innerValueConverter.ReadAsPropertyName(ref reader, typeof(global::System.Int32), options);
			return From(innerValue);
		}

		public override void WriteAsPropertyName(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValueConverter = GetInnerValueConverter(options);
			innerValueConverter.WriteAsPropertyName(writer, value.Value, options);
		}

	}

#endregion JSON Augment
""";

        var output = TestHelpers.RunGenerator("TestValueObject.g.cs", input);
        TestHelpers.GetRegion(output, "Augment Attributes").Should().Be(expectedAttributes.SourceNormalize());
        TestHelpers.GetRegion(output, "JSON Augment").Should().Be(expectedJsonAugment.SourceNormalize());
    }
}
