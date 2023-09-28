public record struct SampleIndex(long Value)
{
  public SampleIndex Next => new (Value + 1);
}