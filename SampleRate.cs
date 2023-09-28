public record struct SampleRate(double Hertz)
{
  public static SampleRate Default => new(44100);
}