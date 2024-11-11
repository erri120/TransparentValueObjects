using System;

namespace TransparentValueObjects;

public static class JsonAugmentWriter
{
    private enum BaseType
    {
        String,
        Guid,
        Int16,
        Int32,
        Int64,
        UInt16,
        UInt32,
        UInt64,
        None,
    }

    public static void AddJsonConverter(
        CodeWriter cw,
        string targetTypeSimpleName,
        string innerValueTypeGlobalName,
        bool isReferenceType)
    {
        using var _ = cw.AddRegionBlock("JSON Augment");

        // TODO: Newtonsoft.Json support
        // TODO: choose System.Text.Json and/or Newtonsoft.Json implementation depending on the imports

        BaseType baseType;
        if (string.Equals(innerValueTypeGlobalName, "global::System.String", StringComparison.Ordinal))
            baseType = BaseType.String;
        else if (string.Equals(innerValueTypeGlobalName, "global::System.Guid", StringComparison.Ordinal))
            baseType = BaseType.Guid;
        else if (string.Equals(innerValueTypeGlobalName, "global::System.Int16", StringComparison.Ordinal))
            baseType = BaseType.Int16;
        else if (string.Equals(innerValueTypeGlobalName, "global::System.Int32", StringComparison.Ordinal))
            baseType = BaseType.Int32;
        else if (string.Equals(innerValueTypeGlobalName, "global::System.Int64", StringComparison.Ordinal))
            baseType = BaseType.Int64;
        else if (string.Equals(innerValueTypeGlobalName, "global::System.UInt16", StringComparison.Ordinal))
            baseType = BaseType.UInt16;
        else if (string.Equals(innerValueTypeGlobalName, "global::System.UInt32", StringComparison.Ordinal))
            baseType = BaseType.UInt32;
        else if (string.Equals(innerValueTypeGlobalName, "global::System.UInt64", StringComparison.Ordinal))
            baseType = BaseType.UInt64;
        else
            baseType = BaseType.None;

        cw.AppendLine("/// <summary>");
        cw.AppendLine("/// Custom JSON converter for the Value Object.");
        cw.AppendLine("/// </summary>");
        cw.AppendLine($"public class JsonConverter : global::System.Text.Json.Serialization.JsonConverter<{targetTypeSimpleName}>");
        using (cw.AddBlock())
        {
            // GetInnerValueConverter method as a fallback
            if (baseType == BaseType.None)
            {
                cw.AppendLine($"private static global::System.Text.Json.Serialization.JsonConverter<{innerValueTypeGlobalName}> GetInnerValueConverter(global::System.Text.Json.JsonSerializerOptions options)");
                using (cw.AddBlock())
                {
                    cw.AppendLine($"var innerValueConverter = options.GetConverter(typeof({innerValueTypeGlobalName}));");
                    cw.AppendLine($"if (innerValueConverter is not global::System.Text.Json.Serialization.JsonConverter<{innerValueTypeGlobalName}> converter)");
                    cw.AppendLine($"\tthrow new global::System.Text.Json.JsonException($\"Unable to find converter for type {{typeof({innerValueTypeGlobalName})}}\");");
                    cw.AppendLine("return converter;");
                }
            }

            AddReadMethod(cw, targetTypeSimpleName, innerValueTypeGlobalName, isReferenceType, baseType);
            AddWriteMethod(cw, targetTypeSimpleName, baseType);
            AddReadAsPropertyNameMethod(cw, targetTypeSimpleName, innerValueTypeGlobalName, baseType);
            AddWriteAsPropertyNameMethod(cw, targetTypeSimpleName, baseType);
        }
    }

    private static void InnerValueOrDefault(CodeWriter cw, bool isReferenceType)
    {
        cw.AppendLine(isReferenceType
            ? "return innerValue is null ? DefaultValue : From(innerValue);"
            : "return From(innerValue);");
    }

    private static void ReadInteger(
        CodeWriter cw,
        string innerValueTypeGlobalName,
        string getMethod)
    {
        cw.AppendLine("if ((options.NumberHandling & global::System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString) == global::System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString && reader.TokenType == global::System.Text.Json.JsonTokenType.String)");
        using (cw.AddBlock())
        {
            cw.AppendLine("var stringValue = reader.GetString();");
            cw.AppendLine($"var innerValue = stringValue is null ? default : {innerValueTypeGlobalName}.Parse(stringValue, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture);");
            cw.AppendLine("return From(innerValue);");
        }

        cw.AppendLine($"return From(reader.{getMethod}());");
    }

    private static void ReadIntegerAsPropertyName(
        CodeWriter cw,
        string innerValueTypeGlobalName)
    {
        cw.AppendLine("var stringValue = reader.GetString();");
        cw.AppendLine($"var innerValue = stringValue is null ? default : {innerValueTypeGlobalName}.Parse(stringValue, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture);");
        cw.AppendLine("return From(innerValue);");
    }

