#pragma warning disable S3898 // Value types should implement "IEquatable<T>"

namespace Qowaiv.Json.UnitTests.Models
{
    internal struct SvoWithFromJsonBoolOnly
    {
        public bool ToJson() => true;
        public static SvoWithFromJsonBoolOnly FromJson(bool json) => default;
    }
}
