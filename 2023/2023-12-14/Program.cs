using System.Text;
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
    private static readonly char[][] Grid = Lines.Select(l => l.ToCharArray()).ToArray();
    
    private static readonly int Height = Grid.Length;
    private static readonly int Width = Grid[0].Length;

    public void Solve()
    {
        var result = TiltNorth();
        
        Console.WriteLine($"Result: {result}");
    }

    private static int TiltNorth()
    {
        var rotated = Enumerable.Range(0, Height)
            .Select(x => string.Join("", Grid.Select(row => row[x])))
            .Select(l => string.Join("", l.ToCharArray().Reverse()))
            .ToArray();
        
        return CalculateLoad(rotated);
    }

    private static int CalculateLoad(string[] lines) =>
        lines.Select(Tilt).Select(CalculateRowLoad).Sum();

    private static int CalculateRowLoad(string line) =>
        Regex.Matches(line, "[O]").Select(m => m.Index + 1).ToArray().Sum();

    private static string Tilt(string line) =>
        string.Join("#", line.Split("#").Select(ShiftRight).ToArray());

    private static string ShiftRight(string line) =>
        string.Join("", line.Where(c => c == 'O').ToArray()).PadLeft(line.Length, '.');
}