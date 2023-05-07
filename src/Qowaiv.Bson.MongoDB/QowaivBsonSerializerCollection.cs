using System.Reflection;

namespace Qowaiv.Bson.MongoDB;

/// <summary>Contains a collection of <see cref="QowaivBsonSerializer{TSvo}"/>s.</summary>
public sealed class QowaivBsonSerializerCollection : IReadOnlyCollection<IBsonSerializer>
{
    private readonly Dictionary<Type, IBsonSerializer> Serializers = new();

    /// <summary>Gets types this collection contains a serializer for.</summary>
    public IReadOnlyCollection<Type> Types => Serializers.Keys;

    /// <inheritdoc />
    public int Count => Serializers.Count;

    /// <summary>Adds a serializer for the specified type, if the type is supported and not added yet.</summary>
    /// <param name="type">
    /// The type to register a serializer for.
    /// </param>
    /// <returns>
    /// True if a serializer was registered, otherwise false.
    /// </returns>
    [Impure]
    public bool Add(Type type)
    {
        Guard.NotNull(type, nameof(type));

        if (TypeHelper.GetCandidateType(type) is { } tp
            && !Contains(tp)
            && CreateConverter(tp) is { } converter
            && TypeIsSupported(converter))
        {
            Serializers[tp] = converter;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>Adds serializers for all applicable exportable types in the specified assembly.</summary>
    /// <param name="assembly">
    /// The assembly to add serializers for.
    /// </param>
    public void Add(Assembly assembly) => Add(assembly, exportableTypesOnly: true);

    /// <summary>Adds serializers for all applicable types in the specified assembly.</summary>
    /// <param name="assembly">
    /// The assembly to add serializers for.
    /// </param>
    /// <param name="exportableTypesOnly">
    /// If true, only exportable types are taken into consideration.
    /// </param>
    public void Add(Assembly assembly, bool exportableTypesOnly)
    {
        Guard.NotNull(assembly, nameof(assembly));
        AddRange(exportableTypesOnly 
            ? assembly.GetExportedTypes()
            : assembly.GetTypes());
    }

    /// <summary>Adds serializers for all applicable specified types.</summary>
    /// <param name="types">
    /// The types to add serializers for.
    /// </param>
    public void AddRange(IEnumerable<Type> types)
    {
        Guard.NotNull(types, nameof(types));

        foreach (var type in types.OfType<Type>())
        {
            Add(type);
        }
    }

    /// <summary>Returns true if the collection contains a serializer for the specified type.</summary>
    [Pure]
    public bool Contains(Type type) 
        => Serializers.ContainsKey(Guard.NotNull(type, nameof(type)));

    /// <summary>Removes the serializer for the specific type from the collection.</summary>
    /// <param name="type">
    /// The type to remove the serializer for.
    /// </param>
    /// <returns>
    /// True if the serializer was removed, otherwise false.
    /// </returns>
    [Impure]
    public bool Remove(Type type) => Serializers.Remove(Guard.NotNull(type, nameof(type)));

    /// <summary>Removes all serializers from the collection.</summary>
    public void Clear() => Serializers.Clear();

    /// <summary>
    /// Calls <see cref="BsonSerializer.RegisterSerializer(Type, IBsonSerializer)"/>
    /// for every serializer in the collection.
    /// </summary>
    public void Register()
    {
        foreach (var serializer in this)
        {
            BsonSerializer.RegisterSerializer(serializer.ValueType, serializer);
        }
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerator<IBsonSerializer> GetEnumerator() => Serializers.Values.GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Returns true if a name based convention is supported.</summary>
    [Pure]
    private static bool TypeIsSupported(IBsonSerializer converter)
        => (bool)converter
        .GetType()
        .GetProperty(nameof(QowaivBsonSerializer<object>.TypeIsSupported), NonPublicInstance)!
        .GetValue(converter, Array.Empty<object>())!;

    /// <summary>Creates an instance of <see cref="QowaivBsonSerializer{TSvo}"/> based on the specified type.</summary>
    [Pure]
    private static IBsonSerializer CreateConverter(Type type)
    {
        var converterType = typeof(QowaivBsonSerializer<>).MakeGenericType(type);
        return (IBsonSerializer)Activator.CreateInstance(converterType)!;
    }

    private const BindingFlags NonPublicInstance = (BindingFlags)36;
}
