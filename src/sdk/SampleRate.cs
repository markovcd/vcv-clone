namespace sdk;

public readonly struct SampleRate
{
  public static SampleRate Default => 44100;
  
  public double Hertz { get; }
  
  private SampleRate(double hertz)
  {
    Hertz = hertz;
  }

  public static implicit operator SampleRate(double hertz)
  {
    return new SampleRate(hertz);
  }

  public static implicit operator double(SampleRate index)
  {
    return index.Hertz;
  }
}