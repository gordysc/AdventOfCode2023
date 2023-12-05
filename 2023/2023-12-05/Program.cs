// See https://aka.ms/new-console-template for more information

var sw = new System.Diagnostics.Stopwatch();

sw.Start();
var lines = File.ReadAllLines(@"../../../Input/File1.txt");

var seeds = lines[0].Split(":")[1].Trim().Split(" ").Select(Int64.Parse).ToArray();
var mappings = (long[]) seeds.Clone();
var mapped = Enumerable.Repeat(false, mappings.Length).ToArray();

for (var loop = 2; loop < lines.Length; loop++)
{
    var line = lines[loop];
    
    if (string.IsNullOrWhiteSpace(line)) continue;

    if (line.Contains("map"))
    {
        mapped = Enumerable.Repeat(false, mappings.Length).ToArray();
        continue;
    }

    var values = line.Split(" ").Select(Int64.Parse).ToArray();
    var map = new SeedMap(values[0], values[1], values[2]);
    
    for (var i = 0; i < mappings.Length; i++)
    {
        if (mapped[i]) continue;
        if (map.InRange(mappings[i]))
        {
            mapped[i] = true;
            mappings[i] = map.Map(mappings[i]);
        }
    }
}

var closest = mappings.Min();
sw.Stop();

Console.WriteLine($"Result: {mappings.Min()}");
Console.WriteLine($"Elapsed: {sw.Elapsed.TotalMilliseconds}ms");

internal sealed class SeedMap(long Destination, long Start, long Range)
{
    public long Map(long source)
    {
        return Destination + (source - Start);
    }
    
    public bool InRange(long source)
    {
        return source >= Start && source < Start + Range;
    }
}