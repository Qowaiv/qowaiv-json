namespace System.Linq;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage/* Syntactic sugar for Array.*. */]
internal static class QowaivJsonArrayExtensions
{
    /// <inheritdoc cref="Array.Exists{T}(T[], Predicate{T})" />
    [Pure]
    public static bool Exists<T>(this T[] array, Predicate<T> match) => Array.Exists(array, match);

    /// <inheritdoc cref="Array.Find{T}(T[], Predicate{T})" />
    [Pure]
    public static T? Find<T>(this T[] array, Predicate<T> match) => Array.Find(array, match);
}
