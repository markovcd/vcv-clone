using sdk;

public class Output
{
  private readonly sdk.Output sdkOutput;
  private readonly HashSet<Input> inputs = new();

  public Output(sdk.Output sdkOutput)
  {
    this.sdkOutput = sdkOutput;
  }

  public bool IsConnected => inputs.Any();
  public PortMetadata Metadata => sdkOutput.Metadata;
  
  public ControlValue Value => sdkOutput.Value;
  
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
    if (!inputs.Add(input)) return; //throw new InvalidOperationException(" input is already connected"); // TODO
  }

  internal void DisconnectInternal(Input input)
  {
    if (!inputs.Remove(input)) return; //throw new InvalidOperationException(" input is not connected"); // TODO
  }
}