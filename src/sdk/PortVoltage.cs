namespace sdk;

public readonly record struct PortVoltage(PortIdentifier Identifier, Voltage Voltage, bool IsConnected);