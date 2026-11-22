namespace AdventOfCode.Core;

/// <summary>
/// Finds the repository root (by Directory.Build.props), so the input location
/// does not depend on where the app is started from (VS, Rider, dotnet run).
/// </summary>
public static class RepositoryPaths
{
    private const string RootMarker = "Directory.Build.props";

    public static string? FindRoot()
    {
        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
            {
                if (File.Exists(Path.Combine(dir.FullName, RootMarker)))
                    return dir.FullName;
            }
        }

        return null;
    }

    public static string GetInputRoot()
    {
        var fromEnvironment = Environment.GetEnvironmentVariable("AOC_INPUT_DIR");
        if (!string.IsNullOrWhiteSpace(fromEnvironment))
            return Path.GetFullPath(fromEnvironment);

        return Path.Combine(FindRoot() ?? Directory.GetCurrentDirectory(), "inputs");
    }
}
