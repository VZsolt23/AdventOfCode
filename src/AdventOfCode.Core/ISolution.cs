namespace AdventOfCode.Core;

/// <summary>
/// The solution of a daily puzzle. The answer type is object on purpose,
/// because AoC answers can be int, long, string, etc.
/// A new instance is created for every run, so parsing shared by both parts
/// can safely be cached in fields.
/// </summary>
public interface ISolution
{
    public object? Part1(PuzzleInput input);

    /// <summary>Not implemented by default – the runner reports this separately.</summary>
    public object? Part2(PuzzleInput input) => throw new NotImplementedException();
}
