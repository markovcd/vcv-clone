public record ModuleMetadata(
    string Name,
    Author Author,
    ModuleDescription Description,
    ModuleCollection Collection,
    IReadOnlyList<ModuleTag> Tags);