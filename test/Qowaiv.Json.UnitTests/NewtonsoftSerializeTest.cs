using Newtonsoft.Json;
using NUnit.Framework;
using Qowaiv.Json.Newtonsoft;

namespace Qowaiv.Json.UnitTests;

public class NewtonsoftSerializeTest : JsonSerializeTestBase<JsonSerializationException>
{
    public NewtonsoftSerializeTest()
    {
        if (JsonConvert.DefaultSettings == null)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Converters = { new QowaivJsonConverter() } };
        }
        QowaivJsonConverter.Register();
    }

    [Test]
    public void Register_ClearAll_Successful()
    {
        var settings = JsonConvert.DefaultSettings.Invoke();
        settings.Converters.Clear();

        Assert.AreEqual(0, settings.Converters.Count, "Default converters should be empty.");

        QowaivJsonConverter.Register();

        settings = JsonConvert.DefaultSettings.Invoke();

        Assert.AreEqual(1, settings.Converters.Count, "Default converters should contain one converter.");
        Assert.AreEqual(typeof(QowaivJsonConverter), settings.Converters[0].GetType(), "Default converters should contain a QowaivJsonConverter.");
    }

    protected override T Deserialize<T>(string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    protected override string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}
