#nullable enable

namespace Qowaiv.Internals;

/// <summary>Internal helper class for analyzing <see cref="Type"/>s.</summary>
internal static class TypeHelper
{
    /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    [Pure]
    public static Type? NotNullable(Type? objectType)
        => objectType is { } ? Nullable.GetUnderlyingType(objectType) ?? objectType : null;

    /// <summary>Gets the candidate types.</summary>
    /// <param name="types">
    /// The types to select from.
    /// </param>
    [Pure]
    public static IEnumerable<Type> GetCandidateTypes(IEnumerable<Type> types)
    {
        return types.Where(IsSupported).Select(Transform);

        static bool IsSupported(Type type)
            => !type.IsAbstract
            && !type.IsGenericType
            && !type.ContainsGenericParameters
            && !type.IsGenericTypeDefinition;

        static Type Transform(Type type)
            => IdBehavior(type)
            ?? SvoBehavior(type)
            ?? type;
    }

    [Pure]
    public static Type? IdBehavior(Type type)
        => Id.Is(type)
        ? Type.GetType(Id.Generic)?.MakeGenericType(type)
        : null;

    [Pure]
    public static Type? SvoBehavior(Type type)
        => Svo.Is(type)
        ? Type.GetType(Svo.Generic)?.MakeGenericType(type)
        : null;

    private static class Id
    {
        [Pure]
        public static bool Is(Type type)
            => type.GetInterfaces().Any(i => i.FullName == Behavior)
            && type.GetConstructors().Any(ctor => !ctor.GetParameters().Any());

        public const string Generic = "Qowaiv.Identifiers.Id`1, Qowaiv";
        public const string Behavior = "Qowaiv.Identifiers.IIdentifierBehavior";
    }

    private static class Svo
    {
        [Pure]
        public static bool Is(Type type)
           => type.BaseTypes().Any(i => i.FullName == Behavior)
           && type.GetConstructors().Any(ctor => !ctor.GetParameters().Any());

        public const string Generic = "Qowaiv.Customization.Svo`1, Qowaiv";
        public const string Behavior = "Qowaiv.Customization.SvoBehavior";
    }

    [Pure]
    private static IEnumerable<Type> BaseTypes(this Type type)
    {
        var current = type.BaseType;
        while (current is { })
        {
            yield return current;
            current = current.BaseType;
        }
    }
}
