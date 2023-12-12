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
    private readonly Dictionary<int, string[]> _permutations = new();
    public void Solve()
    {
        var total = 0;

        foreach (var line in Input)
            total += Arrangements(line);
        
        Console.WriteLine($"Total: {total}");
    }
    
    private int Arrangements(string line)
    {
        var (input, counts) = Parse(line);

        var chars = input.ToCharArray();
        var unknowns = Regex.Matches(input, @"[?]").Select(m => m.Index).ToArray();
        var permutations = GeneratePermutations(chars, 0, unknowns).ToHashSet();

        var total = 0;
        
        foreach(var permutation in permutations)
        {
            var segments = permutation.Split(".", StringSplitOptions.RemoveEmptyEntries);
            
            if (segments.Length != counts.Length)
                continue;
            
            if (Enumerable.Range(0, segments.Length).All(loop => segments[loop].Length == counts[loop]))
                total++;
        }
        
        return total;
    }

    private static string[] GeneratePermutations(char[] chars, int index, int[] unknowns)
    {
        if (index == chars.Length)
            return new string[] { new string(chars) };
        
        if (!unknowns.Contains(index))
            return GeneratePermutations(chars, index + 1, unknowns);

        var copy = (char[])chars.Clone();
        
        copy[index] = '.';
        var operational = GeneratePermutations(copy, index + 1, unknowns);

        copy[index] = '#';
        var damaged = GeneratePermutations(copy, index + 1, unknowns);
        
        return operational.Concat(damaged).ToArray();
    }

    private static (string, int[]) Parse(string line)
    {
        var split = line.Split(" ");
        var counts = split[1].Split(",").Select(int.Parse).ToArray();
        
        return (Regex.Replace(split[0], @"[\.]+", "."), counts);
    }
}
