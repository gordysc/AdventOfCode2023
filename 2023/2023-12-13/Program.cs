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

    private static int FindReflection(string[] rows)
    {
        var index = FindIndex(rows);

        if (index > -1) return index * 100;

        var columns = Enumerable.Range(0, rows[0].Length)
            .Select(c => string.Join("", rows.Select(row => row[c])))
            .ToArray();

        return FindIndex(columns);
    }

    private static int FindIndex(string[] rows)
    {
        for (var loop = 1; loop < rows.Length; loop++)
            if (IsReflection(rows, loop))
                return loop;

        return -1;
    }

    private static bool IsReflection(string[] rows, int index)
    {
        var left = rows[..index];
        var right = rows[index..];

        if (left.Length > right.Length)
            return IsSmudged(left.Reverse().Take(right.Length).ToArray(), right);

        if (right.Length > left.Length)
            return IsSmudged(right.Take(left.Length).Reverse().ToArray(), left);

        return IsSmudged(left.Reverse().ToArray(), right);
    }

    private static bool IsSmudged(string[] left, string[] right) =>
        left.Select((l, idx) => Smudges(l, right[idx])).Sum() == 1;

    private static int Smudges(string left, string right) =>
        Enumerable.Range(0, left.Length).Count(idx => left[idx] != right[idx]);

    private static List<string[]> CreateGrids()
    {
        var grids = new List<string[]>();
        var rows = new List<string>();

        foreach (var line in Lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                grids.Add(rows.ToArray());
                rows.Clear();
                continue;
            }

            rows.Add(line);
        }

        grids.Add(rows.ToArray());

        return grids;
    }
}
