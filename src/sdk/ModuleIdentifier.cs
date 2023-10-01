namespace sdk;

public readonly struct ModuleIdentifier : IEquatable<ModuleIdentifier>
{
  private readonly string value;

  private ModuleIdentifier(string value)
  {
    this.value = value;
  }

  public static implicit operator ModuleIdentifier(string value)
  {
    return new ModuleIdentifier(value);
  }

  public static implicit operator string(ModuleIdentifier identifier)
  {
    return identifier.value;
  }

  public bool Equals(ModuleIdentifier other)
  {
    return value == other.value;
  }

  public override bool Equals(object? obj)
  {
    return obj is ModuleIdentifier other && Equals(other);
  }

  public override int GetHashCode()
  {
    return value.GetHashCode();
  }

  public static bool operator ==(ModuleIdentifier left, ModuleIdentifier right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(ModuleIdentifier left, ModuleIdentifier right)
  {
    return !left.Equals(right);
  }
}