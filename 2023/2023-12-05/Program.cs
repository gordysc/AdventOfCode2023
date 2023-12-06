// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var mappers = AdventUtils.CreateMappers(lines);
var seeds = AdventUtils.CreateSeeds(lines[0]);
var results = new long[seeds.Length];

Parallel.ForEach(seeds, (seed, state, index) =>
{
    results[index] = seed;
    foreach (var mapper in mappers)
        results[index] = mapper.Map(results[index]);
});

var closest = results.Min();

sw.Stop();

Console.WriteLine($"Closest: {closest}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class AdventUtils
{
    public static long[] CreateSeeds(string line)
    {
        var entries = line.Split(":")[1].Trim().Split(" ").Select(long.Parse).ToArray();
        var pairs = entries.Length / 2;
        var seeds = new List<long>();

        for (var loop = 0; loop < pairs; loop++)
        {
            var start = entries[loop * 2];
            var length = entries[loop * 2 + 1];

            for (var iter = 0; iter < length; iter++)
                seeds.Add(start + iter);
        }

        return seeds.ToArray();
    }

    public static List<Mapper> CreateMappers(string[] lines)
    {
        var mappers = new List<Mapper>();
        var mapper = new Mapper();

        for (var loop = 3; loop < lines.Length; loop++)
        {
            var line = lines[loop];

            if (string.IsNullOrEmpty(line))
                continue;

            if (line.Contains("map"))
            {
                mappers.Add(mapper);
                mapper = new Mapper();
                continue;
            }

            mapper.AddMap(line);
        }

        mappers.Add(mapper);

        return mappers;
    }
}

internal class Mapper
{
    private readonly List<SeedMap> _maps = new ();

    public void AddMap(string line)
    {
        var values = line.Trim().Split(" ").Select(long.Parse).ToArray();
        var map = new SeedMap(values[0], values[1], values[2]);

        _maps.Add(map);
    }

    public long Map(long source)
    {
        foreach (var map in _maps)
            if (source >= map.Start && source < map.Start + map.Range)
                return map.Destination + (source - map.Start);

        return source;
    }
}
internal record SeedMap(long Destination, long Start, long Range);
