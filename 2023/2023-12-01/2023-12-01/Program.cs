// See https://aka.ms/new-console-template for more information

var digitizer = new Digitizer();
var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var total = 0;

foreach (var line in lines) {
    var first = digitizer.FindFirstDigit(line);
    var last = digitizer.FindLastDigit(line);
    var value = Int32.Parse($"{first}{last}");

    total += value;
}

Console.WriteLine(total);

internal sealed class Digitizer {
    static readonly string[] HumanizedDigits = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    public int FindFirstDigit(string line) {
        var digit = line.First(char.IsDigit);
        var digitIdx = line.IndexOf(digit);

        var segment = line.Substring(0, digitIdx);
        var humanizedDigit = FindFirstHumanizedDigit(segment);

        if (humanizedDigit != null) {
            return Array.IndexOf(HumanizedDigits, humanizedDigit) + 1;
        }

        return int.Parse(digit.ToString());
    }

    public int FindLastDigit(string line) {
        var digit = line.Last(char.IsDigit);
        var digitIdx = line.LastIndexOf(digit);

        var segment = line.Substring(digitIdx);
        var humanizedDigit = FindLastHumanizedDigit(segment);

        if (humanizedDigit != null) {
            return Array.IndexOf(HumanizedDigits, humanizedDigit) + 1;
        }

        return int.Parse(digit.ToString());
    }

    private string? FindFirstHumanizedDigit(string segment) {
        var results = HumanizedDigits.Select(t => (t, segment.IndexOf(t, StringComparison.Ordinal)));
        var result = results.Where(t => t.Item2 > -1).OrderBy(t => t.Item2).FirstOrDefault();

        return result == default ? null : result.Item1;
    }

    private string? FindLastHumanizedDigit(string segment) {
        var results = HumanizedDigits.Select(t => (t, segment.LastIndexOf(t, StringComparison.Ordinal)));
        var result = results.Where(t => t.Item2 > -1).OrderByDescending(t => t.Item2).FirstOrDefault();

        return result == default ? null : result.Item1;
    }
}
