using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json
{
    internal partial class ConventionBasedSerializer<TSvo> : JsonConverter<TSvo>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(TSvo) && TypeIsSupported;
        }

        /// <inheritdoc />
        public override TSvo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                        return FromJson(reader.GetString());

                    case JsonTokenType.True:
                        return FromJson(true);

                    case JsonTokenType.False:
                        return FromJson(false);

                    case JsonTokenType.Number:
                        if (reader.TryGetInt64(out long num))
                        {
                            return FromJson(num);
                        }
                        else if (reader.TryGetDouble(out double dec))
                        {
                            return FromJson(dec);
                        }
                        else
                        {
                            throw new JsonException($"QowaivJsonConverter does not support writing from {reader.GetString()}.");
                        }

                    case JsonTokenType.Null:
                        return default;

                    default:
                        throw new JsonException($"Unexpected token parsing {typeToConvert.FullName}. {reader.TokenType} is not supported.");
                }
            }
            catch (Exception x)
            {
                if (x is JsonException)
                {
                    throw;
                }

                throw new JsonException(x.Message, x);
            }
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, TSvo value, JsonSerializerOptions options)
        {
            var obj = ToJson(value);

            if (obj is null)
            {
                writer.WriteNullValue();
            }
            else if (obj is string str)
            {
                writer.WriteStringValue(str);
            }
            else if (obj is decimal dec)
            {
                writer.WriteNumberValue(dec);
            }
            else if (obj is double dbl)
            {
                writer.WriteNumberValue(dbl);
            }
            else if (obj is long num)
            {
                writer.WriteNumberValue(num);
            }
            else if (obj is int int_)
            {
                writer.WriteNumberValue(int_);
            }
            else if (obj is bool b)
            {
                writer.WriteBooleanValue(b);
            }
            else if (obj is DateTime dt)
            {
                writer.WriteStringValue(dt);
            }
            else if (obj is IFormattable f)
            {
                writer.WriteStringValue(f.ToString(null, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteStringValue(obj.ToString());
            }
        }
    }
}
