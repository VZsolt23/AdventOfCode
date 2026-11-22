using AdventOfCode.Core;

namespace AdventOfCode.Cli;

internal static class CommandLineParser
{
    public static Command Parse(IReadOnlyList<string> args)
    {
        var positional = new List<string>();
        int? part = null;
        var example = false;

        for (var i = 0; i < args.Count; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-h" or "--help" or "help":
                    return new HelpCommand();

                case "-e" or "--example":
                    example = true;
                    break;

                case "-p" or "--part":
                    if (i + 1 >= args.Count || !int.TryParse(args[++i], out var p) || p is not (1 or 2))
                        throw new ArgumentException("The value of --part must be 1 or 2.");
                    part = p;
                    break;

                default:
                    if (arg.StartsWith('-'))
                        throw new ArgumentException($"Unknown option: {arg}");
                    positional.Add(arg);
                    break;
            }
        }

        var options = new RunOptions(part, example);
        if (positional.Count == 0)
            return new RunCommand(null, null, options);

        switch (positional[0].ToLowerInvariant())
        {
            case "latest":
                ExpectAtMost(positional, 1);
                return new RunCommand(null, null, options);

            case "list" or "ls":
                ExpectAtMost(positional, 2);
                return new ListCommand(positional.Count > 1 ? ParseYear(positional[1]) : null);

            case "new":
                if (positional.Count < 3)
                    throw new ArgumentException("Usage: new <year> <day> [title]");
                var newYear = ParseYear(positional[1]);
                var newDay = ParseDay(newYear, positional[2]);
                var title = positional.Count > 3 ? string.Join(' ', positional.Skip(3)) : null;
                return new NewCommand(new PuzzleId(newYear, newDay), title);

            default:
                ExpectAtMost(positional, 2);
                var year = ParseYear(positional[0]);
                int? day = positional.Count > 1 ? ParseDay(year, positional[1]) : null;
                return new RunCommand(year, day, options);
        }
    }

    private static void ExpectAtMost(List<string> positional, int max)
    {
        if (positional.Count > max)
            throw new ArgumentException($"Too many arguments: {string.Join(' ', positional.Skip(max))}");
    }

    private static int ParseYear(string value)
    {
        var lastYear = DateTime.Now.Year;
        if (!int.TryParse(value, out var year) || year < PuzzleId.FirstYear || year > lastYear)
            throw new ArgumentException($"Invalid year: '{value}' (valid: {PuzzleId.FirstYear}–{lastYear}).");
        return year;
    }

    private static int ParseDay(int year, string value)
    {
        var max = PuzzleId.DaysInYear(year);
        if (!int.TryParse(value, out var day) || day < 1 || day > max)
            throw new ArgumentException($"Invalid day: '{value}' ({year}: 1–{max}).");
        return day;
    }
}
