using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve();
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private static readonly string[] Input = File.ReadAllLines(@"../../../Data/Input.txt");
    private readonly Dictionary<string, long> _cache = new();

    public void Solve()
    {
        var total = Input.Aggregate(0L, (acc, line) => CalculateArrangements(line) + acc);

        Console.WriteLine($"Total: {total}");
    }

    private const int FoldingFactor = 5;
    
    private long CalculateArrangements(string line)
    {
        var split = line.Split(" ");
        var data = Regex.Replace(split[0], @"[\.]+", ".");
        var numbers = split[1].Split(",").Select(int.Parse).ToArray();

        var input = string.Join("?", Enumerable.Repeat(data, FoldingFactor));
        var counts = Enumerable.Repeat(numbers, FoldingFactor).SelectMany(n => n).ToArray();

        return Calculate(input, counts);
    }

    private long Calculate(string input, int[] counts)
    {
        var key = $"[{input}|{string.Join(":", counts)}";

        if (_cache.TryGetValue(key, out var result))
            return result;

        result = CalculateInternal(input, counts);

        _cache.Add(key, result);

        return result;
    }

    private long CalculateInternal(string input, int[] counts)
    {
        // Check if there are still damaged springs
        if (counts.Length == 0)
            return input.Contains('#') ? 0L : 1L;

        // No matches if there are more count blocks than characters
        if (input.Length < counts.Length)
            return 0L;

        // No matches if the input length is less than the first count
        if (input.Length < counts[0])
            return 0L;

        // Skip all the operational springs and continue calculating
        if (input.StartsWith("."))
            return Calculate(input.TrimStart('.'), counts);

        // Calculate both possibilities
        if (input.StartsWith("?"))
            return Calculate('#' + input[1..], counts) + Calculate('.' + input[1..], counts);

        // No matches if the first block length is less than the first count
        if (input.Contains('.') && input.IndexOf('.') + 1 <= counts[0])
            return 0L;

        // No matches if there isn't a potential . after the first damaged spring block
        if (input.Length > counts[0] && input[counts[0]] == '#')
            return 0L;

        var skip = input.Length == counts[0] ? counts[0] : counts[0] + 1;

        return Calculate(input[skip..], counts[1..]);
    }
}
