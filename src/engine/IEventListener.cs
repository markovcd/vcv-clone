using engine;

public interface IEventListener<out TEvent> 
  where TEvent : IEvent
{
  event Action<TEvent> On;
}