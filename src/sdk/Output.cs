namespace sdk;

public class Output
{ 
  public Output(PortMetadata metadata)
  {
    Metadata = metadata;
  }
    
  public PortMetadata Metadata { get; }
  public Voltage Value { get; private set; }
  
  public void UpdateValue(Voltage value)
  {
    Value = value;
  }
}