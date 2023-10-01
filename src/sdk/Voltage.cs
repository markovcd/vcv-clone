namespace sdk;

public readonly struct Voltage : IComparable<Voltage>, IEquatable<Voltage>
{
  private readonly double value;

  private Voltage(double value)
  {
    this.value = value;
  }
  
  public static implicit operator Voltage(double value)
  {
    return new Voltage(value);
  }
  
  public static implicit operator double(Voltage voltage)
  {
    return voltage.value;
  }
  
  public int CompareTo(Voltage other)
  {
    return value.CompareTo(other.value);
  }

  public static bool operator <(Voltage left, Voltage right)
  {
    return left.CompareTo(right) < 0;
  }

  public static bool operator <=(Voltage left, Voltage right)
  {
    return left.CompareTo(right) <= 0;
  }

  public static bool operator >(Voltage left, Voltage right)
  {
    return left.CompareTo(right) > 0;
  }

  public static bool operator >=(Voltage left, Voltage right)
  {
    return left.CompareTo(right) >= 0;
  }

  public bool Equals(Voltage other)
  {
    return value.Equals(other.value);
  }

  public override bool Equals(object? obj)
  {
    return obj is Voltage other && Equals(other);
  }

  public override int GetHashCode()
  {
    return value.GetHashCode();
  }

  public static bool operator ==(Voltage left, Voltage right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(Voltage left, Voltage right)
  {
    return !left.Equals(right);
  }
}
