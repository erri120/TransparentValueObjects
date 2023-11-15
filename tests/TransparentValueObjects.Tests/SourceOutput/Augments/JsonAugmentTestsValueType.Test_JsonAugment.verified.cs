#region JSON Augment

	/// <summary>
	/// Custom JSON converter for the Value Object.
	/// </summary>
	public class JsonConverter : global::System.Text.Json.Serialization.JsonConverter<TestValueObject>
	{
		public override TestValueObject Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValue = reader.GetGuid();
			return From(innerValue);
		}

		public override void Write(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Value);
		}

		public override TestValueObject ReadAsPropertyName(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var innerValue = reader.GetString();
			return From(innerValue is null ? global::System.Guid.Empty : global::System.Guid.Parse(innerValue));
		}

		public override void WriteAsPropertyName(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			writer.WritePropertyName(value.Value.ToString());
		}

	}

#endregion JSON Augment