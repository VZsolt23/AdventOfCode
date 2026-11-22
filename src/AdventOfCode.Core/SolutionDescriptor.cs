namespace AdventOfCode.Core;

public sealed record SolutionDescriptor(PuzzleId Id, string? Title, Type Type)
{
    public ISolution CreateInstance() => (ISolution)Activator.CreateInstance(Type)!;
}
