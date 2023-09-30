namespace sdk.ui;

public interface IUserInterface
{
  ModuleIdentifier Identifier { get; }

  void ControlChanged(ControlIdentifier identifier, ControlValue value);
  
  void PortsChanged(IEnumerable<(PortIdentifier identifier, ControlValue value)> ports);

}