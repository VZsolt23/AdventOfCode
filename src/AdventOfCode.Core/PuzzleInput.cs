namespace AdventOfCode.Core;

/// <summary>
/// A feladvány bemenete egységesített sorvégekkel, a záró üres sorok nélkül.
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

    /// <summary>A teljes bemenet egy stringként.</summary>
    public string Raw { get; }

    /// <summary>A bemenet soronként.</summary>
    public IReadOnlyList<string> Lines => _lines.Value;

    /// <summary>Üres sorokkal elválasztott blokkok (gyakori AoC-formátum).</summary>
    public IReadOnlyList<string> Blocks => _blocks.Value;

    public override string ToString() => Raw;
}
