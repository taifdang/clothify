namespace Application.Common.Utilities;

public static class CartesianHelper
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };
        foreach (var sequence in sequences)
        {
            result = result.SelectMany(
                prefix => sequence,
                (prefix, item) => prefix.Append(item)
            );
        }
        return result;
    }
}
