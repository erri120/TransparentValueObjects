#region JSON Augment

	/// <summary>
	/// Custom JSON converter for the Value Object.
	/// </summary>
	public class JsonConverter : global::System.Text.Json.Serialization.JsonConverter<TestValueObject>
	{
		/// <inheritdoc/>
		public override TestValueObject Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			if ((options.NumberHandling & global::System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString) == global::System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString && reader.TokenType == global::System.Text.Json.JsonTokenType.String)
			{
				var stringValue = reader.GetString();
				var innerValue = stringValue is null ? default : global::System.Int16.Parse(stringValue, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture);
				return From(innerValue);
			}

			return From(reader.GetInt16());
		}

		/// <inheritdoc/>
		public override void Write(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Value);
		}

		/// <inheritdoc/>
		public override TestValueObject ReadAsPropertyName(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
		{
			var stringValue = reader.GetString();
			var innerValue = stringValue is null ? default : global::System.Int16.Parse(stringValue, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture);
			return From(innerValue);
		}

		/// <inheritdoc/>
		public override void WriteAsPropertyName(global::System.Text.Json.Utf8JsonWriter writer, TestValueObject value, global::System.Text.Json.JsonSerializerOptions options)
		{
			writer.WritePropertyName(value.Value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));
		}

	}

#endregion JSON Augment