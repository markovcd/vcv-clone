namespace sdk;

public readonly struct SampleIndex
{
  private readonly long value;
  
  public SampleIndex Next => value + 1;


  private SampleIndex(long value)
  {
    this.value = value;
  }

  public static implicit operator SampleIndex(long value)
  {
    return new SampleIndex(value);
  }

  public static implicit operator long(SampleIndex index)
  {
    return index.value;
  }
}