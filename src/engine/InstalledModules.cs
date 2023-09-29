using System.Collections.Immutable;
using System.Reflection;
using sdk;

public static class InstalledModules
{
    private static ImmutableDictionary<ModuleIdentifier, IModule> modules =
        ImmutableDictionary<ModuleIdentifier, IModule>.Empty;
    
    private static ImmutableDictionary<ModuleIdentifier, IUserInterface> uis =
        ImmutableDictionary<ModuleIdentifier, IUserInterface>.Empty;
    
    private static ImmutableDictionary<ModuleIdentifier, IModule> GetModules(Assembly assembly)
    {
        return GetInstancesImplementing<IModule>(assembly)
            .Where(m => !string.IsNullOrEmpty(m.Metadata?.Identifier))
            .ToImmutableDictionary(m => m.Metadata.Identifier, m => m);
    }
    
    private static ImmutableDictionary<ModuleIdentifier, IUserInterface> GetModuleUis(Assembly assembly)
    {
        return GetInstancesImplementing<IUserInterface>(assembly)
            .Where(m => m.GetType().IsAssignableTo(typeof(System.Windows.Controls.Control)))
            .Where(m => !string.IsNullOrEmpty(m.Identifier))
            .ToImmutableDictionary(m => m.Identifier, m => m);
    }
    
    private static IEnumerable<T> GetInstancesImplementing<T>(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(T)))
            .Where(t => t.IsPublic)
            .Select(Activator.CreateInstance)
            .OfType<T>();
    }

    public static void Load()
    {
        var assembly = Assembly.GetExecutingAssembly();
        modules = GetModules(assembly);
        uis = GetModuleUis(assembly);
    }
    
    public static ModuleInstance Get(ModuleIdentifier identifier)
    {
        var module = modules.GetValueOrDefault(identifier) ?? throw new ModuleNotFoundException();
        var ui = uis.GetValueOrDefault(identifier) ?? throw new ModuleNotFoundException();
        return new ModuleInstance(module.Clone(), (IUserInterface)Activator.CreateInstance(ui.GetType())!);
    }
    
    public static IEnumerable<ModuleIdentifier> GetByTags(IEnumerable<string> tags)
    {
        var tagHashSet = tags.ToImmutableHashSet();
        return modules.Values
            .Where(m => tagHashSet.Overlaps(m.Metadata.Tags))
            .Select(m => m.Metadata.Identifier);
    }
    
    public static IEnumerable<ModuleIdentifier> GetByCollections(IEnumerable<string> collections)
    {
        var collectionHashSet = collections.ToImmutableHashSet();
        return modules.Values
            .Where(m => collectionHashSet.Contains(m.Metadata.Collection))
            .Select(m => m.Metadata.Identifier);
    }
}