using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace main;

public partial class Window1 
{
  private bool isDragging;
  private Point clickPosition;
  private TranslateTransform origin;
  private readonly ScaleTransform scale = new(1, 1);
  private const int GridSize = 50;

  private Canvas mainCanvas = null!;

  private MainViewModel Vm => (MainViewModel)DataContext;

  public Window1()
  {
    InitializeComponent();
  }
  
  public static double CanvasWidth => 5000;
  public static double CanvasHeight => 5000;

  private void Canvas_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
  {
    var p = e.GetPosition(mainCanvas);
    p.X = SnapToGrid(p.X);
    p.Y = SnapToGrid(p.Y);
    
    Vm.AddModule(p.X, p.Y);
  }

  private static double SnapToGrid(double position)
  {
    return Math.Floor(position / GridSize) * GridSize;
  }

  private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    var draggable = (UIElement)sender;
    origin = draggable.RenderTransform as TranslateTransform ?? new TranslateTransform();
    isDragging = true;
    clickPosition = e.GetPosition(mainCanvas);
    draggable.CaptureMouse();
  }

  private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    isDragging = false;
    var draggable = (UIElement)sender;
    draggable.ReleaseMouseCapture();
  }

  private void Canvas_MouseMove(object sender, MouseEventArgs e)
  {
    if (!isDragging) return;

    var draggable = (UIElement)sender;
    var currentPosition = e.GetPosition(mainCanvas);
    var transform = draggable.RenderTransform as TranslateTransform ?? new TranslateTransform();
    transform.X = origin.X + (currentPosition.X - clickPosition.X);
    transform.Y = origin.Y + (currentPosition.Y - clickPosition.Y);
    
    transform.X = SnapToGrid(transform.X);
    transform.Y = SnapToGrid(transform.Y);
    
    draggable.RenderTransform = new TranslateTransform(transform.X, transform.Y);
  }

  private void Canvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    var zoomFactor = e.Delta > 0 ? 1.1 : 0.9;

    scale.ScaleX *= zoomFactor;
    scale.ScaleY *= zoomFactor;
    
    if (scale.ScaleX < 0.1)
    {
      scale.ScaleX = 0.1;
      scale.ScaleY = 0.1;
    }
    else if (scale.ScaleX > 10)
    {
      scale.ScaleX = 10;
      scale.ScaleY = 10;
    }
    
    var c = sender as FrameworkElement;
    c.Width = CanvasWidth * scale.ScaleX;
    c.Height = CanvasHeight * scale.ScaleY;

    e.Handled = true;
  }

  private void MainCanvas_OnLoaded(object sender, RoutedEventArgs e)
  {
    mainCanvas = (Canvas)sender;
    mainCanvas.LayoutTransform = scale;
  }
}