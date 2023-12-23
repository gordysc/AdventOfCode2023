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
    private static readonly Modifier[] Modifiers =
    {
        new Modifier('R', 0, 1),
        new Modifier('D', 1, 0),
        new Modifier('L', 0, -1),
        new Modifier('U', -1, 0)
    };

    public void Solve(char[][] grid)
    {
        var answer = 0;
        var finish = (grid.Length - 1, grid[0].Length - 2); 
        var queue = new Queue<Path>([new Path(0, 1, [(0, 1)])]);

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            
            foreach (var mod in Modifiers)
            {
                var row = path.Row + mod.Row;
                var col = path.Col + mod.Col;

                if (row < 0 || row >= grid.Length || col < 0 || col >= grid[0].Length)
                    continue;
                
                var element = grid[row][col];

                if (element == '#')
                    continue;

                if (mod.Direction == 'R' && element == '<')
                    continue;
                if (mod.Direction == 'L' && element == '>')
                    continue;
                if (mod.Direction == 'D' && element == '^')
                    continue;
                if (mod.Direction == 'U' && element == 'v')
                    continue;
                    
                if (path.Traveled.Contains((row, col)))
                    continue;

                if ((row, col) == finish)
                {
                    answer = Math.Max(answer, path.Traveled.Count);
                    continue;
                }

                var traveled = new HashSet<(int, int)>(path.Traveled) { (row, col) };

                queue.Enqueue(new Path(row, col, traveled));
            }
        }
        
        Console.WriteLine($"Answer: {answer}");
    }
}

internal record Path(int Row, int Col, HashSet<(int, int)> Traveled);
internal record Modifier(char Direction, int Row, int Col);