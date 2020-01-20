using MongoDB.Bson.Serialization;
using System;

namespace Qowaiv.Bson.MongoDB
{
    public static class QowaivBsonConverter
    {
        public static void RegisterType(Type type)
        {
            var converter = (IBsonSerializer)Activator.CreateInstance(MakeGenericType(type));
            BsonSerializer.RegisterSerializer(type, converter);
        }

        public static void RegisterType<TSvo>()
        {
            BsonSerializer.RegisterSerializer(new QowaivBsonConverter<TSvo>());
        }

        private static Type MakeGenericType(Type type)
        {
            return typeof(QowaivBsonConverter<>).MakeGenericType(type);
        }
    }
}
