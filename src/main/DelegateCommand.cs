using System.Windows.Input;

namespace main;

public class DelegateCommand : ICommand
{
  private bool canExecute = true;
  private readonly Action action;
  private EventHandler? canExecuteChanged;

  public bool CanExecute
  {
    get => canExecute;
    set
    {
      canExecute = value;
      canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  public DelegateCommand(Action action)
  {
    this.action = action;
  }
  
  bool ICommand.CanExecute(object? parameter)
  {
    return CanExecute;
  }

  void ICommand.Execute(object? parameter)
  {
    if (!CanExecute) return;
    action.Invoke();
    
  }

  event EventHandler? ICommand.CanExecuteChanged
  {
    add => canExecuteChanged += value;
    remove => canExecuteChanged -= value;
  }
}