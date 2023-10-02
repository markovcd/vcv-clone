using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using engine;
using engine.ui;
using sdk.ui;

namespace main;

public enum Operation
{
  None, 
  DraggingModule,
  Panning,
  Connecting
}

public partial class Window1
{
  private Operation operation;
  private Point clickPosition;
  private Point origin;
  private readonly ScaleTransform scale = new(1, 1);
  private const int GridSize = 50;

  private Point offset;
  
  private Canvas canvas = null!;
  private Line? currentLine;

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
    var p = Mouse.GetPosition(canvas);
    p.X = SnapToGrid(p.X);
    p.Y = SnapToGrid(p.Y);
    var vm = (MainViewModel)DataContext;
    vm.AddModule(p.X, p.Y);
  }

  private void StartPanning(IInputElement element)
  {
    offset = new Point(ModuleListScrollViewer.HorizontalOffset, ModuleListScrollViewer.VerticalOffset);
    clickPosition = Mouse.GetPosition(ModuleListScrollViewer);

    operation = Operation.Panning;

    Mouse.OverrideCursor = Cursors.Hand;
    element.CaptureMouse();
    element.Focus();
  }
  
  private void Pan() 
  {
    if (operation != Operation.Panning) return;
    
    var scrollPosition = Mouse.GetPosition(ModuleListScrollViewer);
    
    ModuleListScrollViewer.ScrollToHorizontalOffset(offset.X + (clickPosition.X - scrollPosition.X));
    ModuleListScrollViewer.ScrollToVerticalOffset(offset.Y + (clickPosition.Y - scrollPosition.Y));
  }
  
  private bool StopPanning(IInputElement element)
  {
    if (operation != Operation.Panning) return false;
    element.ReleaseMouseCapture();

    operation = Operation.None;
    Mouse.OverrideCursor = null;
    return true;
  }
  
  private void StartDraggingModule(UIElement element)
  {
    origin = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
    operation = Operation.DraggingModule;
    clickPosition = Mouse.GetPosition(canvas);
    element.CaptureMouse();
  }
  
  private void DragModule(FrameworkElement draggable)
  {
    if (operation != Operation.DraggingModule) return;

    var currentPosition = Mouse.GetPosition(canvas);
    
    var x = SnapToGrid(origin.X + (currentPosition.X - clickPosition.X));
    var y = SnapToGrid(origin.Y + (currentPosition.Y - clickPosition.Y));

    const int minX = 0;
    const int minY = 0;

    var maxX = CanvasWidth - draggable.ActualWidth;
    var maxY = CanvasHeight - draggable.ActualHeight;
    
    if (x < minX) x = minX;
    if (y < minY) y = minY;
    
    if (x > maxX) x = maxX;
    if (y > maxY) y = maxY;
    
    Canvas.SetLeft(draggable, x);
    Canvas.SetTop(draggable, y);
  }
  
  private void StopDraggingModule(FrameworkElement element)
  {
    if (operation != Operation.DraggingModule) return;
    
    operation = Operation.None;

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
    canvas = (Canvas)sender;
    canvas.LayoutTransform = scale;
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

  private void StartConnecting(Connector connector)
  {
    operation = Operation.Connecting;

    var startPoint = Mouse.GetPosition(canvas);
    currentLine = new Line
    {
      X1 = startPoint.X,
      Y1 = startPoint.Y,
      X2 = startPoint.X,
      Y2 = startPoint.Y,
      Stroke = Brushes.Black,
      StrokeThickness = 2
    };
    canvas.Children.Add(currentLine);

    connector.CaptureMouse();
  }
  
  private void DoConnecting()
  {
    if (operation != Operation.Connecting) return;
    var endPoint = Mouse.GetPosition(canvas);
    currentLine.X2 = endPoint.X;
    currentLine.Y2 = endPoint.Y;
  }
  
  private void FinishConnecting(object sender)
  {
    if (operation != Operation.Connecting) return;
    if (sender is Connector connector)
    {
      var endPoint = Mouse.GetPosition(canvas);
      currentLine.X2 = endPoint.X;
      currentLine.Y2 = endPoint.Y;

      connector.ReleaseMouseCapture();

      // Check if the endpoint is over another connector
      // foreach (var child in canvas.Children)
      // {
      //   if (child is UserControl otherConnector && otherConnector != connector)
      //   {
      //     Point otherCenter = new Point(Canvas.GetLeft(otherConnector) + otherConnector.Width / 2,
      //       Canvas.GetTop(otherConnector) + otherConnector.Height / 2);
      //
      //     if (Math.Abs(endPoint.X - otherCenter.X) < 5 && Math.Abs(endPoint.Y - otherCenter.Y) < 5)
      //     {
      //       // Connect the two connectors (you can add your specific logic here)
      //       MessageBox.Show("Connected!");
      //     }
      //   }
      // }
    }

    canvas.Children.Remove(currentLine);
    currentLine = null;
    operation = Operation.None;
  }
  
  private void Module_MouseMove(object sender, MouseEventArgs e)
  {
    DoConnecting();
    DragModule((FrameworkElement)sender);
  }
  
  private void Module_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    FinishConnecting(sender);
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

  private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
  {
    if (sender is Connector connector) StartConnecting(connector);
    e.Handled = true;
  }
}