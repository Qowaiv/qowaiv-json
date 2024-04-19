using Qowaiv.Json.Newtonsoft;

namespace Qowaiv.Json.UnitTests;

public class NewtonsoftCanConvertTest : JsonCanConvertTestBase
{
    protected override bool CanConvert(Type type)
    {
        var converter = new QowaivJsonConverter();
        return converter.CanConvert(type);
    }
}
