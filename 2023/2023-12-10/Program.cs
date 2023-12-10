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
    public int Evaluate((int, int) position)
    {
        var route = BuildRoute(position);
        var vertices = FindVertices(route);
        
        // Check if starting node is a vertex
        var start = new Node(position, 'S');
        var neighbors = GetNeighbors(start);
        if (neighbors.Any(n => "7LFJ".Contains(n.Value))) vertices.Add(start);
        
        // Shoelace Theorem (finds area of polygon)
        // https://en.wikipedia.org/wiki/Shoelace_formula
        var area = CalculateArea(vertices.ToArray());
        
        // Pick's Theorem (Area = I + B/2 - 1)
        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        return area + 1 - (route.Length / 2);
    }
    
    private int CalculateArea(Node[] vertices) {
        var mod = vertices.Length;

        var area = 0;

        for (var loop = 0; loop < vertices.Length; loop++) {
            var (x1, y1) = vertices[loop].Position;
            var (x2, y2) = vertices[(loop + 1) % mod].Position;

            area += x1 * y2 - y1 * x2;
        }

        return area / 2;
    }

    private List<Node> FindVertices(Node[] nodes) => 
        nodes.Where(n => n.Value != '-' && n.Value != '|').ToList();

    private Node[] BuildRoute((int, int) position)
    {
        var start = new Node(position, 'S');

        var visited = new HashSet<Node>();
        var nodes = new List<Node> { start };

        var node = GetNeighbors(start).First();

        while (node is not null)
        {
            visited.Add(node);
            nodes.Add(node);
            
            node = GetNeighbors(node).FirstOrDefault(n => !visited.Contains(n));
        }

        return nodes.ToArray();
    }
    
    private Node[] GetNeighbors(Node node)
    {
        var (row, col) = node.Position;
        var validator = new PipeValidator(node.Value);

        var neighbors = new List<Node>();
        
        if (row > 0 && validator.IsValidTop(Map[row - 1][col]))
            neighbors.Add(new Node((row - 1, col), Map[row - 1][col]));
        
        if (row + 1 <= Map.Length -1 && validator.IsValidBottom(Map[row + 1][col]))
            neighbors.Add(new Node((row + 1, col), Map[row + 1][col]));
        
        if (col > 0 && validator.IsValidLeft(Map[row][col - 1]))
            neighbors.Add(new Node((row, col - 1), Map[row][col - 1]));
        
        if (col + 1 <= Map[row].Length - 1 && validator.IsValidRight(Map[row][col + 1]))
            neighbors.Add(new Node((row, col + 1), Map[row][col + 1]));
        
        return neighbors.ToArray();
    }
}

internal record Node((int, int) Position, char Value);
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