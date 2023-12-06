// See https://aka.ms/new-console-template for more information

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/Example.txt");
var numbers = lines[0].Split(":")[1].Trim().Split(" ").Select(long.Parse).ToArray();
var ranges = new List<SeedRange>();

for (var loop = 0; loop < numbers.Length / 2; loop++)
{
    var start = numbers[loop * 2];
    var length = numbers[loop * 2 + 1];
    
    ranges.Add(new SeedRange(start, start + length - 1));
}

var mappers = new List<SeedMapper>();
var mapper = new SeedMapper();

for (var loop = 2; loop < lines.Length; loop++)
{
    var line = lines[loop];
    
    if (string.IsNullOrWhiteSpace(line)) continue;
    
    if (line.Contains("map"))
    {
        mappers.Add(mapper);
        mapper = new SeedMapper();
        continue;
    }
    
    mapper.AddMapping(line);
}

mappers.Add(mapper);


sw.Stop();

Console.WriteLine($"Total time: {sw.ElapsedMilliseconds}ms");

internal record SeedRange(long Start, long Stop);

internal sealed class SeedMapper
{
    private List<SeedMapping> _mappings = new List<SeedMapping>();

    public void AddMapping(string line)
    {
        var parts = line.Trim().Split(" ").Select(long.Parse).ToArray();
        var mapping = new SeedMapping(parts[0], parts[1], parts[2]);

        _mappings.Add(mapping);
    }
    
    public SeedMapping[] Intersects(SeedRange range)
    {
        return _mappings.Where(m => m.Source >= range.Start && m.Source <= range.Stop).ToArray();
    }

}
internal record SeedMapping(long Destination, long Source, long Range);