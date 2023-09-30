using System.Windows;
using sdk.ui;

namespace engine.ui;

public class AttachedProperties
{
  public static readonly DependencyProperty ParentUiProperty =
    DependencyProperty.RegisterAttached(
      "ParentUi", 
      typeof(IUserInterface), 
      typeof(AttachedProperties));

  public static readonly DependencyProperty AssociatedControlProperty =
    DependencyProperty.RegisterAttached(
      "AssociatedControl", 
      typeof(Control), 
      typeof(AttachedProperties));
  
  public static IUserInterface GetParentUi(DependencyObject obj)
  {
    return (IUserInterface)obj.GetValue(ParentUiProperty);
  }

  public static void SetParentUi(DependencyObject obj, IUserInterface parent)
  {
    obj.SetValue(ParentUiProperty, parent);
  }
  
  public static Control GetAssociatedControl(DependencyObject obj)
  {
    return (Control)obj.GetValue(AssociatedControlProperty);
  }

  public static void SetAssociatedControl(DependencyObject obj, Control control)
  {
    obj.SetValue(AssociatedControlProperty, control);
  }
}