namespace AdventOfCode.Core;

/// <summary>
/// Egy napi feladvány megoldása. A válasz típusa szándékosan object,
/// mert az AoC válaszai lehetnek int, long, string stb.
/// Minden futtatáshoz új példány készül, így mezőkben nyugodtan cache-elhető
/// a közös feldolgozás a két rész között.
/// </summary>
public interface ISolution
{
    public object? Part1(PuzzleInput input);

    /// <summary>Alapértelmezésben még nincs kész – a runner ezt külön jelzi.</summary>
    public object? Part2(PuzzleInput input) => throw new NotImplementedException();
}
