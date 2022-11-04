using Qowaiv.Text.Json.Serialization;
using System.Text.Json;

namespace Qowaiv.Json.UnitTests;

public class TextJsonSerializeTest : JsonSerializeTestBase<JsonException>
{
    protected override T Deserialize<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString, options);
    }

    protected override string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj, options);
    }

    private static readonly JsonSerializerOptions options = GetOptions();

    private static JsonSerializerOptions GetOptions()
    {
        var o = new JsonSerializerOptions();
        o.Converters.Add(new QowaivJsonConverter());
        return o;
    }
}
