using AdventOfCode.Core;
using AdventOfCode.Solutions;

var registry = SolutionRegistry.FromAssemblies(typeof(SolutionsAssembly).Assembly);
Console.WriteLine("Available solutions:");
foreach (SolutionDescriptor? solution in registry.All)
    Console.WriteLine($"{solution.Id} {solution.Title}");
