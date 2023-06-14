namespace Qowaiv.Bson.MongoDB.Qowaiv.Internals;

internal sealed class NullableBsonSerializer : IBsonSerializer
{
    private readonly IBsonSerializer Serializer;

    public NullableBsonSerializer(IBsonSerializer serializer) => Serializer = serializer;

    public Type ValueType => typeof(Nullable<>).MakeGenericType(Serializer.ValueType);

    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Serializer.Deserialize(context, args);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        Serializer.Serialize(context, args, value);
    }
}
