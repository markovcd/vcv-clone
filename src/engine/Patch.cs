using System.Windows;
using engine.ui;
using sdk;

namespace engine;

public class Patch 
{
    private readonly Dictionary<InstanceIdentifier, ModuleInstance> modules = new();
    
    public InstanceIdentifier AddModule(ModuleIdentifier identifier, Position position)
    {
        var instance = InstalledModules.Get(identifier);

        instance.Position = position;
        modules.Add(instance.Identifier, instance);
        Application.Current.Dispatcher.Invoke(instance.InitializeUserInterface);
        return instance.Identifier;
    }
    
    public void DeleteModule(InstanceIdentifier identifier)
    {
        var module = GetModule(identifier);
        module.Disconnect();
        modules.Remove(identifier);
    }

    public void Process(SampleRate sampleRate, SampleTime sampleTime, SampleIndex sampleIndex)
    {
        foreach (var instance in modules.Values)
            instance.Process(sampleRate, sampleTime, sampleIndex);
    }
    
    public ModuleInstance GetModule(InstanceIdentifier identifier)
    {
        return modules.GetValueOrDefault(identifier) ?? throw new ModuleNotFoundException();
    }


}