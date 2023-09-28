public class Project
{
    private readonly Dictionary<InstanceIdentifier, ModuleInstance> modules = new();
    
    public void AddModule(ModuleIdentifier identifier)
    {
        var installedModule = InstalledModules.Get(identifier);
        var instance = installedModule.New();
        modules.Add(instance.Identifier, instance);
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
            instance.Module.Process(sampleRate, sampleTime, sampleIndex);
    }

    public void Connect(
        InstanceIdentifier source,
        InstanceIdentifier destination, 
        InputIdentifier input,
        OutputIdentifier output)
    {
        GetModule(destination)
            .GetInput(input)
            .Connect(GetModule(source).GetOutput(output));
    }

    public void Disconnect(InstanceIdentifier destination, InputIdentifier input)
    {
        GetModule(destination)
            .GetInput(input)
            .Disconnect();
    }

    public void ChangeControlValue(InstanceIdentifier destination, ControlIdentifier control, ControlValue value)
    {
        GetModule(destination).ChangeValue(control, value);
    }
    
    public ModuleInstance GetModule(InstanceIdentifier identifier)
    {
        return modules.GetValueOrDefault(identifier) ?? throw new ModuleNotFoundException();
    }
}