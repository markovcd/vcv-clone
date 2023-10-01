using System.Windows;
using engine;
using plugins;

namespace main;

public partial class App : Application
{

  
  protected override void OnStartup(StartupEventArgs e)
  {
    InstalledModules.Load(typeof(TestModule).Assembly);
    var events = new Events();
    var patch = new Patch();
    var vm = new MainViewModel(events, events, patch);
    
      
    var window = new Window1
    {
      DataContext = vm
    };
      
    window.Show();
  }
}