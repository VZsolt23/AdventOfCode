namespace AdventOfCode.Core;

/// <summary>
/// Marks the class as the solution for a given year and day.
/// Validity is checked by <see cref="SolutionRegistry"/> at startup.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PuzzleAttribute(int year, int day, string? title = null) : Attribute
{
    public int Year { get; } = year;
    public int Day { get; } = day;
    public string? Title { get; } = title;
}
