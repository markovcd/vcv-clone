using System.Windows;
using engine;
using plugins;

namespace main;

public partial class App
{
  protected override void OnStartup(StartupEventArgs e)
  {
    InstalledModules.Load(typeof(TestModule).Assembly);
    var patch = new Patch();
    var vm = new MainViewModel(patch);
    var window = new Window1 { DataContext = vm };
      
    window.Show();
  }
}