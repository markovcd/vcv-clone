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
    Output?.DisconnectInternal(this);
    DisconnectInternal();
  }

  internal void ConnectInternal(Output output)
  {
    if (output == Output) throw new InvalidOperationException("output is already connected"); // todo
    if (Output is not null) throw new InvalidOperationException("there is already other output connected"); // todo
    Output = output;
  }

  internal void DisconnectInternal()
  {
    if (Output is null) throw new InvalidOperationException("no output connected");
    Output = null;
  }
}