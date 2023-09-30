using System.Collections.Immutable;
using engine.ui;
using sdk;
using sdk.ui;

namespace engine;

public class ModuleInstance
{
    public ModuleInstance(IModule module, IUserInterface userInterface)
    {
        Module = module;
        UserInterface = userInterface;
        
        Identifier = InstanceIdentifier.Generate();
        Inputs = module.Inputs.ToImmutableDictionary(m => m.Identifier, m => new Input(m));
        Outputs = module.Outputs.ToImmutableDictionary(m => m.Metadata.Identifier, m => new Output(m));
        Controls = module.Controls.ToImmutableDictionary(m => m.Identifier, m => new Control(m));
    }
    
    public Position Position { get; set; }

    public IModule Module { get; }
    
    public IUserInterface UserInterface { get; }
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
        var connectedInputs = Inputs.Where(x => x.Value.IsConnected);
        var connectedOutputs = Outputs.Where(x => x.Value.IsConnected);
        
        var connectedPorts = connectedInputs.Select(x => x.Key)
            .Concat(connectedOutputs.Select(x => x.Key))
            .ToImmutableHashSet();
        
        var arguments = new ProcessArguments(
            sampleRate,
            sampleTime,
            sampleIndex,
            Controls.ToImmutableDictionary(x => x.Key, x => x.Value.Value),
            connectedPorts,
            Inputs.ToImmutableDictionary(x => x.Key, x => x.Value.Value));
        
        try
        {
            Module.Process(arguments);
            var values = connectedInputs.Select(i => (i.Key, i.Value.Value))
                .Concat(connectedOutputs.Select(i => (i.Key, i.Value.Value)));
            UserInterface.PortsChanged(values);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public void InitializeUserInterface()
    {
        foreach (var control in Controls.Values)
            UserInterface.InitializeUserInterface(control);
        
        foreach (var input in Inputs.Values)
            UserInterface.InitializeUserInterface(input);
        
        foreach (var output in Outputs.Values)
             UserInterface.InitializeUserInterface(output);
    }
}