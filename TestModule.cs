public class TestModule : IModule
{
  private readonly Input input = new(new InputIdentifier("In"), new IoMetadata("Input"));
  private readonly Output output1 = new(new OutputIdentifier("Out1"), new IoMetadata("Output 1"));
  private readonly Output output2 = new(new OutputIdentifier("Out2"), new IoMetadata("Output 2"));
  private readonly Control attenuation = new(new ControlIdentifier("Attenuation"), 0, 1, 1);
    
  public ModuleIdentifier Identifier { get; } = new (typeof(TestModule).FullName!);
  public ModuleMetadata Metadata { get; }

  public IEnumerable<Input> Inputs => new[] { input };

  public IEnumerable<Output> Outputs => new[] { output1, output2 };

  public IEnumerable<Control> Controls => new[] { attenuation };
    
  public void Process()
  {
    if (!input.IsConnected) return;
        
    foreach (var output in Outputs.Where(o => o.IsConnected))
      output.UpdateValue(input.Value.Value * attenuation.Value.Value);
  }
}