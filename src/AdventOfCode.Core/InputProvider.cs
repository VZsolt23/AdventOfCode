namespace AdventOfCode.Core;

/// <summary>
/// Inputok betöltése az inputs mappából:
/// inputs/{év}/dayNN.txt (valódi input) és inputs/{év}/dayNN.example.txt (példa input).
/// </summary>
public sealed class InputProvider(string inputRoot)
{
    public string InputRoot { get; } = inputRoot;

    public string GetPath(PuzzleId id, bool example = false) =>
        Path.Combine(InputRoot, id.Year.ToString(), example ? $"day{id.Day:00}.example.txt" : $"day{id.Day:00}.txt");

    public async Task<PuzzleInput> GetAsync(PuzzleId id, bool example, CancellationToken ct = default)
    {
        var path = GetPath(id, example);
        if (!File.Exists(path))
        {
            var kind = example ? "példa input" : "input";
            throw new AocException($"Nincs {kind}: {path}\n  Másold be a(z) {id} {kind}át ebbe a fájlba.");
        }

        return new PuzzleInput(await File.ReadAllTextAsync(path, ct));
    }
}
