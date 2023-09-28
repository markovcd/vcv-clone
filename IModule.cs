public interface IModule
{
  ModuleIdentifier Identifier { get; }
  ModuleMetadata Metadata { get; }
  IEnumerable<Input> Inputs { get; }
  IEnumerable<Output> Outputs { get; }
  IEnumerable<Control> Controls { get; }

  void Process(SampleRate sampleRate, SampleTime sampleTime, SampleIndex sampleIndex);
}