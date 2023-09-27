public record Output(OutputIdentifier Identifier, IoMetadata Metadata)
{
  private readonly Dictionary<InputIdentifier, Input> inputs = new();
  
  public SignalValue Value { get; private set; }

  public bool IsConnected => inputs.Any();
  
  
  public void Connect(Input input)
  {
    ConnectInternal(input);
    input.ConnectInternal(this);
  }

  public void Disconnect(InputIdentifier identifier)
  {
    if (!inputs.TryGetValue(identifier, out var input)) return;
    DisconnectInternal(identifier);
    input.DisconnectInternal();
  }

  public void Disconnect()
  {
    foreach (var input in inputs.Values)
      input.DisconnectInternal();
    
    inputs.Clear();
  }
  
  internal void ConnectInternal(Input input)
  {
    inputs.Add(input.Identifier, input);
  }

  internal void DisconnectInternal(InputIdentifier input)
  {
    inputs.Remove(input);
  }
  
  public void UpdateValue(SignalValue value)
  {
    Value = value;
  }
}