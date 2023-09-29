using System.Collections.Immutable;
using sdk;

public class ModuleInstance
{
    private readonly IModule module;
    private readonly IUserInterface ui;
    
    public ModuleInstance(IModule module, IUserInterface ui)
    {
        this.module = module ?? throw new InvalidOperationException();
        this.ui = ui;
        
        Identifier = InstanceIdentifier.Generate();
        Inputs = module.Inputs.ToImmutableDictionary(m => m.Identifier, m => new Input(m));
        Outputs = module.Outputs.ToImmutableDictionary(m => m.Metadata.Identifier, m => new Output(m));
        Controls = module.Controls.ToImmutableDictionary(m => m.Identifier, m => new Control(m));
    }
    
    public InstanceIdentifier Identifier { get; }
    
    public IReadOnlyDictionary<PortIdentifier, Input> Inputs { get; }
    
    public IReadOnlyDictionary<PortIdentifier, Output> Outputs { get; }
    
    public IReadOnlyDictionary<ControlIdentifier, Control> Controls { get; }
    
    public void DisconnectInputs()
    {
        foreach (var input in Inputs.Values)
            input.Disconnect();
    }
    
    public void DisconnectOutputs()
    {
        foreach (var output in Outputs.Values)
            output.Disconnect();
    }
    
    public void Disconnect()
    {
        DisconnectInputs();
        DisconnectOutputs();
    }

    public void ChangeValue(ControlIdentifier identifier, ControlValue value)
    {
        Controls[identifier].ChangeValue(value);
    }
    
    public void RandomizeControls()
    {
        foreach (var control in Controls.Values.Where(c => c.Metadata.ShouldBeRandomized))
          control.Randomize();
    }

    public void ResetControls()
    {
        foreach (var control in Controls.Values)
            control.Reset();
    }
    
    public void Process(SampleRate sampleRate, SampleTime sampleTime, SampleIndex sampleIndex)
    {
        var connectedInputs = Inputs.Where(x => x.Value.IsConnected).Select(x => x.Key);
        var connectedOutputs = Outputs.Where(x => x.Value.IsConnected).Select(x => x.Key);
        var connectedPorts = connectedInputs.Concat(connectedOutputs).ToImmutableHashSet();
        
        var arguments = new ProcessArguments(
            sampleRate,
            sampleTime,
            sampleIndex,
            Controls.ToImmutableDictionary(x => x.Key, x => x.Value.Value),
            connectedPorts,
            Inputs.ToImmutableDictionary(x => x.Key, x => x.Value.Value));
        
        module.Process(arguments);
    }
    
    public void InitializeUserInterface()
    {
        foreach (var control in Controls.Values)
            ui.InitializeUserInterface(control);

        foreach (var input in Inputs.Values)
            ui.InitializeUserInterface(input);
        
        foreach (var output in Outputs.Values)
            ui.InitializeUserInterface(output);
    }
}