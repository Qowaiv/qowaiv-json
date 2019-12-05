#pragma warning disable S3898 // Value types should implement "IEquatable<T>"
using System;

namespace Qowaiv.Json.UnitTests.Models
{
    internal struct SvoThatThrows
    {
        public static SvoThatThrows FromJson(string str) => throw new FormatException("Value in the wrong format.");
    }
}
