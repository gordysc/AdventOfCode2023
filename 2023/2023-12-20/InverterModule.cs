namespace _2023_12_20;

public class InverterModule(string label, IEnumerable<string> targets) :IPulseModule
{
    private readonly IDictionary<string, Pulse> _memory = new Dictionary<string, Pulse>();

    public string Label => label;
    public IEnumerable<string> Targets => targets;
    public Pulse Signal => IsAllHigh ? Pulse.Low : Pulse.High;
    public void AddSource(string source)
    {
        _memory[source] = Pulse.Low;
    }

    public void Receive(string source, Pulse pulse)
    {
        _memory[source] = pulse;
    }

    public IEnumerable<EmittedPulse> Emit() =>
        targets.Select(s => new EmittedPulse(label, s, Signal));
    private bool IsAllHigh => _memory.Values.All(v => v == Pulse.High);
}
