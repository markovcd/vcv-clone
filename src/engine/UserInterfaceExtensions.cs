using System.Windows;
using System.Windows.Controls.Primitives;
using sdk;
using sdk.ui;

internal static class UserInterfaceExtensions
{
  internal static void InitializeUserInterface(this IUserInterface ui, Control control)
  {
    var uiControl = ui.GetUiControl(control.Metadata.Identifier);
    uiControl.Tag = control;
        
    if (uiControl is RangeBase rangeBase)
    {
      rangeBase.Minimum = control.Metadata.Minimum;
      rangeBase.Maximum = control.Metadata.Maximum;
      rangeBase.Value = control.Metadata.Default;
      rangeBase.ValueChanged += OnRangeValueChanged;
    }

    if (uiControl is ToggleButton toggleButton)
    {
      toggleButton.Checked += ToggleButtonChecked;
      toggleButton.Unchecked += ToggleButtonUnchecked;
    }
  }
  
  internal static void InitializeUserInterface(this IUserInterface ui, Input input)
  {
    var uiPort = ui.GetUiPort(input.Metadata.Identifier);
    uiPort.Tag = input;
    uiPort.IsInput = true;
    uiPort.ConnectionChanged += InputConnectionChanged;
  }

  internal static void InitializeUserInterface(this IUserInterface ui, Output output)
  {
    var uiPort = ui.GetUiPort(output.Metadata.Identifier);
    uiPort.Tag = output;
    uiPort.IsInput = false;
  }

  private static System.Windows.Controls.Control GetUiControl(this IUserInterface ui, ControlIdentifier identifier)
  {
    return ui.AsControl().FindName(identifier) as System.Windows.Controls.Control 
           ?? throw new InvalidOperationException(identifier); // TODO
  }
  
  private static Port GetUiPort(this IUserInterface ui, PortIdentifier identifier)
  {
    return ui.AsControl().FindName(identifier) as Port 
           ?? throw new InvalidOperationException(identifier); // TODO
  }

  private static System.Windows.Controls.Control AsControl(this IUserInterface ui)
  {
    return (System.Windows.Controls.Control)ui;
  }
  
  private static void InputConnectionChanged(object sender, RoutedPropertyChangedEventArgs<Port> e)
  {
    var input = GetInputFromSender(sender);
    if (e.NewValue is null)
    {
      input.Disconnect();
      return;
    }

    var output = GetOutputFromSender(e.NewValue);
    input.Connect(output);
  }
  
  private static void ToggleButtonUnchecked(object sender, RoutedEventArgs e)
  {
    var control = GetControlFromSender(sender);
    control.ChangeValue(control.Metadata.Minimum);
  }

  private static void ToggleButtonChecked(object sender, RoutedEventArgs e)
  {
    var control = GetControlFromSender(sender);
    control.ChangeValue(control.Metadata.Maximum);
  }
    
  private static void OnRangeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
  {
    var control = GetControlFromSender(sender);

    control.ChangeValue(args.NewValue);
  }
    
  private static Control GetControlFromSender(object sender)
  {
    if (sender is FrameworkElement { Tag: Control control }) return control;
    throw new InvalidOperationException();
  }
  
  private static Input GetInputFromSender(object sender)
  {
    if (sender is FrameworkElement { Tag: Input input }) return input;
    throw new InvalidOperationException();
  }
  
  private static Output GetOutputFromSender(object sender)
  {
    if (sender is FrameworkElement { Tag: Output output }) return output;
    throw new InvalidOperationException();
  }
}