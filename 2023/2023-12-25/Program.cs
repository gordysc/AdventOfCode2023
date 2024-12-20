var input = File.ReadAllLines("Data/Input.txt");

var graph = new Graph();

foreach (var line in input)
{
    var parts = line.Split(':');
    var node = parts[0];

    var connections = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    
    foreach (var connection in connections)
    {
        graph.AddEdge(node, connection);
    }
}

var distances = new List<(Edge Edge, int Distance)>();
var numberOfEdges = graph.Edges.Count;

Console.WriteLine($"Found {numberOfEdges} edges");

for (var loop = 0; loop < numberOfEdges; loop++)
{
    if (loop % 100 == 0 && loop > 0)
        Console.WriteLine($"Processing edge {loop:N0}");
    
    var edge = graph.Edges[loop];
    var copy = new Graph(graph);
    
    copy.RemoveEdge(edge);
    
    var distance = copy.FindShortestPath(edge.Node1, edge.Node2);
    
    distances.Add((edge, distance));
}

var longest = distances.OrderByDescending(d => d.Distance).Take(3).ToArray();

foreach (var entry in longest)
    graph.RemoveEdge(entry.Edge);

var partitions = graph.GetPartitions();

Console.WriteLine($"Found {partitions.Count} partitions");

var result = partitions.Aggregate(1, (acc, partition) => acc * partition.Count);

internal sealed class Graph
{
    private readonly HashSet<string> _nodes = [];
    private readonly List<Edge> _edges = [];
    public IReadOnlyList<Edge> Edges => _edges;
    public IReadOnlySet<string> Nodes => _nodes;
    
    public Graph() { }

    public Graph(Graph original)
    {
        _edges = [..original._edges];
    }

    public List<List<string>> GetPartitions()
    {
        var partitions = new List<List<string>>();
        var visited = new HashSet<string>();
        
        foreach (var node in _nodes)
        {
            if (visited.Contains(node))
                continue;
            
            var partition = new List<string>();
            var queue = new Queue<string>();
            
            queue.Enqueue(node);
            
            while (queue.TryDequeue(out var current))
            {
                if (!visited.Add(current))
                    continue;
                
                partition.Add(current);
                
                foreach (var edge in _edges.Where(e => e.Contains(current)))
                {
                    var nextNode = edge.Node1 == current ? edge.Node2 : edge.Node1;
                    
                    if (!visited.Contains(nextNode))
                        queue.Enqueue(nextNode);
                }
            }
            
            partitions.Add(partition);
        }

        return partitions;
    }

    public int FindShortestPath(string start, string finish)
    {
        var fastest = int.MaxValue;
        var queue = new PriorityQueue<string, int>();
        var cache = new Dictionary<string, int>();
        
        queue.Enqueue(start, 0);

        while (queue.TryDequeue(out var node, out var distance))
        {
            if (distance > fastest)
                continue;
            
            if (node == finish)
            {
                fastest = Math.Min(fastest, distance);
                continue;
            }
            
            if (cache.TryGetValue(node, out var cachedDistance) && cachedDistance <= distance)
                continue;
            
            cache[node] = distance;
            
            foreach (var edge in _edges.Where(e => e.Contains(node)))
            {
                var nextNode = edge.Node1 == node ? edge.Node2 : edge.Node1;
                var nextDistance = distance + 1;
                
                if (nextDistance < fastest)
                    queue.Enqueue(nextNode, nextDistance);
            }
        }

        return fastest;
    }
    
    public void AddEdge(string node1, string node2)
    {
        AddEdge(Edge.Create(node1, node2));
    }
    
    private void AddEdge(Edge edge)
    {
        _edges.Add(edge);
        _nodes.Add(edge.Node1);
        _nodes.Add(edge.Node2);
    }

    public void RemoveEdge(Edge edge)
    {
        _edges.Remove(edge);
    }
}

    internal readonly record struct Edge(string Node1, string Node2)
    {
        public bool Contains(string node) => 
            Node1 == node || Node2 == node;
        
        public override string ToString() => $"{Node1}-{Node2}";
        
        public bool Equals(Edge other) => 
            Node1 == other.Node1 && Node2 == other.Node2;

        public override int GetHashCode()
        {
            return HashCode.Combine(Node1, Node2);
        }

        public static Edge Create(string node1, string node2)
        {
            return string.CompareOrdinal(node1, node2) < 0
                ? new Edge(node1, node2)
                : new Edge(node2, node1);
        }
    }
