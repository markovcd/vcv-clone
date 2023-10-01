using System.Collections.ObjectModel;
using engine;
using engine.ui;

namespace main;

public class MainViewModel
{
  private readonly Patch patch;

  public MainViewModel(
    IEventListener<ModuleAdded> moduleAddedListener,
    IEventListener<ModuleRemoved> moduleRemovedListener,
    Patch patch)
  {
    this.patch = patch;
    moduleAddedListener.On += e => Modules.Add(this.patch.GetModule(e.Identifier));
    moduleRemovedListener.On += e => Modules.Remove(Modules.First(m => m.Identifier == e.Identifier));
  }


    
  public ObservableCollection<ModuleInstance> Modules { get; } = new ();

  public void AddModule(double x, double y)
  {
    var position  = new Position(x, y);
    var identifier = patch.AddModule("Penis", position);
    var module = patch.GetModule(identifier);
    Modules.Add(module);
  }
}