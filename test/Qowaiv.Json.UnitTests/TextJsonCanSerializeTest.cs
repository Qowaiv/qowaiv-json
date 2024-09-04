﻿using Qowaiv.Text.Json.Serialization;

namespace Qowaiv.Json.UnitTests;

public class TextJsonCanSerializeTest : JsonCanConvertTestBase
{
    protected override bool CanConvert(Type? type)
    {
        var factory = new QowaivJsonConverter();
        return type is { } &&  factory.CanConvert(type);
    }
}
