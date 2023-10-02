using System.Collections.ObjectModel;
using System.Windows.Input;
using engine;
using engine.ui;

namespace main;

public class MainViewModel
{
  private readonly Patch patch;

  public MainViewModel(Patch patch)
  {
    this.patch = patch;
  }


    
  public ObservableCollection<ModuleInstance> Modules { get; } = new ();

  public void AddModule(double x, double y)
  {
    var position  = new Position(x, y);
    var identifier = patch.AddModule("Penis", position);
    var module = patch.GetModule(identifier);
    Modules.Add(module);
  }
  
  public ICommand DeleteModuleCommand { get; }

  public void DeleteModule(InstanceIdentifier identifier)
  {
    patch.DeleteModule(identifier);
  }
}