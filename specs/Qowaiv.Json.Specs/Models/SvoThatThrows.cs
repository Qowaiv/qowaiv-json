namespace Specs.Models;

internal struct SvoThatThrows
{
    public static SvoThatThrows FromJson(string _) => throw new FormatException("Value in the wrong format.");
}
