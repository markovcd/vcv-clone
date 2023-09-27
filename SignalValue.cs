public record struct SignalValue(double Value) : IComparable<SignalValue>
{
  public int CompareTo(SignalValue other)
  {
    return Value.CompareTo(other.Value);
  }

  public static bool operator <(SignalValue left, SignalValue right)
  {
    return left.CompareTo(right) < 0;
  }

  public static bool operator <=(SignalValue left, SignalValue right)
  {
    return left.CompareTo(right) <= 0;
  }

  public static bool operator >(SignalValue left, SignalValue right)
  {
    return left.CompareTo(right) > 0;
  }

  public static bool operator >=(SignalValue left, SignalValue right)
  {
    return left.CompareTo(right) >= 0;
  }
    
  public static implicit operator SignalValue(double v)
  {
    return new SignalValue(v);
  }
    
  public static implicit operator SignalValue(int v)
  {
    return new SignalValue(v);
  }
}