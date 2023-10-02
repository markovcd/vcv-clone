using System.Windows;

namespace sdk.ui;

public class Connector : System.Windows.Controls.Control
{
  public static readonly RoutedEvent ConnectionChangedEvent = EventManager.RegisterRoutedEvent(
    nameof(ConnectionChanged),
    RoutingStrategy.Bubble,
    typeof(RoutedPropertyChangedEventHandler<Connector>),
    typeof(Connector));
  
  static Connector() 
  { 
    DefaultStyleKeyProperty.OverrideMetadata(typeof(Connector), new FrameworkPropertyMetadata(typeof(Connector))); 
  } 
    
  public event RoutedPropertyChangedEventHandler<Connector> ConnectionChanged
  {
    add => AddHandler(ConnectionChangedEvent, value);
    remove => RemoveHandler(ConnectionChangedEvent, value);
  }

  public bool IsInput { get; set; }
}