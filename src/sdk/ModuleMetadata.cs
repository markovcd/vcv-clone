namespace sdk;

public readonly record struct ModuleMetadata(
  ModuleIdentifier Identifier,
  string Name,
  string Author,
  string Description,
  string Collection,
  IReadOnlyList<string> Tags);