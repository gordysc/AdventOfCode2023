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
    public int Evaluate(string[] lines)
    {
        var grid = BuildGrid(lines);
        var galaxies = FindGalaxies(grid);
        var distances = new List<int>();
        
        for (var i = 0; i < galaxies.Length; i++)
            for (var j = i + 1; j < galaxies.Length; j++)
                distances.Add(CalculateDistance(galaxies[i], galaxies[j]));

        return distances.Sum();
    }

    private int CalculateDistance(Galaxy start, Galaxy finish)
    {
        var height = Math.Max(start.Row, finish.Row) - Math.Min(start.Row, finish.Row);
        var width = Math.Max(start.Column, finish.Column) - Math.Min(start.Column, finish.Column);
        
        return height + width;
    }
    
    private Galaxy[] FindGalaxies(char[][] grid)
    {
        var galaxies = new List<Galaxy>();
        
        for (var i = 0; i < grid.Length; i++)
            for (var j = 0; j < grid[i].Length; j++)
                if (grid[i][j] == '#')
                    galaxies.Add(new Galaxy(i, j));

        return galaxies.ToArray();
    }
    
    private char[][] BuildGrid(string[] lines)
    {
        var height = lines.Length;
        var width = lines[0].Length;
        
        var grid = lines.Select(line => line.PadRight(width, '.')).ToArray();

        var rows = Enumerable.Range(0, height).Where(i => grid[i].All(c => c == '.'));
        grid = grid.SelectMany((line, i) => rows.Contains(i) ? new[] { line, line } : new[] { line }).ToArray();

        var columns = Enumerable.Range(0, width).Where(j => grid.All(line => line[j] == '.'));
        grid = grid.Select(line => new string(line.SelectMany((c, j) => columns.Contains(j) ? new[] { c, c } : new[] { c }).ToArray())).ToArray();

        return grid.Select(line => line.ToCharArray()).ToArray();
    }
}

internal record Galaxy(int Row, int Column);