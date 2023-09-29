namespace sdk;

public record ProcessArguments(
  SampleRate SampleRate,
  SampleTime SampleTime,
  SampleIndex SampleIndex,
  IReadOnlyDictionary<ControlIdentifier, ControlValue> Controls,
  IReadOnlySet<PortIdentifier> ConnectedPorts,
  IReadOnlyDictionary<PortIdentifier, ControlValue> Inputs)
{
  public bool IsConnected(PortIdentifier identifier)
  {
    return ConnectedPorts.Contains(identifier);
  }
}