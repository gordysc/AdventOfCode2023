var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();

// var input = File.ReadAllLines(@"../../../Data/Example.txt");
var input = File.ReadAllLines(@"../../../Data/Input.txt");

solution.Solve(input);

sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private const float S1 = 200_000_000_000_000;
    private const float S2 = 400_000_000_000_000;
    // private static readonly float S1 = 7;
    // private static readonly float S2 = 27;
    public void Solve(string[] lines)
    {
        // Give up on life and use mathematica.
    }

    private static Vector ParseVector(string line)
    {
        var parts = line.Split(new[] { ',', '@', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToArray();

        return new Vector(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]);
    }
}

internal record Vector(float X, float Y, float Z, float DX, float DY, float DZ)
{
    public float M => DY / DX;
    public float B => Y - M * X;

    public bool InPast(float x, float y) =>
        // Are we moving to the right and it intersected to the left?
        (DX >= 0 && x < X) ||
        // Are we moving to the left and it intersects to the right?
        (DX < 0 && x > X) ||
        // Are we moving up and it intersected below?
        (DY >= 0 && y < Y) ||
        // Are we moving downward and it intersected above?
        (DY < 0 && y > Y);

}