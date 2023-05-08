using System.Reflection;

namespace Qowaiv.Bson.MongoDB;

/// <summary>
/// Implements <see cref="IBsonSerializationProvider"/> using <see cref="QowaivBsonSerializer{TSvo}"/>.
/// </summary>
public sealed class QowaivBsonSerializationProvider : IBsonSerializationProvider
{
    private readonly HashSet<Type> NotSupported = new();
    private readonly Dictionary<Type, IBsonSerializer> Serializers = new();

    /// <inheritdoc />
    [Pure]
    public IBsonSerializer? GetSerializer(Type? type)
    {
        IBsonSerializer? serializer = null;

        if (TypeHelper.GetCandidateType(type) is not { } tp
            || NotSupported.Contains(tp)
            || Serializers.TryGetValue(tp, out serializer))
        {
            return serializer;
        }
        else
        {
            serializer = CreateSerializer(tp);
            if (serializer is { })
            {
                Serializers[tp] = serializer;
            }
            return serializer;
        }
    }

    /// <summary>Creates an instance of <see cref="QowaivBsonSerializer{TSvo}"/> based on the specified type.</summary>
    [Pure]
    private static IBsonSerializer? CreateSerializer(Type type)
    {
        var converterType = typeof(QowaivBsonSerializer<>).MakeGenericType(type);
        var serializer = (IBsonSerializer)Activator.CreateInstance(converterType)!;
        return IsSupported(serializer)
            ? serializer
            : null;
    }

    /// <summary>Returns true if a name based convention is supported.</summary>
    [Pure]
    private static bool IsSupported(IBsonSerializer converter)
        => (bool)converter
        .GetType()
        .GetProperty(nameof(QowaivBsonSerializer<object>.TypeIsSupported), NonPublicInstance)!
        .GetValue(converter, Array.Empty<object>())!;

    private const BindingFlags NonPublicInstance = (BindingFlags)36;
}
