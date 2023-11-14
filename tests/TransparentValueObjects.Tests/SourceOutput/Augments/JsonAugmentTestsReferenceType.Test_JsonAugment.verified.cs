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
