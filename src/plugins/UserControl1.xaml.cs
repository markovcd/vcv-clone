using System.Windows.Controls;
using sdk;
using sdk.ui;

namespace plugins;

public partial class UserControl1 : IUserInterface
{
  private readonly IReadOnlyDictionary<ControlIdentifier, Control> controlMappings; 
  private readonly IReadOnlyDictionary<PortIdentifier, Port> portMappings; 

  public UserControl1()
  {
    InitializeComponent();
    
    controlMappings = new Dictionary<ControlIdentifier, Control>
    {
      { TestModule.DupaControl, Dupa }
    };

    portMappings = new Dictionary<PortIdentifier, Port>
    {
      { TestModule.Input1, in1 },
      { TestModule.Output1, out1 }
    };
  }

  public ModuleIdentifier Identifier => "Penis";

  public Control GetControl(ControlIdentifier identifier)
  {
    return controlMappings[identifier];
  }

  public Port GetPort(PortIdentifier identifier)
  {
    return portMappings[identifier];
  }

  public void ControlChanged(ControlIdentifier identifier, Voltage value)
  {
  }

  public void PortsChanged(IReadOnlyList<PortVoltage> ports)
  {
  }


  
}