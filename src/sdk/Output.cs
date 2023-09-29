namespace sdk;

public class Output
{ 
  public Output(PortMetadata metadata)
  {
    Metadata = metadata;
  }
    
  public PortMetadata Metadata { get; }
  public ControlValue Value { get; private set; }
  
  public void UpdateValue(ControlValue value)
  {
    Value = value;
  }
}