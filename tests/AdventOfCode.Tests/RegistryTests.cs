using AdventOfCode.Solutions;

namespace AdventOfCode.Tests;

public class RegistryTests
{
    [Fact]
    public void All_solutions_are_registered_without_conflicts()
    {
        // Throws on duplicate or incorrectly annotated solutions.
        var registry = SolutionRegistry.FromAssemblies(typeof(SolutionsAssembly).Assembly);
        Assert.All(registry.All, s => Assert.True(PuzzleId.IsValid(s.Id.Year, s.Id.Day)));
    }
}
