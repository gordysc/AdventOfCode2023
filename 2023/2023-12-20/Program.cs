using _2023_12_20;

// var input = File.ReadAllLines(@"../../../Data/Example1.txt");
// var input = File.ReadAllLines(@"../../../Data/Example2.txt");
var input = File.ReadAllLines(@"../../../Data/Input.txt");
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

        foreach (var (_, handler) in handlers)
            foreach (var target in handler.Targets)
                if (handlers.TryGetValue(target, out var h))
                    h.AddSource(handler.Label);

        var network = new Network(broadcaster, handlers);

        while (!network.IsComplete)
            network.PressButton();

        Console.WriteLine($"Answer: {network.TotalPressesRequired}");
    }


    private static Broadcaster BuildBroadcaster(IEnumerable<string> input) =>
        new(input.First(l => l.StartsWith(Broadcaster))
            .Split(" -> ")[1].Trim()
            .Split(",").Select(l => l.Trim())
            .ToArray());

    private static IDictionary<string, IPulseModule> BuildHandlers(IEnumerable<string> input) =>
        input.Where(l => !l.StartsWith(Broadcaster))
            .Select(BuildHandler)
            .ToDictionary();

    private static (string, IPulseModule) BuildHandler(string line)
    {
        var parts = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToArray();
        
        var targets = parts[1].Split(",")
            .Select(item => item.Trim()).ToArray();

        var module = parts[0][..1];
        var label = parts[0][1..];

        return module switch
        {
            FlipFlop => (label, new FlipFlopModule(label, targets)),
            Inverter => (label, new InverterModule(label, targets)),
            _ => throw new InvalidDataException($"Unknown module: {module}")
        };
    }
}
