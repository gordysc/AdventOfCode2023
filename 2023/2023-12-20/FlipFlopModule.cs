namespace _2023_12_20;

public class FlipFlopModule(string label, IEnumerable<string> targets) : IPulseModule
{
    private bool _enabled;
    private Pulse _memory = Pulse.Low;

    public IEnumerable<string> Targets => targets;

    public string Label => label;
    public void AddSource(string source)
    {
        // Do nothing
    }
    public void Receive(string source, Pulse pulse)
    {
        _memory = pulse;

        if (pulse != Pulse.High)
            _enabled = !_enabled;
    }

    public IEnumerable<EmittedPulse> Emit()
    {
        if (_memory == Pulse.High)
            return Enumerable.Empty<EmittedPulse>();

        return targets.Select(s => new EmittedPulse(label, s, _enabled ? Pulse.High : Pulse.Low));
    }
}
