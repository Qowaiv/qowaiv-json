using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using Qowaiv.Bson.MongoDB;
using Qowaiv.Json.UnitTests.Models;
using System;
using System.IO;
using System.Text;

namespace Qowaiv.Json.UnitTests
{
    public class BsonMongoDBSerializeTest : JsonSerializeTestBase<FormatException>
    {
        public BsonMongoDBSerializeTest()
        {
            QowaivBsonConverter.RegisterType<SvoThatThrows>();
            QowaivBsonConverter.RegisterType<SvoWithFromJson>();
            QowaivBsonConverter.RegisterType<SvoWithFromJsonBoolOnly>();
            QowaivBsonConverter.RegisterType<SvoWithFromJsonClass>();
            QowaivBsonConverter.RegisterType<SvoWithFromJsonStringOnly>();
        }

        [Test, Ignore("For BSON a Date() is generated, that is default behaviour we don't want to interfere with.")]
        public override void Serialize_DateTime_Successful()
        {
            base.Serialize_DateTime_Successful();
        }

        [Test]
        public void Serialize_LongBiggerThanIntMax_Successful()
        {
            var json = Serialize(new SvoWithFromJson(123456789000));
            Assert.AreEqual(@"NumberLong(""123456789000"")", json);
        }

        protected override T Deserialize<T>(string jsonString)
        {
            return BsonSerializer.Deserialize<T>(jsonString);
        }

        protected override string Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                var jsonWriter = new JsonWriter(writer);
                var nominaleType = obj?.GetType();
                BsonSerializer.Serialize(jsonWriter, nominaleType, obj);
                jsonWriter.Flush();

                stream.Position = 0;

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
