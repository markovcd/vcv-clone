using engine;

namespace main;

public class Events : IEventListener<ModuleAdded>, IEventListener<ModuleRemoved>, IEventNotifier
{
  private Action<ModuleAdded>? moduleAddedEvent;
  private Action<ModuleRemoved>? moduleRemovedEvent;
  
  event Action<ModuleAdded>? IEventListener<ModuleAdded>.On
  {
    add => moduleAddedEvent += value;
    remove => moduleAddedEvent -= value;
  }

  event Action<ModuleRemoved>? IEventListener<ModuleRemoved>.On
  {
    add => moduleRemovedEvent += value;
    remove => moduleRemovedEvent -= value;
  }

  public Task Notify(IEvent e)
  {
    switch (e)
    {
      case ModuleAdded moduleAdded:
        moduleAddedEvent?.Invoke(moduleAdded);
        break;
      case ModuleRemoved moduleRemoved:
        moduleRemovedEvent?.Invoke(moduleRemoved);
        break;
    }

    return Task.CompletedTask;
  }
}