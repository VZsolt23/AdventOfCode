using AdventOfCode.Core;

namespace AdventOfCode.Cli;

/// <summary>Scaffolds a new day: Solutions/Y{year}/DayNN.cs plus empty real and example input files.</summary>
internal static class Scaffolder
{
    public static IReadOnlyList<string> Create(string repoRoot, InputProvider inputs, PuzzleId id, string? title)
    {
        var created = new List<string>();

        var classFile = Path.Combine(repoRoot, "src", "AdventOfCode.Solutions", $"Y{id.Year}", $"Day{id.Day:00}.cs");
        if (File.Exists(classFile))
            throw new AocException($"The file already exists: {classFile}");

        Directory.CreateDirectory(Path.GetDirectoryName(classFile)!);
        File.WriteAllText(classFile, Template(id, title));
        created.Add(classFile);

        // Empty input files, ready for the puzzle input to be pasted in.
        foreach (var example in new[] { false, true })
        {
            var inputPath = inputs.GetPath(id, example);
            if (File.Exists(inputPath))
                continue;

            Directory.CreateDirectory(Path.GetDirectoryName(inputPath)!);
            File.WriteAllText(inputPath, "");
            created.Add(inputPath);
        }

        return created;
    }

    private static string Template(PuzzleId id, string? title)
    {
        var titleArgument = title is null
            ? ""
            : $", \"{title.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

        return $$"""
            namespace AdventOfCode.Solutions.Y{{id.Year}};

            [Puzzle({{id.Year}}, {{id.Day}}{{titleArgument}})]
            public sealed class Day{{id.Day:00}} : ISolution
            {
                public object? Part1(PuzzleInput input)
                {
                    throw new NotImplementedException();
                }

                public object? Part2(PuzzleInput input)
                {
                    throw new NotImplementedException();
                }
            }

            """;
    }
}
