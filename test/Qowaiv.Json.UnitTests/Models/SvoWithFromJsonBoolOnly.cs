namespace Qowaiv.Json.UnitTests.Models;

internal struct SvoWithFromJsonBoolOnly
{
    public bool ToJson() => true;

    public static SvoWithFromJsonBoolOnly FromJson(bool _) => default;
}
