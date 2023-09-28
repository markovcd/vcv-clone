public record Output(OutputIdentifier Identifier, IoMetadata Metadata)
{
  private readonly HashSet<Input> inputs = new();
  
  public SignalValue Value { get; private set; }

  public bool IsConnected => inputs.Any();
  
  
  public void Connect(Input input)
  {
    ConnectInternal(input);
    input.ConnectInternal(this);
  }

  public void Disconnect(Input input)
  {
    DisconnectInternal(input);
    input.DisconnectInternal();
  }

  public void Disconnect()
  {
    foreach (var input in inputs)
      input.DisconnectInternal();
    
    inputs.Clear();
  }
  
  internal void ConnectInternal(Input input)
  {
    if (!inputs.Add(input)) throw new InvalidOperationException(" input is already connected"); // TODO
  }

  internal void DisconnectInternal(Input input)
  {
    if (!inputs.Remove(input)) throw new InvalidOperationException(" input is not connected"); // TODO
  }
  
  public void UpdateValue(SignalValue value)
  {
    Value = value;
  }
}