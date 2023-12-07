// See https://aka.ms/new-console-template for more information

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var hands = new Hand[lines.Length];

for (var loop = 0; loop < lines.Length; loop++)
{
    var parts = lines[loop].Split(" ");
    hands[loop] = new Hand(parts[0], int.Parse(parts[1]));
}

var comparer = new Comparer();

Array.Sort(hands, comparer);

var winnings = 0;

for (var loop = 0; loop < hands.Length; loop++)
    winnings += hands[loop].Bid * (loop + 1);

sw.Stop();

Console.WriteLine($"Winnings: {winnings}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

class Comparer : IComparer<Hand>
{
    private static readonly char[] Cards = new[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
    private static readonly IDictionary<char, int> CardStrength = Cards.ToDictionary(c => c, c => Array.IndexOf(Cards, c) + 2);
    
    public int Compare(Hand A, Hand B)
    {
        if (A.Strength > B.Strength) return 1;
        if (A.Strength < B.Strength) return -1;

        for (var loop = 0; loop < A.Cards.Length; loop++)
        {
            if (A.Cards[loop] == B.Cards[loop]) continue;
            if (CardStrength[A.Cards[loop]] > CardStrength[B.Cards[loop]]) return 1;
            
            return -1;
        }
        
        return 0;
    }
}

record Hand(string Cards, int Bid)
{
    public int Strength => Counts switch {
        var x when x.Any(c => c.Item2 == 5) => 7,
        var x when x.Any(c => c.Item2 == 4) => 6,
        var x when x.Any(c => c.Item2 == 3) && x.Any(c => c.Item2 == 2) => 5,
        var x when x.Any(c => c.Item2 == 3) => 4,
        var x when x.Count(c => c.Item2 == 2) == 2 => 3,
        var x when x.Any(c => c.Item2 == 2) => 2,
        _ => 1
    };

    private (char, int)[] Counts => Cards.Distinct().Select(c => (c, Cards.Count(x => x == c))).ToArray();
};