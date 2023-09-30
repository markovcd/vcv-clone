using sdk;

namespace plugins;

  
public class TestModule : IModule
{
  public static ControlMetadata Dupa => new ("Dupa", 0, 1, 0, true);

  public static PortMetadata Input => new ("in1", "input 1");
  
  private readonly Output output;
  public TestModule()
  {
    output = new Output(new PortMetadata("out1", "output 1"));
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
    if (!arguments.IsConnected(Input.Identifier)) return;
    if (!arguments.IsConnected(output.Metadata.Identifier)) return;

    var inputValue = arguments.Inputs[Input.Identifier];
    var controlValue = arguments.Controls[Dupa.Identifier];
    output.UpdateValue(inputValue * controlValue);
  }
  
  public IModule Clone()
  {
    return (IModule)MemberwiseClone();
  }
}