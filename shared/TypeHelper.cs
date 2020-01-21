using System;

namespace Qowaiv.Internals
{
    /// <summary>Internal helper class for analyzing <see cref="Type"/>s.</summary>
    internal static class TypeHelper
    {
        /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static Type GetNotNullableType(Type objectType)
        {
            if (objectType is null)
            {
                return null;
            }

            if (IsNullable(objectType))
            {
                return objectType.GetGenericArguments()[0];
            }

            return objectType;
        }

        /// <summary>Returns true if the object is null-able, otherwise false.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsNullable(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
