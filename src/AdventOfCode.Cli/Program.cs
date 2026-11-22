using System.Text;
using AdventOfCode.Cli;
using AdventOfCode.Core;
using AdventOfCode.Solutions;

Console.OutputEncoding = Encoding.UTF8;

Command command;
try
{
    command = CommandLineParser.Parse(args);
}
catch (ArgumentException ex)
{
    ConsoleOutput.Error(ex.Message);
    ConsoleOutput.Help();
    return 2;
}

SolutionRegistry registry;
try
{
    registry = SolutionRegistry.FromAssemblies(typeof(SolutionsAssembly).Assembly);
}
catch (InvalidOperationException ex)
{
    ConsoleOutput.Error($"Invalid solution configuration: {ex.Message}");
    return 1;
}

var inputs = new InputProvider(RepositoryPaths.GetInputRoot());

var app = new App(registry, new SolutionRunner(inputs), inputs);
return await app.ExecuteAsync(command);