    private static void AddReadMethod(
        CodeWriter cw,
        string targetTypeSimpleName,
        string innerValueTypeGlobalName,
        bool isReferenceType,
        BaseType baseType)
    {
        cw.AppendLine("/// <inheritdoc />");
        cw.AppendLine($"public override {targetTypeSimpleName} Read(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)");
        using var _ = cw.AddBlock();

        switch (baseType)
        {
            case BaseType.String:
                cw.AppendLine("var innerValue = reader.GetString();");
                InnerValueOrDefault(cw, isReferenceType: true);
                break;
            case BaseType.Guid:
                cw.AppendLine("var innerValue = reader.GetGuid();");
                InnerValueOrDefault(cw, isReferenceType: false);
                break;
            case BaseType.Int16:
            case BaseType.Int32:
            case BaseType.Int64:
            case BaseType.UInt16:
            case BaseType.UInt32:
            case BaseType.UInt64:
                ReadInteger(cw, innerValueTypeGlobalName, $"Get{baseType.ToString()}");
                break;
            case BaseType.None:
                cw.AppendLine("var innerValueConverter = GetInnerValueConverter(options);");
                cw.AppendLine($"var innerValue = innerValueConverter.Read(ref reader, typeof({innerValueTypeGlobalName}), options);");
                InnerValueOrDefault(cw, isReferenceType);
                break;
        }
    }

    private static void AddWriteMethod(
        CodeWriter cw,
        string targetTypeSimpleName,
        BaseType baseType)
    {
        cw.AppendLine("/// <inheritdoc />");
        cw.AppendLine($"public override void Write(global::System.Text.Json.Utf8JsonWriter writer, {targetTypeSimpleName} value, global::System.Text.Json.JsonSerializerOptions options)");
        using var _ = cw.AddBlock();

        switch (baseType)
        {
            case BaseType.String:
            case BaseType.Guid:
                cw.AppendLine("writer.WriteStringValue(value.Value);");
                break;
            case BaseType.Int16:
            case BaseType.Int32:
            case BaseType.Int64:
            case BaseType.UInt16:
            case BaseType.UInt32:
            case BaseType.UInt64:
                cw.AppendLine("writer.WriteNumberValue(value.Value);");
                break;
            case BaseType.None:
                cw.AppendLine("var innerValueConverter = GetInnerValueConverter(options);");
                cw.AppendLine("innerValueConverter.Write(writer, value.Value, options);");
                break;
        }
    }

    private static void AddReadAsPropertyNameMethod(
        CodeWriter cw,
        string targetTypeSimpleName,
        string innerValueTypeGlobalName,
        BaseType baseType)
    {
        cw.AppendLine("/// <inheritdoc />");
        cw.AppendLine($"public override {targetTypeSimpleName} ReadAsPropertyName(ref global::System.Text.Json.Utf8JsonReader reader, global::System.Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)");
        using var _ = cw.AddBlock();

        switch (baseType)
        {
            case BaseType.String:
                cw.AppendLine("var innerValue = reader.GetString();");
                InnerValueOrDefault(cw, isReferenceType: true);
                break;
            case BaseType.Guid:
                cw.AppendLine("var innerValue = reader.GetString();");
                cw.AppendLine("return From(innerValue is null ? global::System.Guid.Empty : global::System.Guid.Parse(innerValue));");
                break;
            case BaseType.Int16:
            case BaseType.Int32:
            case BaseType.Int64:
            case BaseType.UInt16:
            case BaseType.UInt32:
            case BaseType.UInt64:
                ReadIntegerAsPropertyName(cw, innerValueTypeGlobalName);
                break;
            case BaseType.None:
                cw.AppendLine("var innerValueConverter = GetInnerValueConverter(options);");
                cw.AppendLine($"var innerValue = innerValueConverter.ReadAsPropertyName(ref reader, typeof({innerValueTypeGlobalName}), options);");
                cw.AppendLine("return From(innerValue);");
                break;
        }
    }

    private static void AddWriteAsPropertyNameMethod(
        CodeWriter cw,
        string targetTypeSimpleName,
        BaseType baseType)
    {
        cw.AppendLine("/// <inheritdoc />");
        cw.AppendLine($"public override void WriteAsPropertyName(global::System.Text.Json.Utf8JsonWriter writer, {targetTypeSimpleName} value, global::System.Text.Json.JsonSerializerOptions options)");
        using var _ = cw.AddBlock();

        switch (baseType)
        {
            case BaseType.String:
                cw.AppendLine("writer.WritePropertyName(value.Value);");
                break;
            case BaseType.Guid:
                cw.AppendLine("writer.WritePropertyName(value.Value.ToString());");
                break;
            case BaseType.Int16:
            case BaseType.Int32:
            case BaseType.Int64:
            case BaseType.UInt16:
            case BaseType.UInt32:
            case BaseType.UInt64:
                cw.AppendLine("writer.WritePropertyName(value.Value.ToString(global::System.Globalization.CultureInfo.InvariantCulture));");
                break;
            case BaseType.None:
                cw.AppendLine("var innerValueConverter = GetInnerValueConverter(options);");
                cw.AppendLine("innerValueConverter.WriteAsPropertyName(writer, value.Value, options);");
                break;
        }
    }
}
