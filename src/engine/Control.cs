using sdk;

namespace engine;

public class Control
{
  public Control(ControlMetadata metadata)
  {
    Metadata = metadata;
    Value = metadata.Default;
  }
    
  public ControlValue Value { get; private set; }
  
  public ControlMetadata Metadata { get; }

  public void ChangeValue(ControlValue newValue)
  {
    if (newValue < Metadata.Minimum) throw new ArgumentOutOfRangeException(nameof(newValue));
    if (newValue > Metadata.Maximum) throw new ArgumentOutOfRangeException(nameof(newValue));

    Value = newValue;
  }

  public void Reset()
  {
    ChangeValue(Metadata.Default);
  }

  public void Randomize()
  {
    var random = Random.Shared.NextDouble() * (Metadata.Maximum - Metadata.Minimum) + Metadata.Minimum;
    ChangeValue(random);
  }

  public Control Clone()
  {
    var c = new Control(Metadata);
    c.ChangeValue(c.Value);
    return c;
  }
}