using System.Collections.Immutable;
using System.Reflection;

public static class InstalledModules
{
    private static ImmutableDictionary<ModuleIdentifier, IModule> modules =
        ImmutableDictionary<ModuleIdentifier, IModule>.Empty;
    
    private static ImmutableDictionary<ModuleIdentifier, IModule> GetModules()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .OfType<IModule>()
            .ToImmutableDictionary(m => m.Identifier, m => m);
    }

    public static void Load()
    {
        modules = GetModules();
    }
    
    public static ModuleInstance Get(ModuleIdentifier identifier)
    {
        var module = modules.GetValueOrDefault(identifier) ?? throw new ModuleNotFoundException();
        return ModuleInstance.New(module);
    }
    
    public static IEnumerable<ModuleIdentifier> Get(IEnumerable<ModuleTag> tags)
    {
        var tagHashSet = tags.ToImmutableHashSet();
        return modules.Values
            .Where(m => tagHashSet.Overlaps(m.Metadata.Tags))
            .Select(m => m.Identifier);
    }
    
    public static IEnumerable<ModuleIdentifier> Get(IEnumerable<ModuleCollection> collections)
    {
        var collectionHashSet = collections.ToImmutableHashSet();
        return modules.Values
            .Where(m => collectionHashSet.Contains(m.Metadata.Collection))
            .Select(m => m.Identifier);
    }
}