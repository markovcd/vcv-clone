using System.Collections.ObjectModel;
using System.Windows.Input;
using engine;
using engine.ui;

namespace main;

public class MainViewModel
{
  private readonly Patch patch;
  
  private Position clickPosition;
  private Position origin;
  private Position offset;
  private double scale = 1;


  public MainViewModel(Patch patch)
  {
    this.patch = patch;
  }
  
  public Operation Operation { get; set; }
  
  private const int GridSize = 50;
  
  public static double CanvasWidth => 5000;
  public static double CanvasHeight => 5000;

  public bool StartDraggingModule(Position newOrigin, Position newClickPosition)
  {
    origin = newOrigin;
    Operation = Operation.DraggingModule;
    clickPosition = newClickPosition;
    return true;
  }
  
  public Position? DragModule(Position mouse, ModuleInstance instance)
  {
    if (Operation != Operation.DraggingModule) return null;
    
    var x = SnapToGrid(origin.X + (mouse.X - clickPosition.X));
    var y = SnapToGrid(origin.Y + (mouse.Y - clickPosition.Y));

    const int minX = 0;
    const int minY = 0;

    var maxX = CanvasWidth - instance.UserInterface.ActualWidth;
    var maxY = CanvasHeight - instance.UserInterface.ActualHeight;
    
    if (x < minX) x = minX;
    if (y < minY) y = minY;
    
    if (x > maxX) x = maxX;
    if (y > maxY) y = maxY;

    return new Position(x, y);
  }
  
  public bool StopDraggingModule(ModuleInstance moduleInstance, Position finalPosition)
  {
    if (Operation != Operation.DraggingModule) return false;
    
    Operation = Operation.None;
    
    moduleInstance.Position = finalPosition;

    return true;
  }
  
  private static double SnapToGrid(double position)
  {
    return Math.Floor(position / GridSize) * GridSize;
  }
    
  public ObservableCollection<ModuleInstance> Modules { get; } = new ();

  public void AddModule(Position position)
  {
    var identifier = patch.AddModule("Penis", new Position(SnapToGrid(position.X), SnapToGrid(position.Y)));
    var module = patch.GetModule(identifier);
    Modules.Add(module);
  }
  
  public bool StartPanning(Position newOffset, Position newClickPosition)
  {
    offset = newOffset;
    clickPosition = newClickPosition;
    Operation = Operation.Panning;

    return true;
  }
  
  public Position? Pan(Position scrollPosition) 
  {
    if (Operation != Operation.Panning) return null;

    return new Position(
      offset.X + (clickPosition.X - scrollPosition.X),
      offset.Y + (clickPosition.Y - scrollPosition.Y));
  }
  
  public bool StopPanning()
  {
    if (Operation != Operation.Panning) return false;
    Operation = Operation.None;
    return true;
  }
  
  public (double scale, Position newSize) Zoom(int delta)
  {
    const double minScale = 0.1;
    const double maxScale = 10;
    var zoomFactor = delta > 0 ? 1.1 : 0.9;

    scale *= zoomFactor;
    if (scale < minScale) scale = minScale;
    if (scale > maxScale) scale = maxScale;
    
    return (scale, new Position(CanvasWidth * scale, CanvasHeight * scale));
  }
  
  
  public ICommand DeleteModuleCommand { get; }

  public void DeleteModule(InstanceIdentifier identifier)
  {
    patch.DeleteModule(identifier);
  }
}