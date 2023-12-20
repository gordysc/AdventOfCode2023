namespace _2023_12_20;

public interface IPulseModule
{
    public string Label { get; }
    IEnumerable<string> Targets { get; }
    void AddSource(string source);
    void Receive(string source, Pulse pulse);
    IEnumerable<EmittedPulse> Emit();
}
