var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();

// var input = File.ReadAllLines(@"../../../Data/Example.txt");
var input = File.ReadAllLines(@"../../../Data/Input.txt");
var grid = input.Select(l => l.ToCharArray()).ToArray();

solution.Solve(grid);

sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution()
{
    private const int StepCount = 64;
    private static readonly (int, int)[] Modifiers = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    public void Solve(char[][] grid)
    {
        var start = FindStart(grid);
        var queue = new Queue<(int, int)>();

        queue.Enqueue(start);

        for (var loop = 0; loop < StepCount; loop++)
        {
            var positions = new HashSet<(int, int)>();

            while (queue.Count > 0)
            {
                var (oRow, oCol) = queue.Dequeue();

                foreach (var (deltaY, deltaX) in Modifiers)
                {
                    var row = oRow + deltaY;
                    var col = oCol + deltaX;

                    if (row < 0 || row >= grid.Length || col < 0 || col >= grid[row].Length)
                        continue;

                    if (grid[row][col] != '#')
                        positions.Add((row, col));
                }
            }

            foreach (var position in positions)
                queue.Enqueue(position);
        }

        var answer = queue.Count;

        Console.WriteLine($"Answer: {answer}");
    }

    private static (int, int) FindStart(char[][] grid)
    {
        for (var row = 0; row < grid.Length; row++)
            for (var col = 0; col < grid[row].Length; col++)
                if (grid[row][col] == 'S')
                    return (row, col);

        throw new Exception("No start found");
    }
}
