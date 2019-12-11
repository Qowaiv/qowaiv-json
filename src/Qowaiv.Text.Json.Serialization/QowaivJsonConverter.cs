using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Text.Json.Serialization
{
    /// <summary>The Qowaiv JSON converter converts types .</summary>
    public class QowaivJsonConverter : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert != null
                && !TypeHelper.GetNotNullableType(typeToConvert).IsPrimitive
                && CreateConverter(typeToConvert, null) != null;
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var type = TypeHelper.GetNotNullableType(typeToConvert);

            if (notSupported.Contains(type))
            {
                return null;
            }

            if (!converters.TryGetValue(typeToConvert, out JsonConverter converter))
            {
                lock (locker)
                {
                    if (!converters.TryGetValue(typeToConvert, out converter))
                    {
                        var converterType = typeof(ConventionBasedSerializer<>).MakeGenericType(typeToConvert);
                        converter = (JsonConverter)Activator.CreateInstance(converterType);

                        if (converter.CanConvert(typeToConvert))
                        {
                            converters[typeToConvert] = converter;
                        }
                        else
                        {
                            notSupported.Add(type);
                            return null;
                        }
                    }
                }
            }

            return converter;
        }

        private readonly object locker = new object();
        private readonly Dictionary<Type, JsonConverter> converters = new Dictionary<Type, JsonConverter>();
        private readonly HashSet<Type> notSupported = new HashSet<Type>();
    }
}
