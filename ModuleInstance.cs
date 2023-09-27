public record ModuleInstance(InstanceIdentifier Identifier, IModule Module)
{
    public ModuleInstance New()
    {
        return new ModuleInstance(InstanceIdentifier.Generate(), CloneModule());
    }

    public static ModuleInstance New(IModule module)
    {
        return new ModuleInstance(InstanceIdentifier.Generate(), module);
    }

    private IModule CloneModule(bool withInputs = false)
    {
        var clone = Activator.CreateInstance(Module.GetType()) as IModule ?? throw new InvalidOperationException();
        var controls = Module.Controls.OrderBy(c => c.Identifier).Zip(clone.Controls.OrderBy(c => c.Identifier));
        var inputs = Module.Inputs.OrderBy(c => c.Identifier).Zip(clone.Inputs.OrderBy(c => c.Identifier));

        foreach (var (originalControl, cloneControl) in controls)
            cloneControl.ChangeValue(originalControl.Value);

        if (!withInputs) return clone;
        
        foreach (var (originalInput, cloneInput) in inputs)
            cloneInput.Duplicate(originalInput);

        return clone;
    }

    public void DisconnectInputs()
    {
        foreach (var input in Module.Inputs)
            input.Disconnect();
    }
    
    public void DisconnectOutputs()
    {
        foreach (var output in Module.Outputs)
            output.Disconnect();
    }
    
    public void Disconnect()
    {
        DisconnectInputs();
        DisconnectOutputs();
    }

    public Input GetInput(InputIdentifier identifier)
    {
        return Module.Inputs.FirstOrDefault(i => i.Identifier == identifier) ?? throw new IoNotFoundException();
    }
    
    public Output GetOutput(OutputIdentifier identifier)
    {
        return Module.Outputs.FirstOrDefault(i => i.Identifier == identifier) ?? throw new IoNotFoundException();
    }
    
    public Control GetControl(ControlIdentifier identifier)
    {
        return Module.Controls.FirstOrDefault(i => i.Identifier == identifier) ?? throw new ControlNotFoundException();
    }
}