var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
// var lines = File.ReadAllLines(@"../../../Input/Example.txt");
var evaluator = new Evaluator();

var result = evaluator.Evaluate(lines);

sw.Stop();

Console.WriteLine($"Result: {result}");
Console.WriteLine($"Total time: {sw.Elapsed.TotalMilliseconds} ms");

internal class Evaluator()
{
    public long Evaluate(string[] lines)
    {
        var grid = lines.Select(line => line.ToCharArray()).ToArray();
        var galaxies = FindGalaxies(grid);
        
        var rows = FindRows(lines);
        var columns = FindColumns(lines);

        var total = 0L;
        
        for (var i = 0; i < galaxies.Length; i++)
        for (var j = i + 1; j < galaxies.Length; j++)
        {
            var start = galaxies[i];
            var finish = galaxies[j];

            var height = Calculate(start.Row, finish.Row, rows);
            var width = Calculate(start.Column, finish.Column, columns);

            total += height + width;
        }

        return total;
    }

    private static int Calculate(int a, int b, int[] blanks)
    {
        var (max, min) = (Math.Max(a, b), Math.Min(a, b));
        var count = blanks.Count(c => c > min && c < max);
        
        return max - min + count * (1_000_000 - 1);
    }

    private static Galaxy[] FindGalaxies(char[][] grid)
    {
        var galaxies = new List<Galaxy>();
        
        for (var i = 0; i < grid.Length; i++)
            for (var j = 0; j < grid[i].Length; j++)
                if (grid[i][j] == '#')
                    galaxies.Add(new Galaxy(i, j));

        return galaxies.ToArray();
    }

    private static int[] FindRows(string[] lines) =>
        Enumerable.Range(0, lines.Length)
            .Where(loop => lines[loop].All(c => c == '.'))
            .ToArray();

    private static int[] FindColumns(string[] lines) =>
        Enumerable.Range(0, lines[0].Length)
            .Where(loop => lines.All(line => line[loop] == '.'))
            .ToArray();
}

internal record Galaxy(int Row, int Column);