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
    public int Evaluate(string text)
    {
        var cursor = (0, 0);
        var points = new HashSet<(int, int)> { (0, 0) };

        foreach (char c in text)
        {
            var x = cursor.Item1 + c switch { '>' => 1, '<' => -1, _ => 0 };
            var y = cursor.Item2 + c switch { '^' => 1, 'v' => -1, _ => 0 };

            points.Add((x, y));
            cursor = (x, y);
        }

        return points.Count();
    }
}