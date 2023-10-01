using sdk;

namespace engine;

public class Input
{
  private Output? connectedOutput;

  public Input(PortMetadata metadata)
  {
    Metadata = metadata;
  }
  
  public PortMetadata Metadata { get; }
  
  public bool IsConnected => connectedOutput is not null;

  public PortVoltage PortVoltage => new (Metadata.Identifier, connectedOutput?.PortVoltage.Voltage ?? 0, IsConnected);
  
  public void Duplicate(Input other)
  {
    if (IsConnected) Connect(other.connectedOutput!);
  }
  
  public void Connect(Output output)
  {
    output.ConnectInternal(this);
    ConnectInternal(output);
  }
  
  public void Disconnect()
  {
    connectedOutput?.DisconnectInternal(this);
    DisconnectInternal();
  }

  internal void ConnectInternal(Output output)
  {
    //if (output == connectedOutput) throw new InvalidOperationException("output is already connected"); // todo
    //if (connectedOutput is not null) throw new InvalidOperationException("there is already other output connected"); // todo
    connectedOutput = output;
  }

  internal void DisconnectInternal()
  {
    //if (connectedOutput is null) throw new InvalidOperationException("no output connected");
    connectedOutput = null;
  }
}