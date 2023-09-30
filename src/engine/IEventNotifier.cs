using engine;

public interface IEventNotifier
{
  Task Notify(IEvent e);
}