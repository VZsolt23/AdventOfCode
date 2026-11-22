using AdventOfCode.Core;

namespace AdventOfCode.Cli;

internal static class ConsoleOutput
{
    public static void Header(SolutionDescriptor solution, bool example)
    {
        Console.WriteLine();
        Write($"★ {solution.Id.Year} · Day {solution.Id.Day:00}", ConsoleColor.Green);
        if (solution.Title is not null)
            Write($" – {solution.Title}", ConsoleColor.White);
        if (example)
            Write("  [example input]", ConsoleColor.DarkYellow);
        Console.WriteLine();
    }

    public static void Print(RunResult result)
    {
        Header(result.Solution, result.UseExample);

        foreach (PartResult part in result.Parts)
        {
            Write($"  Part {part.Part}: ", ConsoleColor.Gray);
            switch (part.Status)
            {
                case PartStatus.Solved:
                    Write(part.Answer?.ToString() ?? "(null)", ConsoleColor.Yellow);
                    Write($"  ({FormatElapsed(part.Elapsed)})", ConsoleColor.DarkGray);
                    Console.WriteLine();
                    break;

                case PartStatus.NotImplemented:
                    Write("not solved yet", ConsoleColor.DarkGray);
                    Console.WriteLine();
                    break;

                case PartStatus.Failed:
                    Write($"ERROR – {part.Error!.GetType().Name}: {part.Error.Message}", ConsoleColor.Red);
                    Console.WriteLine();
                    Write(part.Error.StackTrace ?? "", ConsoleColor.DarkGray);
                    Console.WriteLine();
                    break;
            }
        }
    }

    public static void List(IEnumerable<SolutionDescriptor> solutions)
    {
        var any = false;
        foreach (IGrouping<int, SolutionDescriptor> year in solutions.GroupBy(s => s.Id.Year))
        {
            any = true;
            Write($"{year.Key}", ConsoleColor.Green);
            Console.WriteLine();
            foreach (SolutionDescriptor? solution in year)
            {
                Write($"  Day {solution.Id.Day:00}  {solution.Title}", ConsoleColor.Gray);
                Console.WriteLine();
            }
        }

        if (!any)
        {
            Write("No solutions registered.", ConsoleColor.DarkGray);
            Console.WriteLine();
        }
    }

    public static void Info(string message)
    {
        Write(message, ConsoleColor.Cyan);
        Console.WriteLine();
    }

    public static void Error(string message)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"  {message}");
        Console.ForegroundColor = previous;
    }

    public static void Help() => Console.WriteLine("""

        Usage:
          aoc                          run the latest solution (highest year/day)
          aoc <year> <day>             run one day, e.g. aoc 2025 4
          aoc <year>                   run every solution of the year
          aoc list [year]              list the registered solutions
          aoc new <year> <day> [title] create a new solution skeleton and empty input files

        Options:
          -p, --part <1|2>             run only the given part
          -e, --example                use the example input (inputs/<year>/dayNN.example.txt)
          -h, --help                   show this help

        Environment variables:
          AOC_INPUT_DIR                override the location of the inputs folder
        """);

    private static string FormatElapsed(TimeSpan elapsed) =>
        elapsed.TotalMilliseconds < 1 ? $"{elapsed.TotalMicroseconds:0} µs"
        : elapsed.TotalSeconds < 1 ? $"{elapsed.TotalMilliseconds:0.00} ms"
        : $"{elapsed.TotalSeconds:0.00} s";

    private static void Write(string text, ConsoleColor color)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = previous;
    }
}
