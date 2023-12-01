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
    public int FindFirstDigit(string line) {
        var digit = line.First(char.IsDigit);

        return int.Parse(digit.ToString());
    }

    public int FindLastDigit(string line) {
        var digit = line.Last(char.IsDigit);

        return int.Parse(digit.ToString());
    }
}
