namespace sdk;

public interface IModule
{
  ModuleMetadata Metadata { get; }
  
  IEnumerable<PortMetadata> Inputs { get; }
  IEnumerable<Output> Outputs { get; }
  IEnumerable<ControlMetadata> Controls { get; }

  void Process(ProcessArguments arguments);

  public IModule Clone();
}