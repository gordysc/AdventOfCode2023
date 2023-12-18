var input = File.ReadAllLines(@"../../../Data/Input.txt");
var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve(input);
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution()
{
    private static readonly Modifier[] Modifiers =
    [
        new Modifier('U', 0, -1),
        new Modifier('R', 1, 0),
        new Modifier('D', 0, 1),
        new Modifier('L', -1, 0),
    ];

    private static readonly int MaxRepeatFactor = 3;
    private static readonly char Idle = 'X';

    public void Solve(string[] input)
    {
        var grid = CreateGrid(input);
        var cache = new Dictionary<string, int>();
        var queue = new Queue<Path>();
        
        var height = grid.Length;
        var width = grid[0].Length;
        
        queue.Enqueue(new Path(0, 0, 0, Idle, 0));

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();

            // See if we've been here before w/ the same direction & counter
            // If we have check if we've found a faster route, if so bail.
            if (cache.TryGetValue(path.Key, out var value))
                if (value <= path.Total)
                    continue;

            cache[path.Key] = path.Total;

            foreach (var (direction, deltaX, deltaY) in Modifiers)
            {
                // Cannot go in reverse
                if (path.IsReverseDirection(direction))
                    continue;
                
                var counter = direction == path.Direction ? path.Counter + 1 : 1;
                
                // Cannot move same direction multiple times
                if (counter > MaxRepeatFactor)
                    continue;
                
                // Check we're still in the grid's boundaries
                var row = path.Row + deltaY;
                var column = path.Column + deltaX;
                
                if (row < 0 || row >= height || column < 0 || column >= width)
                    continue;
                
                var total = path.Total + grid[row][column];
                
                queue.Enqueue(new Path(total, row, column, direction, counter));
            }   
        }

        var answer = cache.Keys.Select(key =>
        {
            var segments = key.Split(":");
            var row = int.Parse(segments[0]);
            var column = int.Parse(segments[1]);

            if (row != height - 1) return int.MaxValue;
            if (column != width - 1) return int.MaxValue;

            return cache[key];
        }).Min();

        Console.WriteLine($"Answer: {answer}");
    }

    private static int[][] CreateGrid(string[] input) =>
        input.Select(r => r.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
}

internal record Path(int Total, int Row, int Column, char Direction, int Counter)
{
    public string Key => $"{Row}:{Column}:{Direction}:{Counter}";
    public bool IsReverseDirection(char direction) => direction switch
    {
        'R' => Direction == 'L',
        'L' => Direction == 'R',
        'U' => Direction == 'D',
        'D' => Direction == 'U',
        _ => false
    };
}
internal record Modifier(char Direction, int X, int Y);