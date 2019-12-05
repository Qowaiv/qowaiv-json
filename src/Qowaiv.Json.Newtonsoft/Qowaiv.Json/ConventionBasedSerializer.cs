using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Qowaiv.Json
{
    internal partial class ConventionBasedSerializer<TSvo> : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TSvo) && TypeIsSupported;
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.NotNull(reader, nameof(reader));
            Guard.NotNull(objectType, nameof(objectType));

            var type = objectType;
            var isNullable = type != objectType || objectType.IsClass;

            try
            {
                switch (reader.TokenType)
                {
                    // Empty value for null-ables.
                    case JsonToken.Null:
                        return isNullable ? null : Activator.CreateInstance(objectType);

                    case JsonToken.String:
                        return fromJsonString((string)reader.Value);

                    case JsonToken.Float:
                        return fromJsonDouble((double)reader.Value);

                    case JsonToken.Integer:
                        return fromJsonLong((long)reader.Value);

                    case JsonToken.Boolean:
                        return fromJsonBool(true.Equals(reader.Value));

                    case JsonToken.Date:
                        return fromJsonString(((DateTime)reader.Value).ToString(CultureInfo.InvariantCulture));

                    // Other scenario's are not supported.
                    default:
                        throw new JsonSerializationException($"Unexpected token parsing {objectType.FullName}. {reader.TokenType} is not supported.");
                }
            }

            // We want to communicate exceptions as JSON serialization exceptions.
            catch (Exception x)
            {
                if (x is JsonSerializationException || x is JsonReaderException)
                {
                    throw;
                }

                throw new JsonSerializationException(x.Message, x);
            }
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guard.NotNull(writer, nameof(writer));

            var json = (TSvo)value;

            var node = ToJson(json);

            if (node is null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(node);
            }
        }
    }
}
