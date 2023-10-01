using sdk;

namespace engine;

public class Control
{
  public Control(ControlMetadata metadata)
  {
    Metadata = metadata;
    Voltage = metadata.Default;
  }
    
  public Voltage Voltage { get; private set; }
  
  public ControlVoltage ControlVoltage => new (Metadata.Identifier, Voltage);
  
  public ControlMetadata Metadata { get; }

  public void ChangeValue(Voltage newValue)
  {
    if (newValue < Metadata.Minimum) throw new ArgumentOutOfRangeException(nameof(newValue));
    if (newValue > Metadata.Maximum) throw new ArgumentOutOfRangeException(nameof(newValue));

    Voltage = newValue;
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
    c.ChangeValue(c.Voltage);
    return c;
  }
}