using System;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.Internals;

/// <summary>Internal helper class for analyzing <see cref="Type"/>s.</summary>
internal static class TypeHelper
{
    /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
    /// <param name="objectType">
    /// The type to test for.
    /// </param>
    public static Type NotNullable(Type objectType)
        => objectType is { } ? Nullable.GetUnderlyingType(objectType) ?? objectType : null;

    /// <summary>Gets the candidate types.</summary>
    /// <param name="types">
    /// The types to select from.
    /// </param>
    public static IEnumerable<Type> GetCandidateTypes(IEnumerable<Type> types)
        => types.Where(IsSupported)
        .Select(type => IsIdBehavior(type) ? CreateIdType(type) : type);

    /// <summary>Returns true if the type implements <see cref="BehaviorType"/> and can be initiated.</summary>
    /// <param name="type">
    /// The type to test for.
    /// </param>
    public static bool IsIdBehavior(Type type)
        => !type.IsAbstract
        && type.GetInterfaces().Any(i => i.FullName == BehaviorType)
        && type.GetConstructors().Any(ctor => !ctor.GetParameters().Any());

    private static Type CreateIdType(Type behavior)
    {
        var type = Type.GetType(IdType);
        return type?.MakeGenericType(behavior);
    }

    private static bool IsSupported(Type type)
        => !type.IsAbstract
        && !type.IsGenericType
        && !type.ContainsGenericParameters
        && !type.IsGenericTypeDefinition;

    private const string IdType = "Qowaiv.Identifiers.Id`1, Qowaiv";
    private const string BehaviorType = "Qowaiv.Identifiers.IIdentifierBehavior";
}
