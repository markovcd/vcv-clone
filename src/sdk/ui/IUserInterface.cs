namespace sdk.ui;

public interface IUserInterface
{
  ModuleIdentifier Identifier { get; }

  System.Windows.Controls.Control GetControl(ControlIdentifier identifier);

  Port GetPort(PortIdentifier identifier);

  void ControlChanged(ControlIdentifier identifier, Voltage value);
  
  void PortsChanged(IReadOnlyList<PortVoltage> ports);

  double ActualWidth { get; }
  
  double ActualHeight { get; }
}