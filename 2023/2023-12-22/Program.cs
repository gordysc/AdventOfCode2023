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
        var sorted = bricks.OrderBy(b => Math.Min(b.C1.Z, b.C2.Z)).ToList();

        var answer = 0;
        
        Console.WriteLine($"Answer: {answer}");
    }

    private static IEnumerable<Brick> CreateBricks(IEnumerable<string> input) =>
        input.Select(CreateBrick).ToArray();

    private static Brick CreateBrick(string input)
    {
        var parts = input.Split("~")
            .Select(l => l.Split(",")
                .Select(int.Parse).ToArray()).ToArray();

        var c1 = new Coordinates { X = parts[0][0], Y = parts[0][1], Z = parts[0][2] };
        var c2 = new Coordinates { X = parts[1][0], Y = parts[1][1], Z = parts[1][2] };

        return new Brick(c1, c2);
    }
}

internal sealed class Coordinates
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}

internal sealed class Brick(Coordinates c1, Coordinates c2)
{
    public Coordinates C1 { get; } = c1;
    public Coordinates C2 { get; } = c2;

    public void Descend()
    {
        C1.Z -= 1;
        C2.Z -= 1;
    }

    public bool Intersects(Brick brick)
    {
        brick.
        // Check if the highest point of this class is lower than the lowest point of the given brick
        if (Math.Max(C1.Z, C2.Z) < (Math.Min(brick.C1.Z, brick.C2.Z) - 1))
            return false;
    }
    
    public bool IsFlat => C1.Z == C2.Z;
}