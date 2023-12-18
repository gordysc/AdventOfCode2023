using System.Text.RegularExpressions;

// var input = File.ReadAllLines(@"../../../Data/Example.txt");
var input = File.ReadAllLines(@"../../../Data/Input.txt");
var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve(input);
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    public void Solve(IEnumerable<string> input)
    {
        var vertices = BuildVertices(input);
        var area = CalculateArea(vertices);
        var perimeter = CalculatePerimeter(vertices);

        var interior = area - perimeter / 2 + 1;
        var answer = perimeter + interior;
        
        Console.WriteLine($"Answer: {answer}");
    }

    private static readonly char[] Directions = ['R', 'D', 'L', 'U'];

    private static (long, long)[] BuildVertices(IEnumerable<string> input)
    {
        long x = 0, y = 0;
        
        List<(long, long)> coordinates = [(x, y)];

        foreach (var line in input)
        {
            var last = line.Split(" ").Last();
            var sanitized = Regex.Replace(last, @"[#()]", "");

            var hex = sanitized[..^1];
            var direction = Directions[int.Parse(sanitized.Last().ToString())];

            var delta = Convert.ToInt64(hex, 16);

            x += direction == 'R' ? delta : direction == 'L' ? -1 * delta : 0;
            y += direction == 'U' ? delta : direction == 'D' ? -1 * delta : 0;
            
            coordinates.Add((x, y));
        }

        return coordinates.ToArray();
    }

    private static long CalculatePerimeter(IReadOnlyList<(long, long)> vertices) =>
        Enumerable.Range(1, vertices.Count - 1)
            .Aggregate(0L, (acc, loop) =>
                acc +
                Math.Abs(vertices[loop - 1].Item1 - vertices[loop].Item1) +
                Math.Abs(vertices[loop - 1].Item2 - vertices[loop].Item2)
            );
    
    private static long CalculateArea(IReadOnlyList<(long, long)> vertices)
    {
        var total = 0L;

        for (var loop = 0; loop < vertices.Count; loop++) {
            var (x1, y1) = vertices[loop];
            var (x2, y2) = vertices[(loop + 1) % vertices.Count];

            total += x1 * y2 - y1 * x2;
        }

        return Math.Abs(total / 2);
    }
}