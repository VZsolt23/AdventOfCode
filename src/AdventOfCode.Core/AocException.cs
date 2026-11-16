namespace AdventOfCode.Core;

/// <summary>Felhasználónak szóló, várt hiba (hiányzó input, nem létező nap stb.).</summary>
public sealed class AocException(string message) : Exception(message);
