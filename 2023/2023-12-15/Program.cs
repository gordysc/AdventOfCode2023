var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve();
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private static readonly string Data = File.ReadAllText(@"../../../Data/Input.txt");
    // private static readonly string Data = File.ReadAllText(@"../../../Data/Example.txt");
    private static readonly string[] Steps = Data.Split(",", StringSplitOptions.RemoveEmptyEntries);

    private const string Remove = "-";
    private const string Add = "=";

    private static readonly string[] Operations = new[] { Remove, Add };

    public void Solve()
    {
        var dict = new Dictionary<int, List<(string, int)>>();
        
        for (var loop = 0; loop < 256; loop++)
            dict.Add(loop, []);
        
        foreach (var step in Steps)
        {
            var parts = step.Split(Operations, StringSplitOptions.RemoveEmptyEntries);

            var label = parts[0];
            var box = Hash(label);

            var index = dict[box].FindIndex(e => e.Item1 == label);

            if (step.Contains(Remove) && index >= 0)
                dict[box].RemoveAt(index);
            else if (step.Contains(Add) && index >= 0) 
                dict[box][index] = (label, int.Parse(parts[1]));
            else if (step.Contains(Add))
                dict[box].Add((label, int.Parse(parts[1])));
        }

        var answer = dict.Keys.Aggregate(0, (acc, key) =>
            acc + dict[key].Select((kvp, index) => (key + 1) * (index + 1) * kvp.Item2).Sum()
        );

        Console.WriteLine($"Answer: {answer}");
    }

    private static int Hash(string sequence) =>
        sequence.Aggregate(0, (acc, c) => ((acc + c) * 17) % 256);
}