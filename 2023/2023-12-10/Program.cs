var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var builder = new List<char[]>();
var start = (-1, -1);

for (var loop = 0; loop < lines.Length; loop++)
{
    var line = lines[loop];
    builder.Add(line.ToCharArray());

    var index = (line.IndexOf('S'));
    if (index >= 0) start = (loop, index);
}

var map = builder.ToArray();
var evaluator = new Evaluator(map);

var result = evaluator.Evaluate(start);

sw.Stop();

Console.WriteLine($"Result: {result}");
Console.WriteLine($"Total time: {sw.Elapsed.TotalMilliseconds} ms");

internal class Evaluator(char[][] Map)
{
    public int Evaluate((int, int) start)
    {
        var visited = new Dictionary<(int, int), bool>();
        var nodes = new HashSet<Node>();
        var queue = new Queue<Node>();

        queue.Enqueue(new Node(start, 'S', 0));
        
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            var position = node.Position;
            
            if (!visited.TryAdd(position, true)) continue;

            nodes.Add(node);
            
            foreach(var neighbor in GetNeighbors(node))
                queue.Enqueue(neighbor);
        }

        return nodes.Max(n => n.Distance);
    }
    
    private Node[] GetNeighbors(Node node)
    {
        var (row, col) = node.Position;
        var validator = new PipeValidator(node.Value);

        var neighbors = new List<Node>();
        
        if (row > 0 && validator.IsValidTop(Map[row - 1][col]))
            neighbors.Add(new Node((row - 1, col), Map[row - 1][col], node.Distance + 1));
        
        if (row + 1 <= Map.Length -1 && validator.IsValidBottom(Map[row + 1][col]))
            neighbors.Add(new Node((row + 1, col), Map[row + 1][col], node.Distance + 1));
        
        if (col > 0 && validator.IsValidLeft(Map[row][col - 1]))
            neighbors.Add(new Node((row, col - 1), Map[row][col - 1], node.Distance + 1));
        
        if (col + 1 <= Map[row].Length - 1 && validator.IsValidRight(Map[row][col + 1]))
            neighbors.Add(new Node((row, col + 1), Map[row][col + 1], node.Distance + 1));
        
        return neighbors.ToArray();
    }
}

internal record Node((int, int) Position, char Value, int Distance);
internal record PipeValidator(char Value)
{
    public bool IsValidLeft(char left) =>
        "S-J7".Contains(Value) && "-LF".Contains(left);
    
    public bool IsValidRight(char right) =>
        "S-LF".Contains(Value) && "-J7".Contains(right);
    
    public bool IsValidTop(char top) =>
        "S|JL".Contains(Value) && "|F7".Contains(top);

    public bool IsValidBottom(char bottom) =>
        "S|F7".Contains(Value) && "|JL".Contains(bottom);
}