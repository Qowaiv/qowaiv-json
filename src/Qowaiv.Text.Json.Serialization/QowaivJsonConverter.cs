﻿using Qowaiv.Internals;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Text.Json.Serialization;

/// <summary>A JSON converter that converts Single Value Objects based on naming conventions.</summary>
public class QowaivJsonConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert is { }
        && !TypeHelper.NotNullable(typeToConvert).IsPrimitive
        && CreateConverter(typeToConvert, null) is { };

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var type = TypeHelper.NotNullable(typeToConvert);

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

    private readonly object locker = new();
    private readonly Dictionary<Type, JsonConverter> converters = new();
    private readonly HashSet<Type> notSupported = new();
}
