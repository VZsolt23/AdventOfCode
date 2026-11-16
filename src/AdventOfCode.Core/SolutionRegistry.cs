using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Core;

/// <summary>
/// Reflectionnel összegyűjti a [Puzzle] attribútummal jelölt megoldásokat.
/// A hibás konfigurációt (duplikáció, rossz nap, hiányzó interfész) azonnal jelzi.
/// </summary>
public sealed class SolutionRegistry
{
    private readonly SortedDictionary<PuzzleId, SolutionDescriptor> _solutions;

    private SolutionRegistry(SortedDictionary<PuzzleId, SolutionDescriptor> solutions) =>
        _solutions = solutions;

    public static SolutionRegistry FromAssemblies(params Assembly[] assemblies)
    {
        var solutions = new SortedDictionary<PuzzleId, SolutionDescriptor>();

        foreach (Type? type in assemblies.SelectMany(GetLoadableTypes))
        {
            PuzzleAttribute? attribute = type.GetCustomAttribute<PuzzleAttribute>();
            if (attribute is null)
                continue;

            if (!typeof(ISolution).IsAssignableFrom(type))
                throw new InvalidOperationException(
                    $"{type.FullName}: van [Puzzle] attribútuma, de nem implementálja az ISolution interfészt.");

            if (type.IsAbstract || type.GetConstructor(Type.EmptyTypes) is null)
                throw new InvalidOperationException(
                    "{type.FullName}: nem absztrakt, paraméter nélküli konstruktorral rendelkező osztály kell.");

            if (!PuzzleId.IsValid(attribute.Year, attribute.Day))
                throw new InvalidOperationException(
                    $"{type.FullName}: érvénytelen év/nap: {attribute.Year}/{attribute.Day}.");

            var id = new PuzzleId(attribute.Year, attribute.Day);
            if (solutions.TryGetValue(id, out SolutionDescriptor? existing))
                throw new InvalidOperationException(
                    $"Duplikált megoldás ({id}): {existing.Type.FullName} és {type.FullName}.");

            solutions.Add(id, new SolutionDescriptor(id, attribute.Title, type));
        }

        return new SolutionRegistry(solutions);
    }

    public IEnumerable<SolutionDescriptor> All => _solutions.Values;

    public SolutionDescriptor? Latest => _solutions.Count == 0 ? null : _solutions.Values.Last();

    public IEnumerable<SolutionDescriptor> ForYear(int year) =>
        _solutions.Values.Where(s => s.Id.Year == year);

    public bool TryGet(PuzzleId id, [MaybeNullWhen(false)] out SolutionDescriptor descriptor) =>
        _solutions.TryGetValue(id, out descriptor);

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.OfType<Type>();
        }
    }
}
