using sdk;

namespace plugins;

public class TestModule : IModule
{
  public static ControlIdentifier DupaControl => 1;

  public static PortIdentifier Input1 => 2;
  
  public static PortIdentifier Output1 => 3;

  
  public static ControlMetadata Dupa => new (DupaControl, 0, 1, 0, true);

  public static PortMetadata Input => new (Input1, "input 1");
  
  private readonly Output output;
  public TestModule()
  {
    output = new Output(new PortMetadata(Output1, "output 1"));
    Outputs = new[] { output };
    Inputs = new[] { Input };
    Controls = new[] { Dupa };
  }
  
  public ModuleMetadata Metadata { get; } = new (
    "Penis", 
    "Test!",
    "Jan Paweł II",
    "DD",
    "Watykan", 
    new[] { "Abc srania", "Ruchanie" });

  public IEnumerable<PortMetadata> Inputs { get; }

  public IEnumerable<Output> Outputs { get; }
  
  public IEnumerable<ControlMetadata> Controls { get; }
  
  public void Process(ProcessArguments arguments)
  {
    if (!arguments.Inputs[Input1].IsConnected) return;
    if (!arguments.ConnectedOutputs.Contains(Output1)) return;
    
    var input = arguments.Inputs[Input1];
    var control = arguments.Controls[DupaControl];
    output.UpdateValue(input.Voltage * control.Voltage);
  }
  
  public IModule Clone()
  {
    return (IModule)MemberwiseClone();
  }
}