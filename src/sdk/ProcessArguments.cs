namespace sdk;

public readonly record struct ProcessArguments(
  SampleRate SampleRate,
  SampleTime SampleTime,
  SampleIndex SampleIndex,
  IReadOnlyList<ControlVoltage> Controls,
  IReadOnlyList<PortVoltage> Inputs,
  IReadOnlySet<PortIdentifier> ConnectedOutputs);