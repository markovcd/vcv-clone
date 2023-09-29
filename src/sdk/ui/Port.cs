using System.Windows;

namespace sdk.ui;

public class Port : System.Windows.Controls.Control
{
  public static readonly RoutedEvent ConnectionChangedEvent = EventManager.RegisterRoutedEvent(
    nameof(ConnectionChanged),
    RoutingStrategy.Bubble,
    typeof(RoutedPropertyChangedEventHandler<Port>),
    typeof(Port));
  
  public event RoutedPropertyChangedEventHandler<Port> ConnectionChanged
  {
    add => AddHandler(ConnectionChangedEvent, value);
    remove => RemoveHandler(ConnectionChangedEvent, value);
  }

  public bool IsInput { get; set; }
}