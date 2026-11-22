using System.Diagnostics;
using AdventOfCode.Core;

namespace AdventOfCode.Cli;

internal sealed class App(SolutionRegistry registry, SolutionRunner runner, InputProvider inputs)
{
    public async Task<int> ExecuteAsync(Command command)
    {
        try
        {
            return command switch
            {
                HelpCommand => ShowHelp(),
                ListCommand list => List(list.Year),
                NewCommand create => Scaffold(create),
                RunCommand run => await RunAsync(run),
                _ => throw new UnreachableException(),
            };
        }
        catch (AocException ex)
        {
            ConsoleOutput.Error(ex.Message);
            return 1;
        }
    }

    private static int ShowHelp()
    {
        ConsoleOutput.Help();
        return 0;
    }

    private int List(int? year)
    {
        ConsoleOutput.List(year is { } y ? registry.ForYear(y) : registry.All);
        return 0;
    }

    private async Task<int> RunAsync(RunCommand command)
    {
        var exitCode = 0;

        // A single missing input should not stop a whole-year run.
        foreach (SolutionDescriptor solution in ResolveTargets(command))
        {
            try
            {
                RunResult result = await runner.RunAsync(solution, command.Options, CancellationToken.None);
                ConsoleOutput.Print(result);
                if (result.Parts.Any(p => p.Status == PartStatus.Failed))
                    exitCode = 1;
            }
            catch (AocException ex)
            {
                ConsoleOutput.Header(solution, command.Options.UseExample);
                ConsoleOutput.Error(ex.Message);
                exitCode = 1;
            }
        }

        return exitCode;
    }

    private IReadOnlyList<SolutionDescriptor> ResolveTargets(RunCommand command)
    {
        if (command.Year is not { } year)
        {
            SolutionDescriptor latest = registry.Latest
                ?? throw new AocException("There are no solutions yet. Create one with: new <year> <day>");
            return [latest];
        }

        if (command.Day is not { } day)
        {
            var ofYear = registry.ForYear(year).ToList();
            if (ofYear.Count == 0)
                throw new AocException($"There are no solutions for {year}.");
            return ofYear;
        }

        var id = new PuzzleId(year, day);
        if (registry.TryGet(id, out SolutionDescriptor? descriptor))
            return [descriptor];

        var available = registry.ForYear(year).Select(s => s.Id.Day).ToList();
        var hint = available.Count > 0 ? $" Available days ({year}): {string.Join(", ", available)}." : "";
        throw new AocException($"There is no solution for {id}.{hint} Create one with: new {year} {day}");
    }

    private int Scaffold(NewCommand command)
    {
        if (registry.TryGet(command.Id, out SolutionDescriptor? existing))
            throw new AocException($"A solution for {command.Id} already exists ({existing.Type.FullName}).");

        var root = RepositoryPaths.FindRoot()
            ?? throw new AocException("Could not find the repository root (Directory.Build.props).");

        foreach (var file in Scaffolder.Create(root, inputs, command.Id, command.Title))
            ConsoleOutput.Info($"Created: {Path.GetRelativePath(root, file)}");

        return 0;
    }
}
