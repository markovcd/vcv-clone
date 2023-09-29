namespace sdk;

public record ControlMetadata(
  ControlIdentifier Identifier,
  ControlValue Minimum, 
  ControlValue Maximum, 
  ControlValue Default,
  bool ShouldBeRandomized);