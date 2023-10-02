using System.Windows;
using System.Windows.Controls.Primitives;
using sdk;
using sdk.ui;

namespace engine.ui;

internal static class UserInterfaceExtensions
{
  internal static void InitializeUserInterface(this IUserInterface ui, Control control)
  {
    var uiControl = ui.GetControl(control.Metadata.Identifier);
    uiControl.SetParentUi(ui);
    uiControl.SetAssociatedControl(control);
    
    switch (uiControl)
    {
      case RangeBase rangeBase:
        InitializeUserInterface(rangeBase, control);
        break;
      case ToggleButton toggleButton:
        InitializeUserInterface(toggleButton, control);
        break;
    }
  }

  private static void InitializeUserInterface(RangeBase rangeBase, Control control)
  {
    rangeBase.Minimum = control.Metadata.Minimum;
    rangeBase.Maximum = control.Metadata.Maximum;
    rangeBase.Value = control.Metadata.Default;
    rangeBase.ValueChanged += OnRangeValueChanged;
  }
  
  private static void InitializeUserInterface(ToggleButton toggleButton, Control control)
  {
    toggleButton.Checked += OnToggleChecked;
    toggleButton.Unchecked += OnToggleUnchecked;
  }
  
  internal static void InitializeUserInterface(this IUserInterface ui, Input input)
  {
    var uiPort = ui.GetPort(input.Metadata.Identifier);
    uiPort.SetAssociatedInput(input);
    uiPort.IsInput = true;
    uiPort.ConnectionChanged += InputConnectionChanged;
  }

  internal static void InitializeUserInterface(this IUserInterface ui, Output output)
  {
    var uiPort = ui.GetPort(output.Metadata.Identifier);
    uiPort.SetAssociatedOutput(output);
    uiPort.IsInput = false;
  }
  
  private static void InputConnectionChanged(object sender, RoutedPropertyChangedEventArgs<Connector> e)
  {
    var input = ((FrameworkElement)sender).GetAssociatedInput();
    if (e.NewValue is null)
    {
      input.Disconnect();
      return;
    }

    var output = ((FrameworkElement)sender).GetAssociatedOutput();
    input.Connect(output);
  }
  
  private static void ChangeControlValue(object sender, Func<Control, Voltage> newValue)
  {
    var control = ((FrameworkElement)sender).GetAssociatedControl();
    control.ChangeValue(newValue(control));
    var ui = ((FrameworkElement)sender).GetParentUi();
    ui.ControlChanged(control.Metadata.Identifier, control.Voltage);
  }
  
  private static void ChangeControlValue(object sender, Voltage newValue)
  {
    ChangeControlValue(sender, _ => newValue);
  }
  
  private static void OnToggleUnchecked(object sender, RoutedEventArgs e)
  {
    ChangeControlValue(sender, c => c.Metadata.Minimum);
  }

  private static void OnToggleChecked(object sender, RoutedEventArgs e)
  {
    ChangeControlValue(sender, c => c.Metadata.Maximum);
  }
    
  private static void OnRangeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
  {
    ChangeControlValue(sender, args.NewValue);
  }
}