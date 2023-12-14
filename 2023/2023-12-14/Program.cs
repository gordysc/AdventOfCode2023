using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve();
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private static readonly string[] Lines = File.ReadAllLines(@"../../../Data/Input.txt");
    private static readonly long Rotations = 1_000_000_000L;

    public void Solve()
    {
        var cycled = CycleMany(Lines, Rotations);
        var grid = cycled.Select(s => s.ToCharArray()).ToArray();

        var rotated = RotateRight(grid).Select(row => new string(row)).ToArray();
        var result = CalculateLoad(rotated);

        Console.WriteLine($"Result: {result}");
    }

    private static string[] CycleMany(string[] lines, long rotations)
    {
        var data = (string[])lines.Clone();
        var cursor = string.Join("|", lines);
        var cache = new List<string>();

        while (!cache.Contains(cursor))
        {
            cache.Add(cursor);

            data = Cycle(data);
            cursor = string.Join("|", data);
        }

        var remaining = rotations - cache.Count;
        var index = cache.IndexOf(cursor);
        var cycle = cache.Slice(index, cache.Count - index);
        var spot = (int) remaining % cycle.Count;

        var element = cycle.ElementAt(spot);

        return element.Split("|");
    }

    private static string[] Cycle(string[] lines) =>
        TiltEast(TiltSouth(TiltWest(TiltNorth(lines))));

    private static string[] TiltNorth(string[] lines)
    {
        var grid = lines.Select(line => line.ToCharArray()).ToArray();
        var rotated = RotateRight(grid).Select(row => new string(row)).ToArray();
        var tilted = rotated.Select(Tilt).Select(line => line.ToCharArray()).ToArray();

        return RotateLeft(tilted).Select(row => new string(row)).ToArray();
    }

    private static string[] TiltEast(string[] lines) =>
        lines.Select(Tilt).ToArray();

    private static string[] TiltSouth(string[] lines)
    {
        var grid = lines.Select(line => line.ToCharArray()).ToArray();
        var rotated = RotateLeft(grid).Select(row => new string(row)).ToArray();
        var tilted = rotated.Select(Tilt).Select(line => line.ToCharArray()).ToArray();

        return RotateRight(tilted).Select(row => new string(row)).ToArray();
    }
    
    private static string[] TiltWest(string[] lines)
    {
        var reversed = lines.Select(line => new string(line.Reverse().ToArray())).ToArray();
        
        return reversed.Select(Tilt).Select(line => new string(line.Reverse().ToArray())).ToArray();
    }

    private static char[][] RotateRight(char[][] grid) =>
        Enumerable.Range(0, grid.Length)
            .Select(x => grid.Select(row => row[x]))
            .Select(row => row.Reverse().ToArray())
            .ToArray();
    
    private static char[][] RotateLeft(char[][] grid) =>
        Enumerable.Range(0, grid.Length)
            .Select(x => grid.Select(row => row[grid.Length - x - 1]).ToArray())
            .ToArray();

    private static int CalculateLoad(string[] lines) =>
        lines.Select(CalculateRowLoad).Sum();

    private static int CalculateRowLoad(string line) =>
        Regex.Matches(line, "[O]").Select(m => m.Index + 1).ToArray().Sum();
    
    private static string Tilt(string line) =>
        string.Join("#", line.Split("#").Select(ShiftRight).ToArray());

    private static string ShiftRight(string line) =>
        string.Join("", line.Where(c => c == 'O').ToArray()).PadLeft(line.Length, '.');
}
