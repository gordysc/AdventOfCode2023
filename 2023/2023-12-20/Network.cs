namespace _2023_12_20;

public record Network(Broadcaster Broadcaster, IDictionary<string, IPulseModule> Handlers)
{
    public Dictionary<Pulse,  int> PressButton()
    {
        var counts = new Dictionary<Pulse, int> { { Pulse.Low, 1 }, { Pulse.High, 0 } };
        var emitted = new List<EmittedPulse>();

        foreach (var target in Broadcaster.Targets)
            emitted.Add(new EmittedPulse("", target, Pulse.Low));

        while (emitted.Count > 0)
        {
            var queued = new List<EmittedPulse>();

            foreach (var (source, target, pulse) in emitted)
            {
                counts[pulse]++;

                if (!Handlers.TryGetValue(target, out var handler))
                    continue;

                handler.Receive(source, pulse);
                queued.AddRange(handler.Emit());
            }

            emitted.Clear();
            emitted.AddRange(queued);
        }

        return counts;
    }
}
