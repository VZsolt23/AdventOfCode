# Advent of Code (.NET)

A small .NET 10 runner for [Advent of Code](https://adventofcode.com/) solutions.
Each puzzle is a single class; the CLI finds it automatically, loads its input from a local file,
runs both parts and prints the answers with timings.

> Inputs are **never downloaded**. You copy them from the website into local files yourself.

---

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Quick start

```powershell
dotnet build
dotnet test
dotnet run --project src/AdventOfCode.Cli -- list
```

All examples below use `aoc` as shorthand for `dotnet run --project src/AdventOfCode.Cli --`.

---

## Daily workflow

**1. Create the solution skeleton**

```powershell
dotnet run --project src/AdventOfCode.Cli -- new 2015 2 "I Was Told There Would Be No Math"
```

This creates three files:

| File | Purpose |
| --- | --- |
| `src/AdventOfCode.Solutions/Y2015/Day02.cs` | your solution class |
| `inputs/2015/day02.txt` | empty – paste your personal puzzle input here |
| `inputs/2015/day02.example.txt` | empty – paste the example from the puzzle text here |

**2. Paste the inputs** into the two empty files.

**3. Solve against the example first**

```powershell
dotnet run --project src/AdventOfCode.Cli -- 2015 2 --example
```

**4. Run with the real input**

```powershell
dotnet run --project src/AdventOfCode.Cli -- 2015 2
```

Output looks like this:

```
★ 2015 · Day 02 – I Was Told There Would Be No Math
  Part 1: 1586300  (0.42 ms)
  Part 2: not solved yet
```

A part that still throws `NotImplementedException` is shown as *not solved yet* rather than as an error.

---

## Commands

| Command | What it does |
| --- | --- |
| `aoc` | run the latest solution (highest year/day) |
| `aoc <year> <day>` | run one day, e.g. `aoc 2025 4` |
| `aoc <year>` | run every solution of that year |
| `aoc list [year]` | list the registered solutions |
| `aoc new <year> <day> [title]` | create a new solution skeleton and empty input files |

### Options

| Option | What it does |
| --- | --- |
| `-p`, `--part <1\|2>` | run only one part |
| `-e`, `--example` | use `dayNN.example.txt` instead of the real input |
| `-h`, `--help` | show help |

### Environment variables

| Variable | What it does |
| --- | --- |
| `AOC_INPUT_DIR` | override the location of the `inputs` folder |

---

## Input files

```
inputs/
└── 2015/
    ├── day01.txt            ← real input
    └── day01.example.txt    ← example input
```

- Day numbers are always two digits: `day01.txt`, not `day1.txt`.
- Line endings are normalized and trailing empty lines are removed.
- A missing or empty file stops the run with a message that shows the expected path.
- If the puzzle is not unlocked yet (midnight US Eastern time), the message says so.
- The `inputs/` folder is git-ignored: the AoC author asks that personal inputs are not published.

---

## Writing a solution

```csharp
namespace AdventOfCode.Solutions.Y2015;

[Puzzle(2015, 1, "Not Quite Lisp")]
public sealed class Day01 : ISolution
{
    public object? Part1(PuzzleInput input) =>
        input.Raw.Sum(c => c == '(' ? 1 : -1);

    public object? Part2(PuzzleInput input)
    {
        throw new NotImplementedException();
    }
}
```

- The `[Puzzle(year, day, title)]` attribute registers the class; nothing else needs to be wired up.
- Answers are `object?`, so returning `int`, `long` or `string` all work.
- A new instance is created for every run, so you can cache parsed data in fields and share it between the parts.

### `PuzzleInput`

| Member | Contents |
| --- | --- |
| `Raw` | the whole input as one string |
| `Lines` | the input split into lines |
| `Blocks` | groups separated by empty lines |

---

## Tests

```powershell
dotnet test
```

Example tests for a day go into `tests/AdventOfCode.Tests/Y<year>/DayNNTests.cs`.
`AssertAnswer.Equal` compares answers as strings, so the numeric type does not matter:

```csharp
[Theory]
[InlineData("(())", 0)]
[InlineData(")))", -3)]
public void Part1_examples(string input, int expected) =>
    AssertAnswer.Equal(expected, new Day01().Part1(new PuzzleInput(input)));
```

---

## Running from an IDE

`src/AdventOfCode.Cli/Properties/launchSettings.json` contains ready-made profiles
for Visual Studio and Rider: **Latest**, **Latest (example)** and **List**.
Add your own profile with `commandLineArgs` such as `"2015 2 --part 1"`.

---

## Project structure

| Project | Role |
| --- | --- |
| `src/AdventOfCode.Core` | shared infrastructure: input loading, solution registry, runner |
| `src/AdventOfCode.Solutions` | the puzzle solutions, one folder per year (`Y2015`, `Y2025`, …) |
| `src/AdventOfCode.Cli` | the `aoc` command-line app |
| `tests/AdventOfCode.Tests` | xUnit tests |
