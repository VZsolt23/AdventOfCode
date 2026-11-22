namespace AdventOfCode.Core;

/// <summary>
/// Loads puzzle inputs from the inputs folder:
/// inputs/{year}/dayNN.txt (real input) and inputs/{year}/dayNN.example.txt (example input).
/// There is no download: the files must be filled in by hand.
/// </summary>
public sealed class InputProvider(string inputRoot, TimeProvider? timeProvider = null)
{
    private readonly TimeProvider _time = timeProvider ?? TimeProvider.System;

    public string InputRoot { get; } = inputRoot;

    public string GetPath(PuzzleId id, bool example = false) =>
        Path.Combine(InputRoot, id.Year.ToString(), example ? $"day{id.Day:00}.example.txt" : $"day{id.Day:00}.txt");

    public async Task<PuzzleInput> GetAsync(PuzzleId id, bool example, CancellationToken ct = default)
    {
        var path = GetPath(id, example);
        var kind = example ? "example input" : "input";

        var raw = File.Exists(path) ? await File.ReadAllTextAsync(path, ct) : null;
        if (!string.IsNullOrWhiteSpace(raw))
            return new PuzzleInput(raw);

        if (_time.GetUtcNow() < id.UnlockTime)
            throw new AocException(
                $"Puzzle {id} is not unlocked yet (unlocks at {id.UnlockTime.ToLocalTime():yyyy-MM-dd HH:mm}).");

        var problem = raw is null ? $"Missing {kind}" : $"Empty {kind} file";
        throw new AocException($"{problem}: {path}\n  Paste the {kind} of {id} into this file.");
    }
}
