using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input.txt");
var grid = new Grid();

foreach (var line in lines)
    grid.Execute(line);

sw.Stop();

Console.WriteLine($"Result: {grid.count}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Grid
{
    private readonly int[,] grid = new int[1000, 1000];
    public int count = 0;

    public void Execute(string line)
    {
        var instruction = ParseInstruction(line);
        var coordinates = ParseCoordinates(line);
        
        if (instruction == Instruction.TurnOn)
            TurnOn(coordinates.Item1, coordinates.Item2);
        else if (instruction == Instruction.TurnOff)
            TurnOff(coordinates.Item1, coordinates.Item2);
        else
            Toggle(coordinates.Item1, coordinates.Item2);
    }

    private (Point, Point) ParseCoordinates(string line)
    {
        var pattern = @"(\d+),(\d+) through (\d+),(\d+)";
        var regex = new Regex(pattern);
        var match = regex.Match(line);
        
        var topLeft = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        var bottomRight = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

        return (topLeft, bottomRight);
    }
    
    private Instruction ParseInstruction(string line) => line switch
    {
        _ when line.StartsWith("turn on", StringComparison.OrdinalIgnoreCase) => Instruction.TurnOn,
        _ when line.StartsWith("turn off", StringComparison.OrdinalIgnoreCase) => Instruction.TurnOff,
        _ when line.StartsWith("toggle", StringComparison.OrdinalIgnoreCase) => Instruction.Toggle,
        _ => throw new Exception("Invalid instruction")
    };
    
    private void TurnOn(Point a, Point b)
    {
        for (var x = a.X; x <= b.X; x++)
        for (var y = a.Y; y <= b.Y; y++)
        {
            count++;
            grid[x, y] += 1;
        }
    }
    
    private void TurnOff(Point a, Point b)
    {
        for (var x = a.X; x <= b.X; x++)
        for (var y = a.Y; y <= b.Y; y++)
        {
            if (grid[x, y] == 0) continue;
            count--;
            grid[x, y] -= 1;
        }
    }
    
    private void Toggle(Point a, Point b)
    {
        for (var x = a.X; x <= b.X; x++)
        for (var y = a.Y; y <= b.Y; y++)
        {
            count +=2;

            grid[x, y] += 2;
        }
    }
}

internal record Point(int X, int Y);
enum Instruction { TurnOn, TurnOff, Toggle }