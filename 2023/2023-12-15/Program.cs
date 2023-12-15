var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve();
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private static readonly string Data = File.ReadAllText(@"../../../Data/Input.txt");
    // private static readonly string Data = File.ReadAllText(@"../../../Data/Example.txt");
    private static readonly string[] Steps = Data.Split(",", StringSplitOptions.RemoveEmptyEntries);

    public void Solve()
    {
        var answer = Steps.Aggregate(0, (acc, seq) => acc + Calculate(seq));

        Console.WriteLine($"Answer: {answer}");
    }

    private static int Calculate(string sequence) =>
        sequence.Aggregate(0, (acc, c) => ((acc + c) * 17) % 256);
}