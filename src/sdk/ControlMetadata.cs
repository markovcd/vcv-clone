namespace sdk;

public readonly record struct ControlMetadata(
  ControlIdentifier Identifier,
  Voltage Minimum, 
  Voltage Maximum, 
  Voltage Default,
  bool ShouldBeRandomized);