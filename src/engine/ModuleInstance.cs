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
        Inputs = module.Inputs.Select(m => new Input(m)).ToImmutableList();
        Outputs = module.Outputs.Select(m => new Output(m)).ToImmutableList();
        Controls = module.Controls.Select(m => new Control(m)).ToImmutableList();
    }
    
    public Position Position { get; set; }

    public IModule Module { get; }
    
    public IUserInterface UserInterface { get; }
    public InstanceIdentifier Identifier { get; }
    
    public IReadOnlyList<Input> Inputs { get; }
    
    public IReadOnlyList<Output> Outputs { get; }
    
    public IReadOnlyList<Control> Controls { get; }
    
    public void DisconnectInputs()
    {
        foreach (var input in Inputs)
            input.Disconnect();
    }
    
    public void DisconnectOutputs()
    {
        foreach (var output in Outputs)
            output.Disconnect();
    }
    
    public void Disconnect()
    {
        DisconnectInputs();
        DisconnectOutputs();
    }
    
    public void RandomizeControls()
    {
        foreach (var control in Controls.Where(c => c.Metadata.ShouldBeRandomized))
          control.Randomize();
    }

    public void ResetControls()
    {
        foreach (var control in Controls)
            control.Reset();
    }
    
    public void Process(SampleRate sampleRate, SampleTime sampleTime, SampleIndex sampleIndex)
    {
         var arguments = new ProcessArguments(
            sampleRate,
            sampleTime,
            sampleIndex,
            Controls.Select(c => c.ControlVoltage).ToImmutableList(),
            Inputs.Select(x => x.PortVoltage).ToImmutableList(),
            Outputs.Where(o => o.IsConnected).Select(o => o.Metadata.Identifier).ToImmutableHashSet());
        
        try
        {
            Module.Process(arguments);
            
            var values = Inputs.Select(i => i.PortVoltage)
                .Concat(Outputs.Select(o => o.PortVoltage)).ToImmutableList();
            
            UserInterface.PortsChanged(values);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public void InitializeUserInterface()
    {
        foreach (var control in Controls)
            UserInterface.InitializeUserInterface(control);
        
        foreach (var input in Inputs)
            UserInterface.InitializeUserInterface(input);
        
        foreach (var output in Outputs)
             UserInterface.InitializeUserInterface(output);
    }
}