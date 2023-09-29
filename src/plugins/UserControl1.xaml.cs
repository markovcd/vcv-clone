using sdk;

namespace plugins;

public partial class UserControl1 : IUserInterface
{
  public UserControl1()
  {
    InitializeComponent();
  }

  public ModuleIdentifier Identifier { get; } = typeof(TestModule).FullName!;
}