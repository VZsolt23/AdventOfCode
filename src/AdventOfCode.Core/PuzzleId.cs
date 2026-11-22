namespace AdventOfCode.Core;

public readonly record struct PuzzleId(int Year, int Day) : IComparable<PuzzleId>
{
    public const int FirstYear = 2015;

    /// <summary>2015–2024: 25 days; from 2025 onwards: 12 days.</summary>
    public static int DaysInYear(int year) => year >= 2025 ? 12 : 25;

    public static bool IsValid(int year, int day) =>
        year >= FirstYear && day >= 1 && day <= DaysInYear(year);

    /// <summary>Puzzles unlock at midnight US Eastern time (UTC-5).</summary>
    public DateTimeOffset UnlockTime => new(Year, 12, Day, 0, 0, 0, TimeSpan.FromHours(-5));

    public int CompareTo(PuzzleId other) =>
        Year != other.Year ? Year.CompareTo(other.Year) : Day.CompareTo(other.Day);

    public override string ToString() => $"{Year}/{Day:00}";
}
