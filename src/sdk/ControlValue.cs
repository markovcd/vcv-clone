namespace sdk;

public struct ControlValue : IComparable<ControlValue>, IEquatable<ControlValue>
{
  private readonly double value;

  private ControlValue(double value)
  {
    this.value = value;
  }
  
  public static implicit operator ControlValue(double value)
  {
    return new ControlValue(value);
  }
  
  public static implicit operator double(ControlValue controlValue)
  {
    return controlValue.value;
  }
  
  public int CompareTo(ControlValue other)
  {
    return value.CompareTo(other.value);
  }

  public static bool operator <(ControlValue left, ControlValue right)
  {
    return left.CompareTo(right) < 0;
  }

  public static bool operator <=(ControlValue left, ControlValue right)
  {
    return left.CompareTo(right) <= 0;
  }

  public static bool operator >(ControlValue left, ControlValue right)
  {
    return left.CompareTo(right) > 0;
  }

  public static bool operator >=(ControlValue left, ControlValue right)
  {
    return left.CompareTo(right) >= 0;
  }

  public bool Equals(ControlValue other)
  {
    return value.Equals(other.value);
  }

  public override bool Equals(object? obj)
  {
    return obj is ControlValue other && Equals(other);
  }

  public override int GetHashCode()
  {
    return value.GetHashCode();
  }

  public static bool operator ==(ControlValue left, ControlValue right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(ControlValue left, ControlValue right)
  {
    return !left.Equals(right);
  }
}
