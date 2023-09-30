using System.Collections;
using System.Windows;
using sdk;
using sdk.ui;

namespace engine;

public class Patch : IEnumerable<ModuleInstance>
{
    private readonly Dictionary<InstanceIdentifier, ModuleInstance> modules = new();
    private readonly IEventNotifier eventNotifier;

    public Patch(IEventNotifier eventNotifier)
    {
        this.eventNotifier = eventNotifier;
    }


    public void AddModule(ModuleIdentifier identifier)
    {
        var instance = InstalledModules.Get(identifier);
        AddModule(instance);
    }
    
    public void AddModule(ModuleInstance instance)
    {
        modules.Add(instance.Identifier, instance);
        Application.Current.Dispatcher.Invoke(instance.InitializeUserInterface);
        eventNotifier.Notify(new ModuleAdded(instance.Identifier));
    }
    
    public void AddModule<TModule, TUi>() 
        where TModule : IModule, new()
        where TUi : IUserInterface, new()
    {
        var instance = new ModuleInstance(new TModule(), new TUi());
        AddModule(instance);
    }
    
    public void DeleteModule(InstanceIdentifier identifier)
    {
        var module = GetModule(identifier);
        module.Disconnect();
        modules.Remove(identifier);
        eventNotifier.Notify(new ModuleRemoved(identifier));
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

    public IEnumerator<ModuleInstance> GetEnumerator()
    {
        return modules.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}