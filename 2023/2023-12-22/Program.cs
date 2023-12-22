var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();

// var input = File.ReadAllLines(@"../../../Data/Example.txt");
var input = File.ReadAllLines(@"../../../Data/Input.txt");

solution.Solve(input);

sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution()
{
    private readonly IDictionary<Brick, IEnumerable<Brick>> above = new Dictionary<Brick, IEnumerable<Brick>>();
    private readonly IDictionary<Brick, IEnumerable<Brick>> below = new Dictionary<Brick, IEnumerable<Brick>>();

    public void Solve(IEnumerable<string> input)
    {
        var bricks = CreateBricks(input).OrderBy(b => b.Bottom);
        var rested = new List<Brick>();

        foreach (var brick in bricks)
            rested.Add(ExecuteGravity(rested, brick));

        foreach (var brick in rested)
            above[brick] = rested.Where(r => r.Bottom - 1 == brick.Top)
                .Where(r => r.Overlaps(brick));

        foreach (var brick in rested)
            below[brick] = rested.Where(r => r.Top + 1 == brick.Bottom)
                .Where(r => r.Overlaps(brick));

        var answer = rested.AsParallel().Select(b => FindDisintegratedBricks([b])).Sum();

        Console.WriteLine($"Answer: {answer}");
    }

    private int FindDisintegratedBricks(HashSet<Brick> removed)
    {
        var candidates = removed.SelectMany(r => above[r]).ToHashSet();
        var disintegrated = candidates.Where(c => !removed.Contains(c) && below[c].All(removed.Contains)).ToHashSet();

        if (disintegrated.Count == 0)
            return removed.Count - 1;

        return FindDisintegratedBricks(removed.Union(disintegrated).ToHashSet());
    }

    private static Brick ExecuteGravity(IEnumerable<Brick> rested, Brick brick)
    {
        if (brick.IsOnGround || brick.IsRested(rested))
            return brick;

        return ExecuteGravity(rested, brick.Descend());
    }

    private static IEnumerable<Brick> CreateBricks(IEnumerable<string> input) =>
        input.Select(CreateBrick).ToArray();

    private static Brick CreateBrick(string input)
    {
        var parts = input.Split("~")
            .Select(l => l.Split(",")
                .Select(int.Parse).ToArray()).ToArray();

        var c1 = new Coordinates(parts[0][0], parts[0][1], parts[0][2]);
        var c2 = new Coordinates(parts[1][0], parts[1][1], parts[1][2]);

        return new Brick(c1, c2);
    }
}

internal record Coordinates(int X, int Y, int Z);

internal record Brick(Coordinates C1, Coordinates C2)
{
    public bool IsOnGround => Bottom == 1;

    private int X1 => Math.Min(C1.X, C2.X);
    private int X2 => Math.Max(C1.X, C2.X);
    private int Y1 => Math.Min(C1.Y, C2.Y);
    private int Y2 => Math.Max(C1.Y, C2.Y);
    public int Top => Math.Max(C1.Z, C2.Z);
    public int Bottom => Math.Min(C1.Z, C2.Z);

    public bool IsRested(IEnumerable<Brick> rested) =>
        rested.Any(r => Bottom - 1 == r.Top && Overlaps(r));

    public bool Overlaps(Brick brick) =>
        OverlapsX(brick) && OverlapsY(brick);

    public Brick Descend() =>
        new(C1 with { Z = C1.Z - 1 }, C2 with { Z = C2.Z - 1 });

    private bool OverlapsX(Brick brick) =>
        brick.X1 <= X2 && brick.X2 >= X1;

    private bool OverlapsY(Brick brick) =>
        brick.Y1 <= Y2 && brick.Y2 >= Y1;
}
