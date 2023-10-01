namespace sdk;

public readonly struct ControlIdentifier : IEquatable<ControlIdentifier>
{
  private readonly byte value;

  private ControlIdentifier(byte value)
  {
    this.value = value;
  }

  public static implicit operator ControlIdentifier(byte value)
  {
    return new ControlIdentifier(value);
  }

  public static implicit operator byte(ControlIdentifier identifier)
  {
    return identifier.value;
  }

  public bool Equals(ControlIdentifier other)
  {
    return value == other.value;
  }

  public override bool Equals(object? obj)
  {
    return obj is ControlIdentifier other && Equals(other);
  }

  public override int GetHashCode()
  {
    return value.GetHashCode();
  }

  public static bool operator ==(ControlIdentifier left, ControlIdentifier right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(ControlIdentifier left, ControlIdentifier right)
  {
    return !left.Equals(right);
  }
}