using NUnit.Framework;
using Qowaiv.Identifiers;
using Qowaiv.Json.UnitTests.Models;
using System;

namespace Qowaiv.Json.UnitTests
{
    /// <remarks>
    /// This abstract base class should help to guarantee that different
    /// implementations have the same behaviour.
    /// </remarks>
    public abstract class JsonSerializeTestBase<TException>  where TException: Exception
    {
        [Test]
        public void Deserialize_Null_Successful()
        {
            var obj = Deserialize<SvoWithFromJson>(@"null");
            Assert.IsNull(obj.Value);
        }

        [Test]
        public void Deserialize_NullNullable_Successful()
        {
            var obj = Deserialize<SvoWithFromJson?>(@"null");
            Assert.IsFalse(obj.HasValue);
        }

        [Test]
        public void Deserialize_NullClass_Successful()
        {
            var obj = Deserialize<SvoWithFromJsonClass>(@"null");
            Assert.IsNull(obj);
        }

        [Test]
        public void Deserialize_String_Successful()
        {
            var obj = Deserialize<SvoWithFromJson>(@"""test""");
            Assert.AreEqual("test", obj.Value);
        }

        [Test]
        public void Deserialize_Long_Successful()
        {
            var obj = Deserialize<SvoWithFromJson>("666");
            Assert.AreEqual(666L, obj.Value);
        }

        [Test]
        public void Deserialize_Double_Successful()
        {
            var obj = Deserialize<SvoWithFromJson>("2.5");
            Assert.AreEqual(2.5, obj.Value);
        }

        [Test]
        public void Deserialize_True_Successful()
        {
            var obj = Deserialize<SvoWithFromJson>("true");
            Assert.AreEqual(true, obj.Value);
        }
        
        [Test]
        public void Deserialize_False_Successful()
        {
            var obj = Deserialize<SvoWithFromJson>("false");
            Assert.AreEqual(false, obj.Value);
        }

        [Test]
        public void Deseralize_SvoWithFromJsonClass_Successful()
        {
            var obj = Deserialize<SvoWithFromJsonClass>("2.5");
            Assert.AreEqual(2.5, obj.Value);
        }

        [Test]
        public void Deserialize_Object_Successful()
        {
            var json = @"{ ""Identifier"": 3, ""Svo"": 2017, ""Message"": ""Hello World!"" }";
            var dto = Deserialize<DtoClass>(json);

            Assert.AreEqual(3, dto.Identifier);
            Assert.AreEqual(new SvoWithFromJson(2017L), dto.Svo);
            Assert.AreEqual("Hello World!", dto.Message);
        }

        [Test]
        public void Deserialize_DtoWithNullSvo_Successful()
        {
            var json = @"{ ""Identifier"": 3, ""Svo"": null, ""Message"": ""Hello World!"" }";
            var dto = Deserialize<DtoClass>(json);

            Assert.AreEqual(3, dto.Identifier);
            Assert.AreEqual(default(SvoWithFromJson), dto.Svo);
            Assert.AreEqual("Hello World!", dto.Message);
        }

        [Test]
        public void Deserialize_GenericId_Successful()
        {
            var id = Deserialize<Id<ForGeneric>>("12");
            Assert.AreEqual(Id<ForGeneric>.Create(12), id);
        }

        [Test]
        public void Deserialize_NotSupported_Throws()
        {
            var x = Assert.Throws<TException>(() => Deserialize<SvoThatThrows>(@"""test"""));
            Assert.AreEqual("Value in the wrong format.", x.Message);
        }

        [Test]
        public void Serialize_Null_Successful()
        {
            var json = Serialize(new SvoWithFromJson());
            Assert.AreEqual("null", json);
        }

        [Test]
        public void Serialize_String_Successful()
        {
            var json = Serialize(new SvoWithFromJson("test"));
            Assert.AreEqual(@"""test""", json);
        }

        [Test]
        public void Serialize_Double_Successful()
        {
            var json = Serialize(new SvoWithFromJson(2.5));
            Assert.AreEqual(@"2.5", json);
        }

        [Test]
        public void Serialize_Long_Successful()
        {
            var json = Serialize(new SvoWithFromJson(666L));
            Assert.AreEqual(@"666", json);
        }

        [Test]
        public void Serialize_Boolean_Successful()
        {
            var json = Serialize(new SvoWithFromJson(true));
            Assert.AreEqual(@"true", json);
        }

        [Test]
        public void Serialize_Decimal_Successful()
        {
            var json = Serialize(new SvoWithFromJson(2.5m));
            Assert.AreEqual(@"2.5", json);
        }

        [Test]
        public void Serialize_GenericId_Successful()
        {
            var json = Serialize(Id<ForGeneric>.Create(12));
            Assert.AreEqual(@"12", json);
        }

        [Test]
        public virtual void Serialize_DateTime_Successful()
        {
            var json = Serialize(new SvoWithFromJson(new DateTime(2017, 06, 11)));
            Assert.AreEqual(@"""2017-06-11T00:00:00""", json);
        }

        protected abstract T Deserialize<T>(string jsonString);

        protected abstract string Serialize(object obj);
    }
}
