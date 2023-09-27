public record Input(InputIdentifier Identifier, IoMetadata Metadata)
{
  public SignalValue Value => Output?.Value ?? 0;

  public bool IsConnected => Output is not null;
  
  public Output? Output { get; private set; }

  public void Duplicate(Input other)
  {
    if (IsConnected) Connect(other.Output!);
  }
  
  public void Connect(Output output)
  {
    output.ConnectInternal(this);
    ConnectInternal(output);
  }
  
  public void Disconnect()
  {
    Output?.DisconnectInternal(Identifier);
    DisconnectInternal();
  }

  internal void ConnectInternal(Output output)
  {
    Output = output;
  }

  internal void DisconnectInternal()
  {
    Output = null;
  }
}