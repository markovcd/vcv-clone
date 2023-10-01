namespace sdk;

public readonly struct SampleTime
{
  public double Seconds { get; }
  
  private SampleTime(double seconds)
  {
    Seconds = seconds;
  }

  public static implicit operator SampleTime(double seconds)
  {
    return new SampleTime(seconds);
  }

  public static implicit operator double(SampleTime time)
  {
    return time.Seconds;
  }
}