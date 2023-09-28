public class Oscillator : IModule
{
  public ModuleIdentifier Identifier { get; }
  public ModuleMetadata Metadata { get; }
  public IEnumerable<Input> Inputs { get; }
  public IEnumerable<Output> Outputs { get; }
  public IEnumerable<Control> Controls { get; }
  public void Process(SampleRate sampleRate, SampleTime sampleTime, SampleIndex sampleIndex)
  {
    throw new NotImplementedException();
  }


}