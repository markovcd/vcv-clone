using System.Windows;
using sdk.ui;

namespace engine.ui;

internal static class AttachedProperties
{
  public static readonly DependencyProperty ParentUiProperty = RegisterAttached("ParentUi");
  
  public static IUserInterface GetParentUi(this DependencyObject obj)
  {
    return (IUserInterface)obj.GetValue(ParentUiProperty);
  }

  public static void SetParentUi(this DependencyObject obj, IUserInterface parent)
  {
    obj.SetValue(ParentUiProperty, parent);
  }
  
  
  public static readonly DependencyProperty AssociatedControlProperty = RegisterAttached("AssociatedControl");
  
  public static Control GetAssociatedControl(this DependencyObject obj)
  {
    return (Control)obj.GetValue(AssociatedControlProperty);
  }

  public static void SetAssociatedControl(this DependencyObject obj, Control control)
  {
    obj.SetValue(AssociatedControlProperty, control);
  }
  
  
  public static readonly DependencyProperty AssociatedInputProperty = RegisterAttached("AssociatedInput");
  
  public static Input GetAssociatedInput(this DependencyObject obj)
  {
    return (Input)obj.GetValue(AssociatedInputProperty);
  }

  public static void SetAssociatedInput(this DependencyObject obj, Input input)
  {
    obj.SetValue(AssociatedControlProperty, input);
  }
  
  
  public static readonly DependencyProperty AssociatedOutputProperty = RegisterAttached("AssociatedOutput");
  
  public static Output GetAssociatedOutput(this DependencyObject obj)
  {
    return (Output)obj.GetValue(AssociatedOutputProperty);
  }

  public static void SetAssociatedOutput(this DependencyObject obj, Output output)
  {
    obj.SetValue(AssociatedControlProperty, output);
  }
  
  
  private static DependencyProperty RegisterAttached(string name)
  {
    return DependencyProperty.RegisterAttached(
      name, 
      typeof(IUserInterface), 
      typeof(AttachedProperties));
  }
}