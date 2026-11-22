namespace AdventOfCode.Core;

public sealed record RunOptions(int? Part = null, bool UseExample = false);

public enum PartStatus { Solved, NotImplemented, Failed }

public sealed record PartResult(int Part, PartStatus Status, object? Answer, TimeSpan Elapsed, Exception? Error = null);

public sealed record RunResult(SolutionDescriptor Solution, bool UseExample, IReadOnlyList<PartResult> Parts);

/// <summary>Runs the parts of a solution, times them, and reports failures or missing parts as results instead of throwing.</summary>
public sealed class SolutionRunner(InputProvider inputs)
{
    public async Task<RunResult> RunAsync(SolutionDescriptor descriptor, RunOptions options, CancellationToken ct)
    {
        PuzzleInput? input = await inputs.GetAsync(descriptor.Id, options.UseExample, ct);
        ISolution? solution = descriptor.CreateInstance();
        var results = new List<PartResult>();

        if (options.Part is null or 1)
            results.Add(RunPart(1, () => solution.Part1(input)));

        if (options.Part is null or 2)
            results.Add(RunPart(2, () => solution.Part2(input)));

        return new RunResult(descriptor, options.UseExample, results);
    }

    private static PartResult RunPart(int part, Func<object?> action)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var answer = action();
            stopwatch.Stop();
            return new PartResult(part, PartStatus.Solved, answer, stopwatch.Elapsed);
        }
        catch (NotImplementedException)
        {
            return new PartResult(part, PartStatus.NotImplemented, null, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            return new PartResult(part, PartStatus.Failed, null, stopwatch.Elapsed, ex);
        }
    }
}
