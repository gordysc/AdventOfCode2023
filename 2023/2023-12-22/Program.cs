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

    public void Solve(IEnumerable<string> input)
    {
        var bricks = CreateBricks(input);
        var rested = new List<Brick>();

        foreach (var brick in bricks)
            rested.Add(ExecuteGravity(rested, brick));

        var disintegrate = new HashSet<Brick>();

        foreach (var brick in rested)
        {
            var above = rested.Where(r => brick.Top + 1 == r.Bottom)
                .Where(r => brick.Overlaps(r));

            // Check if there are no bricks above it
            if (above.Count() == 0)
                disintegrate.Add(brick);

            // Check if all the bricks above it have at 2 bricks below them
            else if (above.All(a => NumberOfSupportingBricks(rested, a) > 1))
                disintegrate.Add(brick);
        }

        var answer = disintegrate.Count;
        
        Console.WriteLine($"Answer: {answer}");
    }

    private static int NumberOfSupportingBricks(IEnumerable<Brick> bricks, Brick brick) =>
        bricks.Where(r => r.Top == brick.Bottom - 1)
            .Count(r => r.Overlaps(brick));

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
    public int Bottom => Math.Min(C1.Z, C2.Z);
    public int Top => Math.Max(C1.Z, C2.Z);
    public bool IsOnGround => Bottom == 1;

    public bool IsRested(IEnumerable<Brick> rested) =>
        rested.Any(r => Bottom - 1 == r.Top && Overlaps(r));

    public bool Overlaps(Brick brick) =>
        OverlapsX(brick) && OverlapsY(brick);

    public Brick Descend() =>
        new(C1 with { Z = C1.Z - 1 }, C2 with { Z = C2.Z - 1 });

    private bool OverlapsX(Brick brick) =>
        brick.C1.X <= C2.X && brick.C2.X >= C1.X;

    private bool OverlapsY(Brick brick) =>
        brick.C1.Y <= C2.Y && brick.C2.Y >= C1.Y;
}
