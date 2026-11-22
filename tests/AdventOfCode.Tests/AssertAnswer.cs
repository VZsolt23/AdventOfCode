namespace AdventOfCode.Tests;

/// <summary>Compares as strings, so it doesn't matter whether a solution returns an int, long or string.</summary>
internal static class AssertAnswer
{
    public static void Equal(object expected, object? actual) =>
        Assert.Equal(expected.ToString(), actual?.ToString());
}
