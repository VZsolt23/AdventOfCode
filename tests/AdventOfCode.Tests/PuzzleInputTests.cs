namespace AdventOfCode.Tests;

public class PuzzleInputTests
{
    [Fact]
    public void Normalizes_line_endings_and_trailing_newlines()
    {
        var input = new PuzzleInput("a\r\nb\r\n\r\nc\r\n\r\n");
        Assert.Equal("a\nb\n\nc", input.Raw);
        Assert.Equal(["a", "b", "", "c"], input.Lines);
        Assert.Equal(["a\nb", "c"], input.Blocks);
    }

    [Fact]
    public void Empty_input_has_no_lines()
    {
        var input = new PuzzleInput("\n");
        Assert.Empty(input.Lines);
        Assert.Empty(input.Blocks);
    }
}
