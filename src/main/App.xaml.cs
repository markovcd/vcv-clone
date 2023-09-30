using System.Windows;
using engine;

namespace main;

public partial class App : Application
{

  
  protected override void OnStartup(StartupEventArgs e)
  {
    var events = new Events();
    var patch = new Patch(events);
    var vm = new MainViewModel(events, events, patch);
      
    var window = new Window1
    {
      DataContext = vm
    };
      
    window.Show();
      
    vm.Initialize();
  }
}