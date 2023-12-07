var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var text = File.ReadAllText(@"../../../Input.txt");

var evaluator = new Evaluator();
var houses = evaluator.Evaluate(text);

sw.Stop();

Console.WriteLine($"Houses: {houses}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Evaluator
{
    private readonly HashSet<(int, int)> _points = [(0, 0)];
    public int Evaluate(string text)
    {
        var santa = (0, 0);
        var robot = (0, 0);

        foreach (var values in text.Chunk(2).ToArray())
        {
            var c = values[0];
            
            santa = Move(santa, c);
            robot = values.Length == 2 ? Move(robot, values[1]) : robot;

            _points.Add(santa);
            _points.Add(robot);
        }

        return _points.Count();
    }

    public (int, int) Move((int, int) cursor, char c)
    {
        var x = cursor.Item1 + c switch { '>' => 1, '<' => -1, _ => 0 };
        var y = cursor.Item2 + c switch { '^' => 1, 'v' => -1, _ => 0 };
        
        return (x, y);
    }
}