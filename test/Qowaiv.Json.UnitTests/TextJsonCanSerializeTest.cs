using Qowaiv.Text.Json.Serialization;
using System;

namespace Qowaiv.Json.UnitTests
{
    public class TextJsonCanSerializeTest : JsonCanConvertTestBase
    {
        protected override bool CanConvert(Type type)
        {
            var factory = new QowaivJsonConverter();
            return factory.CanConvert(type);
        }
    }
}
