namespace AdventOfCode.Core;

/// <summary>
/// Megkeresi a repo gyökerét (Directory.Build.props alapján), így az inputok
/// helye nem függ attól, honnan indítod az alkalmazást (VS, Rider, dotnet run).
/// </summary>
public static class RepositoryPath
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
