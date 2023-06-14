using MongoDB.Bson.Serialization;
using NUnit.Framework;
using Qowaiv.Bson.MongoDB;
using Qowaiv.Json.UnitTests.Models;
using Qowaiv.Json.UnitTests.TestTools;
using System;

namespace Qowaiv.Json.UnitTests;

public class BsonMongoDBSerializeTest : JsonSerializeTestBase<FormatException>
{
    public BsonMongoDBSerializeTest()
    {
        BsonSerializer.RegisterSerializationProvider(new QowaivBsonSerializationProvider());
    }

    [Test, Ignore("For BSON a Date() is generated, that is default behaviour we don't want to interfere with.")]
    public override void Serialize_DateTime_Successful() { }

    [Test]
    public void Serialize_LongBiggerThanIntMax_Successful()
    {
        var json = Serialize(new SvoWithFromJson(123456789000));
        Assert.AreEqual(@"NumberLong(""123456789000"")", json);
    }

    protected override T Deserialize<T>(string jsonString) => BsonSerializer.Deserialize<T>(jsonString);

    protected override string Serialize(object obj) => TestSerializer.BsonSerialize(obj);
}
