var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve();
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private static readonly string[] Lines = File.ReadAllLines(@"../../../Data/Input.txt");
    private static readonly char[][] Grid = Lines.Select(x => x.ToCharArray()).ToArray();
    
    private static readonly int Height = Grid.Length;
    private static readonly int Width = Grid[0].Length;

    public void Solve()
    {
        var top = Enumerable.Range(0, Width).Select(x => ((x, 0), 'D')).ToArray();
        var bottom = Enumerable.Range(0, Width).Select(x => ((x, Height - 1), 'U')).ToArray();
        var left = Enumerable.Range(0, Height).Select(y => ((0, y), 'R')).ToArray();
        var right = Enumerable.Range(0, Height).Select(y => ((Width - 1, y), 'L')).ToArray();
        
        var vectors = top.Concat(bottom).Concat(left).Concat(right).ToArray();
        var answer = vectors.AsParallel().Select(FindEnergy).Max();
        
        Console.WriteLine(answer);
    }
    private int FindEnergy(((int, int), char) start)
    {
        var queue = new Queue<((int, int), char)>();
        var visited = new HashSet<(int, int)>();
        var energized = new HashSet<((int, int), char)>();

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var ((x, y), direction) = queue.Dequeue();
            
            if (IsTerminated(x, y)) continue;
            if (energized.Contains(((x, y), direction))) continue;

            visited.Add((x, y));
            energized.Add(((x, y), direction));

            foreach (var beam in Move(x, y, direction))
                queue.Enqueue(beam);
        }

        return visited.Count;
    }
    
    private static IEnumerable<((int, int), char)> Move(int x, int y, char direction) =>
        direction switch
        {
            'R' => Right(x, y),
            'L' => Left(x, y),
            'U' => Up(x, y),
            'D' => Down(x, y),
            _ => throw new Exception("Invalid direction")
        };

    private static IList<((int, int), char)> Right(int x, int y) =>
        Grid[y][x] switch {
            '/' => [((x, y - 1), 'U')],
            '\\' => [((x, y + 1), 'D')],
            '|' => [((x, y - 1), 'U'), ((x, y + 1), 'D')],
            _ => new List<((int, int), char)> {((x + 1, y), 'R')}
        };

    private static IList<((int, int), char)> Left(int x, int y) =>
        Grid[y][x] switch {
            '/' => [((x, y + 1), 'D')],
            '\\' => [((x, y - 1), 'U')],
            '|' => [((x, y - 1), 'U'), ((x, y + 1), 'D')],
            _ => new List<((int, int), char)> {((x - 1, y), 'L')}
        };
    
    private static IList<((int, int), char)> Up(int x, int y) =>
        Grid[y][x] switch {
            '/' => [((x + 1, y), 'R')],
            '\\' => [((x - 1, y), 'L')],
            '-' => [((x - 1, y), 'L'), ((x + 1, y), 'R')],
            _ => new List<((int, int), char)> {((x, y - 1), 'U')}
        };
    
    private static IList<((int, int), char)> Down(int x, int y) =>
        Grid[y][x] switch {
            '/' => [((x - 1, y), 'L')],
            '\\' => [((x + 1, y), 'R')],
            '-' => [((x - 1, y), 'L'), ((x + 1, y), 'R')],
            _ => new List<((int, int), char)> {((x, y + 1), 'D')}
        };

    private static bool IsTerminated(int x, int y) =>
        x < 0 || x > (Width - 1) || y < 0 || y > (Height - 1);
}
