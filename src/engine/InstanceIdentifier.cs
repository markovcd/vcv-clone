namespace engine;

public readonly struct InstanceIdentifier : IEquatable<InstanceIdentifier>
{
    private readonly Guid value;

    private InstanceIdentifier(Guid value)
    {
        this.value = value;
    }

    public static implicit operator InstanceIdentifier(Guid value)
    {
        return new InstanceIdentifier(value);
    }

    public static implicit operator Guid(InstanceIdentifier identifier)
    {
        return identifier.value;
    }

    public bool Equals(InstanceIdentifier other)
    {
        return value == other.value;
    }

    public override bool Equals(object? obj)
    {
        return obj is InstanceIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public static bool operator ==(InstanceIdentifier left, InstanceIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(InstanceIdentifier left, InstanceIdentifier right)
    {
        return !left.Equals(right);
    }

    public static InstanceIdentifier Generate()
    {
        return Guid.NewGuid();
    }
}