public record Control(
  ControlIdentifier Identifier,
  ControlMetadata Metadata, 
  ControlValue Minimum, 
  ControlValue Maximum, 
  ControlValue Default)
{
  public ControlValue Value { get; private set; } = Default;

  public void ChangeValue(ControlValue newValue)
  {
    if (newValue < Minimum) throw new ArgumentOutOfRangeException(nameof(newValue));
    if (newValue > Maximum) throw new ArgumentOutOfRangeException(nameof(newValue));

    Value = newValue;
  }

  public void Reset()
  {
    ChangeValue(Default);
  }

  public void Randomize()
  {
    var random = new ControlValue(Random.Shared.NextDouble() * (Maximum.Value - Minimum.Value) + Minimum.Value);
    ChangeValue(random);
  }
}