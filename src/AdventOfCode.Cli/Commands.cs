using AdventOfCode.Core;

namespace AdventOfCode.Cli;

internal abstract record Command;

internal sealed record HelpCommand : Command;

internal sealed record ListCommand(int? Year) : Command;

/// <summary>Year == null: run the latest solution; Day == null: run every solution of the year.</summary>
internal sealed record RunCommand(int? Year, int? Day, RunOptions Options) : Command;

internal sealed record NewCommand(PuzzleId Id, string? Title) : Command;
