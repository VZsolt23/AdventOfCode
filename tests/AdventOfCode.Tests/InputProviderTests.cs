namespace AdventOfCode.Tests;

public sealed class InputProviderTests : IDisposable
{
    private readonly string _root = Directory.CreateTempSubdirectory("aoc-tests-").FullName;

    public void Dispose() => Directory.Delete(_root, recursive: true);

    [Fact]
    public async Task Reads_real_and_example_input_from_local_files()
    {
        var provider = new InputProvider(_root);
        var id = new PuzzleId(2015, 1);
        Directory.CreateDirectory(Path.Combine(_root, "2015"));
        await File.WriteAllTextAsync(Path.Combine(_root, "2015", "day01.txt"), "real\n");
        await File.WriteAllTextAsync(Path.Combine(_root, "2015", "day01.example.txt"), "example\n");

        Assert.Equal("real", (await provider.GetAsync(id, example: false)).Raw);
        Assert.Equal("example", (await provider.GetAsync(id, example: true)).Raw);
    }

    [Fact]
    public async Task Missing_file_throws_AocException_with_path()
    {
        var provider = new InputProvider(_root);
        var ex = await Assert.ThrowsAsync<AocException>(() => provider.GetAsync(new PuzzleId(2015, 2), example: false));
        Assert.Contains("Missing input", ex.Message);
        Assert.Contains("day02.txt", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\r\n\n  ")]
    public async Task Empty_file_throws_AocException(string content)
    {
        var provider = new InputProvider(_root);
        Directory.CreateDirectory(Path.Combine(_root, "2015"));
        await File.WriteAllTextAsync(Path.Combine(_root, "2015", "day03.example.txt"), content);

        var ex = await Assert.ThrowsAsync<AocException>(() => provider.GetAsync(new PuzzleId(2015, 3), example: true));
        Assert.Contains("Empty", ex.Message);
    }

    [Fact]
    public async Task Missing_input_before_unlock_says_not_yet_open()
    {
        var id = new PuzzleId(2026, 1);
        var provider = new InputProvider(_root, new FixedTime(id.UnlockTime.AddMinutes(-1)));

        var ex = await Assert.ThrowsAsync<AocException>(() => provider.GetAsync(id, example: false));
        Assert.Contains("not unlocked yet", ex.Message);
    }

    [Fact]
    public async Task Missing_input_after_unlock_asks_for_file()
    {
        var id = new PuzzleId(2026, 1);
        var provider = new InputProvider(_root, new FixedTime(id.UnlockTime.AddMinutes(1)));

        var ex = await Assert.ThrowsAsync<AocException>(() => provider.GetAsync(id, example: false));
        Assert.Contains("Missing input", ex.Message);
    }

    private sealed class FixedTime(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now.ToUniversalTime();
    }
}
