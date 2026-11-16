namespace AdventOfCode.Core;

/// <summary>
/// Megjelöli, hogy az osztály melyik év melyik napjának megoldása.
/// Az érvényességet a <see cref="SolutionRegistry"/> ellenőrzi induláskor.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PuzzleAttribute(int year, int day, string? title = null) : Attribute
{
    public int Year { get; } = year;
    public int Day { get; } = day;
    public string? Title { get; } = title;
}
