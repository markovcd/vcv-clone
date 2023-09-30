using System.Collections.ObjectModel;
using engine;
using plugins;
using sdk.ui;

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
  
  public async Task Initialize()
  {
    Modules.Clear();
    
    foreach (var module in patch)
    {
      Modules.Add(module);
    }

    patch.AddModule<TestModule, UserControl1>();
  }
}