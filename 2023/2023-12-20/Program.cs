var input = File.ReadAllLines(@"../../../Data/Example.txt");
// var input = File.ReadAllLines(@"../../../Data/Input.txt");
var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve(input);
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private const string Broadcaster = "broadcaster";
    private const string FlipFlop = "%";
    private const string Inverter = "&";
    public void Solve(IEnumerable<string> input)
    {
        var broadcaster = BuildBroadcaster(input);
        var handlers = BuildHandlers(input);
        var network = new Network(broadcaster, handlers);

    }


    private static Broadcaster BuildBroadcaster(IEnumerable<string> input) =>
        new(input.First(l => l.StartsWith(Broadcaster))
            .Split(" -> ")[1].Trim()
            .Split(",").Select(l => l.Trim())
            .ToArray());

    private static IDictionary<string, IHandlePulse> BuildHandlers(IEnumerable<string> input) =>
        input.Where(l => !l.StartsWith(Broadcaster))
            .Select(BuildHandler)
            .ToDictionary();

    private static (string, IHandlePulse) BuildHandler(string line)
    {
        var parts = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToArray();
        
        var nodes = parts[1].Split(",")
            .Select(item => item.Trim()).ToArray();

        var module = parts[0][0..1];
        var label = parts[0][1..];

        return module switch
        {
            FlipFlop => (label, new FlipFlopHandler(nodes)),
            Inverter => (label, new InverterHandler(nodes)),
            _ => throw new InvalidDataException($"Unknown module: {module}")
        };
    }
}

internal enum Pulse { Low, High }

internal interface IHandlePulse
{
    public IEnumerable<string> Labels { get; }
    public int Capacity { get; }
    Pulse? HandlePulse(Pulse pulse);
}

internal class FlipFlopHandler(IEnumerable<string> labels, bool enabled = false) : IHandlePulse
{
    public IEnumerable<string> Labels => labels;
    public int Capacity => labels.Count();
    
    public Pulse? HandlePulse(Pulse pulse)
    {
        if (pulse == Pulse.High) return null;

        enabled = !enabled;

        return enabled ? Pulse.High : Pulse.Low;
    }
}

internal class InverterHandler(IEnumerable<string> labels, Pulse memory = Pulse.Low) : IHandlePulse
{
    public IEnumerable<string> Labels => labels;
    public int Capacity => labels.Count();
    
    public Pulse? HandlePulse(Pulse pulse)
    {
        memory = pulse;

        return memory == Pulse.Low ? Pulse.High : Pulse.Low;
    }
}

internal record Broadcaster(IEnumerable<string> Labels);

internal record Network(Broadcaster Broadcaster, IDictionary<string, IHandlePulse> Handlers)
{
    private readonly Dictionary<string, Pulse?> _state = Handlers.ToDictionary(
        kvp => kvp.Key, 
        kvp => (Pulse?) null
    );
    
    public int PressButton()
    {
        var counts = new Dictionary<Pulse, int> { { Pulse.Low, 0 }, { Pulse.High, 0 } };

        foreach (var label in Broadcaster.Labels)
        {
            counts[Pulse.Low]++;
            _state[label] = Handlers[label].HandlePulse(Pulse.Low);
            
        }

        var handlers = _state.Where(kvp => kvp.Value != null);

        return counts[Pulse.Low] * counts[Pulse.High];
    }

    private int Capacity => Handlers.Values.Max(h => h.Capacity);
    
}