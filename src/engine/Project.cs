using sdk;

public class Project
{
    private readonly Dictionary<InstanceIdentifier, ModuleInstance> modules = new();
    
    public void AddModule(ModuleIdentifier identifier)
    {
        var instance = InstalledModules.Get(identifier);
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
            instance.Process(sampleRate, sampleTime, sampleIndex);
    }

    public void Connect(
        InstanceIdentifier source,
        InstanceIdentifier destination, 
        PortIdentifier input,
        PortIdentifier output)
    {
        GetModule(destination)
            .Inputs[input]
            .Connect(GetModule(source).Outputs[output]);
    }

    public void Disconnect(InstanceIdentifier destination, PortIdentifier input)
    {
        GetModule(destination)
            .Inputs[input]
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