namespace _2023_12_20;

public record Network(Broadcaster Broadcaster, IDictionary<string, IPulseModule> Handlers)
{
    private static readonly string[] Targets = { "gt","vr","nl","lr" };

    public readonly IDictionary<string, long> _trackers = Targets.ToDictionary(t => t, t => 0L);
    public bool IsComplete => _trackers.Values.All(v => v > 0);
    public long TotalPressesRequired => _trackers.Values.Aggregate(-1L, (acc, v) => v < 0L ? v : LCM(acc, v));

    private long _pressed;
    public void PressButton()
    {
        _pressed++;

        var emitted = new List<EmittedPulse>();

        foreach (var target in Broadcaster.Targets)
            emitted.Add(new EmittedPulse("", target, Pulse.Low));

        while (emitted.Count > 0)
        {
            var queued = new List<EmittedPulse>();

            foreach (var (source, target, pulse) in emitted)
            {
                if (!Handlers.TryGetValue(target, out var handler))
                    continue;

                handler.Receive(source, pulse);

                if (handler.Signal == Pulse.High && _trackers.TryGetValue(target, out var value) && value == 0L)
                    _trackers[target] = _pressed;

                if (IsComplete)
                    return;

                queued.AddRange(handler.Emit());
            }

            emitted.Clear();
            emitted.AddRange(queued);
        }
    }

    private static long LCM(long a, long b) => a * b / GCD(a, b);
    private static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
}
