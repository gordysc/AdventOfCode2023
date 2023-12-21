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
    private const long StepCount = 26_501_365;
    private const long GridLength = 131L;
    private static readonly (int, int)[] Modifiers = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    public void Solve(char[][] grid)
    {
        // The starting point is in the very middle (65, 65)
        // There are no rocks vertically/horizontally between the starting point and edge.
        // This means it takes 65 steps to get to the edge of the first grid
        // If we keep moving straight it'll take us 131 additional steps to get to the far edge of the next grid

        // f(x) = 65 + 131 * x
        // 65 + 131 * 0 => 3744 positions
        // 65 + 131 * 1 => 33417 positions
        // 65 + 131 * 2 = 327 steps => 92680 positions
        // 65 + 131 * 3 = 458 steps => 181533 positions

        // y = ax^2 + bx + c
        // https://www.wolframalpha.com/input?i=quadratic+fit+calculator
        // # of positions = 14795 x^2 + 14878 x + 3744

        // 26501365 steps => (26501365 - 65) / 131 = 202300
        // 14795 * 202300^2 + 14878 * 202300 + 3744 = 605,492,675,373,144

        // y0 = ax0^2 + bx0 + c
        var zero = CalculatePositions(grid, 65 + 131 * 0);
        // y1 = ax1^2 + bx1 + c
        var one = CalculatePositions(grid, 65 + 131 * 1);
        // y2 = ax2^2 + bx2 + c
        var two = CalculatePositions(grid, 65 + 131 * 2);

        // y0/2 - y1 + y2/2
        var a = zero / 2 - one + two / 2;
        // -3y0/2 + 2y1 - y2/2
        var b = -3 * zero / 2 + 2 * one - two / 2;
        // y0
        var c = zero;

        var x = (StepCount - 65) / GridLength;

        var answer = a * x * x + b * x + c;

        Console.WriteLine($"Answer: {answer}");
    }

    private static long CalculatePositions(char[][] grid, int steps)
    {
        var start = FindStart(grid);
        var queue = new Queue<(int, int)>();
        var height = grid.Length;
        var width = grid[0].Length;

        queue.Enqueue(start);

        for (var loop = 0; loop < steps; loop++)
        {
            var positions = new HashSet<(int, int)>();

            while (queue.Count > 0)
            {
                var (oRow, oCol) = queue.Dequeue();

                foreach (var (deltaY, deltaX) in Modifiers)
                {
                    var row = oRow + deltaY;
                    var col = oCol + deltaX;

                    var cRow = (height + (oRow + deltaY) % height) % height;
                    var cCol = (width + (oCol + deltaX) % width) % width;

                    if (grid[cRow][cCol] != '#')
                        positions.Add((row, col));
                }
            }

            foreach (var position in positions)
                queue.Enqueue(position);
        }

        return queue.Count;
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
