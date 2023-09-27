public record struct ControlValue(double Value) : IComparable<ControlValue>
{
    public int CompareTo(ControlValue other)
    {
        return Value.CompareTo(other.Value);
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
    
    public static implicit operator ControlValue(double v)
    {
        return new ControlValue(v);
    }
    
    public static implicit operator ControlValue(int v)
    {
        return new ControlValue(v);
    }
}