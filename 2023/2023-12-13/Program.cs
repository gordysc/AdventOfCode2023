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
    public void Solve()
    {
        var grids = CreateGrids();
        var total = grids.Select(FindReflection).Sum();

        Console.WriteLine($"Total: {total}");
    }

    private static int FindReflection(byte[][] grid)
    {
        var height = grid.Length;
        
        if (height % 2 == 0 && grid[height / 2].SequenceEqual()) {} 

        return 0;
    }

    private static bool IsReflection(byte[][] grid)
    {
        var height = grid.Length;
        
        if (height % 2 == 0)
        {
            var middle = height / 2;
            
        }
    }
        

    private static List<byte[][]> CreateGrids()
    {
        var grids = new List<byte[][]>();
        var rows = new List<byte[]>();

        foreach (var line in Lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                grids.Add(rows.ToArray());
                rows.Clear();
                continue;
            }

            rows.Add(line.Select(c => c == '#' ? (byte)1 : (byte)0).ToArray());
        }

        return grids;
    }
}