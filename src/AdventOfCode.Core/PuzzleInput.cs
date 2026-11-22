namespace AdventOfCode.Core;

/// <summary>
/// Puzzle input with normalized line endings and without trailing empty lines.
/// </summary>
public sealed class PuzzleInput
{
    private readonly Lazy<string[]> _lines;
    private readonly Lazy<string[]> _blocks;

    public PuzzleInput(string raw)
    {
        Raw = raw.ReplaceLineEndings("\n").TrimEnd('\n');
        _lines = new(() => Raw.Length == 0 ? [] : Raw.Split('\n'));
        _blocks = new(() => Raw.Length == 0 ? [] : Raw.Split("\n\n"));
    }

    /// <summary>The whole input as a single string.</summary>
    public string Raw { get; }

    /// <summary>The input split into lines.</summary>
    public IReadOnlyList<string> Lines => _lines.Value;

    /// <summary>Blocks separated by empty lines (a common AoC format).</summary>
    public IReadOnlyList<string> Blocks => _blocks.Value;

    public override string ToString() => Raw;
}
