using NUnit.Framework;
using Qowaiv.Bson.MongoDB;
using Qowaiv.Json.UnitTests.TestTools;
using System;

namespace Qowaiv.Json.UnitTests;

public class BsonConverterRegisterTest
{
    [Test]
    public void RegisterAssembly_Qowaiv_SerializesSvoFromAssembly()
    {
        QowaivBsonConverter.RegisterAssembly(typeof(Date).Assembly);

        Assert.AreEqual(@"""2017-06-11""", TestSerializer.BsonSerialize(new Date(2017, 06, 11)));
    }

    [Test]
    public void RegisterTypeGeneric_Int_Throws()
    {
        Assert.Throws<NotSupportedException>(() => QowaivBsonConverter.RegisterType<int>());
    }

    [Test]
    public void RegisterType_Int_Throws()
    {
        Assert.Throws<NotSupportedException>(() => QowaivBsonConverter.RegisterType(typeof(int)));
    }
}
