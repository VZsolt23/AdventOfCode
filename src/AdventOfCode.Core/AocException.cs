namespace AdventOfCode.Core;

/// <summary>An expected, user-facing error (missing input, unknown day, etc.).</summary>
public sealed class AocException(string message) : Exception(message);
