using sdk;
using sdk.ui;

namespace plugins;

public partial class UserControl1 : IUserInterface
{
  public UserControl1()
  {
    InitializeComponent();
  }

  public ModuleIdentifier Identifier => "Penis";
  
  public void ControlChanged(ControlIdentifier identifier, ControlValue value)
  {
  }
  
  public void PortsChanged(IEnumerable<(PortIdentifier identifier, ControlValue value)> ports)
  {
  }
  
}