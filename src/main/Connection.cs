using System.ComponentModel;
using System.Runtime.CompilerServices;
using engine.ui;

namespace main;

public class Connection : INotifyPropertyChanged
{
  private Position start, end;

  public Position Start
  {
    get => start;
    set => SetField(ref start, value);
  }
  
  public Position End
  {
    get => end;
    set => SetField(ref end, value);
  }

  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
  {
    if (EqualityComparer<T>.Default.Equals(field, value)) return false;
    field = value;
    OnPropertyChanged(propertyName);
    return true;
  }
}