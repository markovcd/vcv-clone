public record InstanceIdentifier(Guid Value)
{
    public static InstanceIdentifier Generate()
    {
        return new InstanceIdentifier(Guid.NewGuid());
    }
}