namespace sdk;

public struct PortIdentifier : IEquatable<PortIdentifier>
{
  private readonly string value;

  private PortIdentifier(string value)
  {
    this.value = value;
  }

  public static implicit operator PortIdentifier(string value)
  {
    return new PortIdentifier(value);
  }

  public static implicit operator string(PortIdentifier identifier)
  {
    return identifier.value;
  }

  public bool Equals(PortIdentifier other)
  {
    return value == other.value;
  }

  public override bool Equals(object? obj)
  {
    return obj is PortIdentifier other && Equals(other);
  }

  public override int GetHashCode()
  {
    return value.GetHashCode();
  }

  public static bool operator ==(PortIdentifier left, PortIdentifier right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(PortIdentifier left, PortIdentifier right)
  {
    return !left.Equals(right);
  }
}