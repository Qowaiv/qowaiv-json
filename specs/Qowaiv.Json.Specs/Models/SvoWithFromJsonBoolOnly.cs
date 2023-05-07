namespace Specs.Models;

internal struct SvoWithFromJsonBoolOnly
{
    public bool ToJson() => GetType() is { };

    public static SvoWithFromJsonBoolOnly FromJson(bool _) => default;
}
