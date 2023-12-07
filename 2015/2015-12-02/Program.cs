var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input.txt");

var feet = 0;

foreach (var line in lines)
{
    var values = line.Split('x').Select(int.Parse).ToArray();
    var box = new Box(values[0], values[1], values[2]);

    feet += box.Volume + box.Perimiter;
}

sw.Stop();

Console.WriteLine($"Answer: {feet}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

record Box(int Height, int Width, int Length)
{
    public int Volume => Height * Width * Length;
    public int Perimiter => new[] { Height + Width, Width + Length, Length + Height }.Min() * 2;
}