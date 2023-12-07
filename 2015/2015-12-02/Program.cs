var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input.txt");

var area = 0;

foreach (var line in lines)
{
    var values = line.Split('x').Select(int.Parse).ToArray();
    var box = new Box(values[0], values[1], values[2]);

    area += box.TotalArea + box.MinimumArea;
}

sw.Stop();

Console.WriteLine($"Total area: {area}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

record Box(int Height, int Width, int Length)
{
    public int TotalArea => 2 * (Height * Width + Width * Length + Length * Height);
    public int MinimumArea => new[] { Height * Width, Width * Length, Length * Height }.Min();
}