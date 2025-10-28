namespace Application.Common.Utilities;

public static class StringHelper
{
    private static readonly Random Random = new();
    public static int Generate(int min, int max) => Random.Next(min, max);
}
