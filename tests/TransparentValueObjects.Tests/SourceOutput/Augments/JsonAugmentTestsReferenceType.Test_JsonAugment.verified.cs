#region JSON Augment

	/// <summary>
	/// Custom JSON converter for the Value Object.
	/// </summary>
	public class JsonConverter : global::System.Text.Json.Serialization.JsonConverter<TestValueObject>
	{
		/// <inheritdoc/>
		public override TestValueObject Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValue = reader.GetString();
			return innerValue is null ? DefaultValue : From(innerValue);
		}

		/// <inheritdoc/>
		public override void Write(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Value);
		}

		/// <inheritdoc/>
		public override TestValueObject ReadAsPropertyName(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValue = reader.GetString();
			return innerValue is null ? DefaultValue : From(innerValue);
		}

		/// <inheritdoc/>
		public override void WriteAsPropertyName(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			writer.WritePropertyName(value.Value);
		}

	}

#endregion JSON Augment