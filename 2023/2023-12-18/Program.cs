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
        var perimeter = CalculatePerimeter(vertices);
        var area = CalculateArea(vertices);
        var interior = area - perimeter / 2 + 1;

        var answer = perimeter + interior;
        
        Console.WriteLine($"Answer: {answer}");
    }

    private static (int, int)[] BuildVertices(IEnumerable<string> input)
    {
        int x = 0, y = 0;
        
        List<(int, int)> coordinates = [(x, y)];

        foreach (var line in input)
        {
            var parts = line.Split(" ");
            var delta = int.Parse(parts[1]);

            x += parts[0] == "R" ? delta : parts[0] == "L" ? -1 * delta : 0;
            y += parts[0] == "U" ? delta : parts[0] == "D" ? -1 * delta : 0;
            
            coordinates.Add((x, y));
        }

        return coordinates.ToArray();
    }

    private static int CalculatePerimeter(IReadOnlyList<(int, int)> vertices) =>
        Enumerable.Range(1, vertices.Count - 1)
            .Aggregate(0, (acc, loop) =>
                acc +
                Math.Abs(vertices[loop - 1].Item1 - vertices[loop].Item1) +
                Math.Abs(vertices[loop - 1].Item2 - vertices[loop].Item2)
            );
    
    private static int CalculateArea(IReadOnlyList<(int, int)> vertices) {
        var mod = vertices.Count;

        var area = 0;

        for (var loop = 0; loop < vertices.Count; loop++) {
            var (x1, y1) = vertices[loop];
            var (x2, y2) = vertices[(loop + 1) % mod];

            area += x1 * y2 - y1 * x2;
        }

        return Math.Abs(area / 2);
    }
}