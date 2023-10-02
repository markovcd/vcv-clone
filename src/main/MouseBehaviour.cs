using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace main;

public class MouseBehaviour : Behavior<UIElement>
{
    public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
        nameof(MouseY), typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

    public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
        nameof(MouseX), typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

    private Point originalMouseDownPoint;
    private double horizontalOffset = 1;
    private double verticalOffset = 1;
    private bool isPanning;

    public double MouseY 
    {
        get => (double)GetValue(MouseYProperty);
        set => SetValue(MouseYProperty, value);
    }

    public double MouseX 
    {
        get => (double)GetValue(MouseXProperty);
        set => SetValue(MouseXProperty, value);
    }

    protected override void OnAttached() 
    {
        AssociatedObject.MouseUp += AssociatedObjectOnMouseUp;
        AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        AssociatedObject.MouseDown += AssociatedObjectOnMouseDown;
    }
    protected override void OnDetaching()
    {
        AssociatedObject.MouseUp -= AssociatedObjectOnMouseUp;
        AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        AssociatedObject.MouseDown -= AssociatedObjectOnMouseDown;
    }

    private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs) 
    {
        var canvasPosition = mouseEventArgs.GetPosition(AssociatedObject);

        MouseX = canvasPosition.X;
        MouseY = canvasPosition.Y;

        var canvas = (FrameworkElement)sender;
        var scrollViewer = (ScrollViewer)canvas.Parent;

        var scrollPosition = mouseEventArgs.GetPosition(scrollViewer);

        if (!isPanning) return;

        scrollViewer.ScrollToHorizontalOffset(horizontalOffset + (originalMouseDownPoint.X - scrollPosition.X));
        scrollViewer.ScrollToVerticalOffset(verticalOffset + (originalMouseDownPoint.Y - scrollPosition.Y));
    }
    
    private void AssociatedObjectOnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.MiddleButton != MouseButtonState.Pressed) return;
        
        var canvas = (FrameworkElement)sender;
        var scrollViewer = (ScrollViewer)canvas.Parent;

        horizontalOffset = scrollViewer.HorizontalOffset;
        verticalOffset = scrollViewer.VerticalOffset;
        originalMouseDownPoint = e.GetPosition(scrollViewer);

        isPanning = true;

        e.MouseDevice.OverrideCursor = Cursors.Hand;
        canvas.CaptureMouse();
        canvas.Focus();
    }
    
    private void AssociatedObjectOnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (e.MiddleButton != MouseButtonState.Released) return;
        
        var canvas = (FrameworkElement)sender;
        canvas.ReleaseMouseCapture();

        isPanning = false;
        e.MouseDevice.OverrideCursor = null;
    }
}