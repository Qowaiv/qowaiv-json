using Qowaiv.Json.Newtonsoft;
using System;

namespace Qowaiv.Json.UnitTests
{
    public class NewtonsoftCanConvertTest : JsonCanConvertTestBase
    {
        protected override bool CanConvert(Type type)
        {
            var converter = new QowaivJsonConverter();
            return converter.CanConvert(type);
        }
    }
}
