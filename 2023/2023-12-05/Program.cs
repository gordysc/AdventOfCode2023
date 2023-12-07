// See https://aka.ms/new-console-template for more information

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");

var inputs = lines[0].Split(":")[1].Trim().Split(" ").Select(long.Parse).ToArray();
var seeds = inputs.Chunk(2).Select(v => new SeedRange(v[0], v[1])).ToArray();

var evaluator = new Evaluator();

var groups = evaluator.CreateGroups(lines[3..]);
var closest = evaluator.Evaluate(seeds, groups);

sw.Stop();

Console.WriteLine($"Closest: {closest}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Evaluator
{
    
    public SeedMapper[][] CreateGroups(string[] lines)
    {
        var groups = new List<SeedMapper[]>();
        var mappers = new List<SeedMapper>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            
            if (line.Contains("map"))
            {
                groups.Add(mappers.ToArray());
                mappers.Clear();
                continue;
            }

            var values = line.Split(" ").Select(long.Parse).ToArray();
            
            var destination = values[0];
            var source = new SeedRange(values[1], values[2]);
            
            mappers.Add(new SeedMapper(destination, source));
        }
        
        if (mappers.Any()) groups.Add(mappers.ToArray());

        return groups.ToArray();
    }

    public long Evaluate(SeedRange[] seeds, SeedMapper[][] groups)
    {
        var results = seeds.ToList();
        var mapped = new List<SeedRange>();
        
        foreach (var group in groups)
        {
            foreach (var result in results)
                foreach (var mapper in group)
                    if (mapper.Overlaps(result))
                        mapped.Add(mapper.Map(result));

            results = mapped;
            mapped = new List<SeedRange>();
        }

        return results.Select(r => r.Start).Min();
    }
}

internal record SeedRange(long Start, long Range)
{
    public long End => Start + Range - 1;
}

internal record SeedMapper(long Destination, SeedRange Source)
{
    public SeedRange Map(SeedRange input)
    {
        var start = Math.Max(input.Start, Source.Start);
        var end = Math.Min(input.End, Source.End);

        var offset = start - Source.Start;
        var range = end - start + 1;
        
        return new SeedRange(Destination + offset, range);
    }   
    public bool Overlaps(SeedRange input) => Source.Start <= input.End && input.Start <= Source.End;
}