using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Qowaiv.Internals;

namespace Qowaiv.Bson.MongoDB
{
    public class QowaivBsonConverter<TSvo>  : SerializerBase<TSvo>
    {
        private readonly IBsonSerializer<TSvo> serializer = new ConventionBasedSerializer<TSvo>();

        /// <inheritdoc/>
        public override TSvo Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return serializer.Deserialize(context, args);
        }

        /// <inheritdoc/>
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TSvo value)
        {
            serializer.Serialize(context, args, value);
        }
    }
}
