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
        var vectors = lines.Select(ParseVector).ToArray();
        var answer = 0;
        
        for (var i = 0; i < vectors.Length; i++)
        for (var j = i + 1; j < vectors.Length; j++)
        {
            var v1 = vectors[i];
            var v2 = vectors[j];

            var x = (v1.B - v2.B) / (v2.M - v1.M);
            var y = v1.M * x + v1.B;

            if (v1.InPast(x, y) || v2.InPast(x, y))
                continue;

            if (x is >= S1 and <= S2 && y is >= S1 and <= S2)
                answer++;
        }
        
        Console.WriteLine($"Answer: {answer}");
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