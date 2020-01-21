using MongoDB.Bson.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Qowaiv.Bson.MongoDB
{
    /// <summary>Static helper class to register <see cref="QowaivBsonConverter{TSvo}"/> for specific types.</summary>
    public static class QowaivBsonConverter
    {
        /// <summary>Registers all supported types of the assembly.</summary>
        /// <param name="assemblyType">
        /// A type that is part of the assembly to register a <see cref="QowaivBsonConverter{TSvo}"/> for.
        /// </param>
        public static void RegisterAssembly(Type assemblyType)
        {
            Guard.NotNull(assemblyType, nameof(assemblyType));

            foreach (var type in assemblyType.Assembly.GetTypes().Where(NonGeneric))
            {
                var converter = CreateConverter(type);
                if (TypeIsSupported(converter))
                {
                    BsonSerializer.RegisterSerializer(type, converter);
                }
            }
        }

        /// <summary>Registers all types.</summary>
        /// <param name="types">
        /// The types to register a <see cref="QowaivBsonConverter{TSvo}"/> for.
        /// </param>
        public static void RegisterTypes(params Type[] types)
        {
            Guard.HasAny(types, nameof(types));

            foreach (var type in types)
            {
                RegisterType(type);
            }
        }

        /// <summary>Registers a type.</summary>
        /// <param name="type">
        /// The type to register a <see cref="QowaivBsonConverter{TSvo}"/> for.
        /// </param>
        public static void RegisterType(Type type)
        {
            Guard.NotNull(type, nameof(type));
            var converter = CreateConverter(type);
            BsonSerializer.RegisterSerializer(type, GuardType(converter));
        }

        /// <summary>Registers a type.</summary>
        /// <typeparam name="TSvo">
        /// The type to register a <see cref="QowaivBsonConverter{TSvo}"/> for.
        /// </typeparam>
        public static void RegisterType<TSvo>()
        {
            var converter = new QowaivBsonConverter<TSvo>();
            GuardType(converter);
            BsonSerializer.RegisterSerializer(converter);
        }

        /// <summary>Guard that the converter actually supports conversion based on conventions.</summary>
        private static IBsonSerializer GuardType(IBsonSerializer converter)
        {
            if (!TypeIsSupported(converter))
            {
                throw new NotSupportedException();
            }

            return converter;
        }

        /// <summary>Returns true if a name based convention is supported.</summary>
        private static bool TypeIsSupported(IBsonSerializer converter)
        {
            var info = converter.GetType().GetProperty(nameof(QowaivBsonConverter<object>.TypeIsSupported), BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)info.GetValue(converter, Array.Empty<object>());
        }

        /// <summary>Creates an instance of <see cref="QowaivBsonConverter{TSvo}"/> based on the specified type.</summary>
        private static IBsonSerializer CreateConverter(Type type)
        {
            var converterType = typeof(QowaivBsonConverter<>).MakeGenericType(type);
            var converter = (IBsonSerializer)Activator.CreateInstance(converterType);
            return converter;
        }

        /// <summary><see cref="BsonSerializer"/> only support non-generic types.</summary>
        private static bool NonGeneric(Type type)
        {
            return !type.IsGenericType && !type.ContainsGenericParameters;
        }
    }
}
