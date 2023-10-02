using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using engine;
using engine.ui;

namespace main;

public partial class Window1 
{
  private bool isDragging;
  private Point clickPosition;
  private Point origin;
  private readonly ScaleTransform scale = new(1, 1);
  private const int GridSize = 50;

  private Point offset;
  private bool isPanning;
  
  private Canvas mainCanvas = null!;

  public Window1()
  {
    InitializeComponent();
  }
  
  public static double CanvasWidth => 5000;
  public static double CanvasHeight => 5000;
  
  private void Zoom(int delta, FrameworkElement element)
  {
    var zoomFactor = delta > 0 ? 1.1 : 0.9;

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
    
    element.Width = CanvasWidth * scale.ScaleX;
    element.Height = CanvasHeight * scale.ScaleY;
  }
  
  private void AddModule()
  {
    var p = Mouse.GetPosition(mainCanvas);
    p.X = SnapToGrid(p.X);
    p.Y = SnapToGrid(p.Y);
    var vm = (MainViewModel)DataContext;
    vm.AddModule(p.X, p.Y);
  }

  private void StartPanning(IInputElement element)
  {
    offset = new Point(ModuleListScrollViewer.HorizontalOffset, ModuleListScrollViewer.VerticalOffset);
    clickPosition = Mouse.GetPosition(ModuleListScrollViewer);

    isPanning = true;

    Mouse.OverrideCursor = Cursors.Hand;
    element.CaptureMouse();
    element.Focus();
  }
  
  private void Pan() 
  {
    if (!isPanning) return;
    
    var scrollPosition = Mouse.GetPosition(ModuleListScrollViewer);
    
    ModuleListScrollViewer.ScrollToHorizontalOffset(offset.X + (clickPosition.X - scrollPosition.X));
    ModuleListScrollViewer.ScrollToVerticalOffset(offset.Y + (clickPosition.Y - scrollPosition.Y));
  }
  
  private bool StopPanning(IInputElement element)
  {
    if (!isPanning) return false;
    element.ReleaseMouseCapture();

    isPanning = false;
    Mouse.OverrideCursor = null;
    return true;
  }
  
  private void StartDraggingModule(UIElement element)
  {
    origin = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
    isDragging = true;
    clickPosition = Mouse.GetPosition(mainCanvas);
    element.CaptureMouse();
  }
  
  private void DragModule(UIElement draggable)
  {
    if (!isDragging) return;

    var currentPosition = Mouse.GetPosition(mainCanvas);
    
    var x = origin.X + (currentPosition.X - clickPosition.X);
    var y = origin.Y + (currentPosition.Y - clickPosition.Y);
    
    x = SnapToGrid(x);
    y = SnapToGrid(y);
    
    Canvas.SetLeft(draggable, x);
    Canvas.SetTop(draggable, y);
  }
  
  private void StopDraggingModule(FrameworkElement element)
  {
    if (!isDragging) return;
    
    isDragging = false;

    var x = Canvas.GetLeft(element);
    var y = Canvas.GetTop(element);
    
    var moduleInstance = (ModuleInstance)element.DataContext;
    moduleInstance.Position = new Position(x, y);
    
    element.ReleaseMouseCapture();
  }

  private static double SnapToGrid(double position)
  {
    return Math.Floor(position / GridSize) * GridSize;
  }
  
  private void MainCanvas_OnLoaded(object sender, RoutedEventArgs e)
  {
    mainCanvas = (Canvas)sender;
    mainCanvas.LayoutTransform = scale;
  }
  
  private void ModuleList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    Zoom(e.Delta, (FrameworkElement)sender);
    e.Handled = true;
  }

  private void Module_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    StartDraggingModule((UIElement)sender);
  }
  
  private void Module_MouseMove(object sender, MouseEventArgs e)
  {
    DragModule((UIElement)sender);
  }
  
  private void Module_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    StopDraggingModule((FrameworkElement)sender);
  }
  
  private void ModuleList_OnMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton == MouseButton.Middle) StartPanning((FrameworkElement)sender);
  }

  private void ModuleList_OnMouseMove(object sender, MouseEventArgs e)
  {
    Pan();
  }
  
  private void ModuleList_OnMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (StopPanning((IInputElement)sender)) return;
    if (e.ChangedButton == MouseButton.Right) AddModule();
  }
}