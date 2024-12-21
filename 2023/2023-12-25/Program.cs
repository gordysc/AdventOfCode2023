using QuikGraph;
using QuikGraph.Algorithms;

var input = File.ReadAllLines("Data/Input.txt");
var graph = new UndirectedGraph<string, Edge<string>>();

foreach (var line in input)
{
    var parts = line.Split(':');
    var node = parts[0];

    graph.AddVertex(node);

    var connections = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    
    foreach (var connection in connections)
    {
        graph.AddVertex(connection);
        graph.AddEdge(new Edge<string>(node, connection));
    }
}

var distances = new Dictionary<Edge<string>, double>();
var edges = graph.Edges.ToList();

foreach (var edge in edges)
{
    graph.RemoveEdge(edge);
    
    var calculator = graph.ShortestPathsDijkstra(_ => 1, edge.Source);

    if (calculator(edge.Target, out var path))
        distances[edge] = path.Count();
    else
        distances[edge] = double.PositiveInfinity;

    graph.AddEdge(edge);
}

var longest = distances.OrderByDescending(x => x.Value).Take(3);

Console.WriteLine("Removing the following edges:");

foreach (var (edge, _) in longest)
{
    graph.RemoveEdge(edge);
    
    Console.WriteLine($"{edge.Source} <-> {edge.Target}");
}

var components = new Dictionary<string, int>();

graph.ConnectedComponents(components);

var values = components.Values.Distinct();
var answer = values.Aggregate(1, (acc, value) =>
{
    var size = components.Count(x => x.Value == value);
    
    Console.WriteLine($"Partition #{value}: {size} nodes");
    
    return acc * components.Count(x => x.Value == value);
});

Console.WriteLine($"Answer: {answer}");
 