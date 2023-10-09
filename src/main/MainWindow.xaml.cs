using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using engine;
using engine.ui;
using sdk.ui;

namespace main;

public partial class MainWindow
{
  private Canvas canvas = null!;
  private Line? currentLine;

  public MainWindow()
  {
    InitializeComponent();
  }

  private MainViewModel Vm => (MainViewModel)DataContext;
  
  private static ModuleInstance GetModuleInstance(object sender)
  {
    return (ModuleInstance)((FrameworkElement)sender).DataContext;
  }
  
  private void StartPanning(IInputElement element)
  {
    if (!Vm.StartPanning(
      new Position(ModuleListScrollViewer.HorizontalOffset, ModuleListScrollViewer.VerticalOffset),
      GetScrollViewerMousePosition())) 
      return;
    
    Mouse.OverrideCursor = Cursors.Hand;
    element.CaptureMouse();
    element.Focus();
  }
  
  private bool StopPanning(IInputElement element)
  {
    if (!Vm.StopPanning()) return false;
    element.ReleaseMouseCapture();
    Mouse.OverrideCursor = null;
    return true;
  }
  
  private void StartDraggingModule(UIElement element)
  {
    if (!Vm.StartDraggingModule(
          new Position(Canvas.GetLeft(element), Canvas.GetTop(element)),
          GetCanvasMousePosition())) 
      return;
    
    element.CaptureMouse();
  }

  private Position GetCanvasMousePosition()
  {
    var point = Mouse.GetPosition(canvas);
    return new Position(point.X, point.Y);
  }
  
  private Position GetScrollViewerMousePosition()
  {
    var point = Mouse.GetPosition(ModuleListScrollViewer);
    return new Position(point.X, point.Y);
  }
  
  private void DragModule(FrameworkElement draggable)
  {
    var mousePosition = GetCanvasMousePosition();
    var elementPosition = Vm.DragModule(mousePosition, GetModuleInstance(draggable));
    if (elementPosition is null) return;
    Canvas.SetLeft(draggable, elementPosition.Value.X);
    Canvas.SetTop(draggable, elementPosition.Value.Y);
  }
  
  private void StopDraggingModule(FrameworkElement element)
  {
    if (!Vm.StopDraggingModule(
          (ModuleInstance)element.DataContext,
          new Position(Canvas.GetLeft(element), Canvas.GetTop(element))))
      return;
    
    element.ReleaseMouseCapture();
  }
  
  private void MainCanvas_OnLoaded(object sender, RoutedEventArgs e)
  {
    canvas = (Canvas)sender;
  }
  
  private void ModuleList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    var element = (FrameworkElement)sender;
    var (newScale, newSize) = Vm.Zoom(e.Delta);
    canvas.LayoutTransform = new ScaleTransform(newScale, newScale);
    element.Width = newSize.X;
    element.Height = newSize.Y;
    e.Handled = true;
  }

  private void Module_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    StartDraggingModule((UIElement)sender);
  }

  private void StartConnecting()
  {
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
    
    canvas.CaptureMouse();
  }
  
  private bool DoConnecting()
  {
    if (currentLine is null) return false;
    var endPoint = Mouse.GetPosition(canvas);
    currentLine.X2 = endPoint.X;
    currentLine.Y2 = endPoint.Y;
    return true;
  }
  
  private bool FinishConnecting()
  {
    if (!DoConnecting()) return false;
    canvas.ReleaseMouseCapture();
    currentLine = null;
    return true;


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
    // }
    //
    // canvas.Children.Remove(currentLine);
    // currentLine = null;
    // operation = Operation.None;
  }
  
  private void Module_MouseMove(object sender, MouseEventArgs e)
  {
    DragModule((FrameworkElement)sender);
  }
  
  private void Module_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    StopDraggingModule((FrameworkElement)sender);
  }
  
  private void ModuleList_OnMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton == MouseButton.Middle) StartPanning((FrameworkElement)sender);
    if (e.ChangedButton == MouseButton.Left) StartConnecting();
  }

  private void ModuleList_OnMouseMove(object sender, MouseEventArgs e)
  {
    if (DoConnecting()) return;
    
    var scrollOffset = Vm.Pan(GetScrollViewerMousePosition());
    if (scrollOffset is null) return;
    
    ModuleListScrollViewer.ScrollToHorizontalOffset(scrollOffset.Value.X);
    ModuleListScrollViewer.ScrollToVerticalOffset(scrollOffset.Value.Y);
  }
  
  private void ModuleList_OnMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (FinishConnecting()) return;
    if (StopPanning((IInputElement)sender)) return;
    if (e.ChangedButton == MouseButton.Right) Vm.AddModule(GetCanvasMousePosition());
  }
}