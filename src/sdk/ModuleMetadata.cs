namespace sdk;

public record ModuleMetadata(
  ModuleIdentifier Identifier,
  string Name,
  string Author,
  string Description,
  string Collection,
  IReadOnlyList<string> Tags);