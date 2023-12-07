var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var floor = 0;
var text = File.ReadAllText(@"../../../Input.txt");

foreach (char c in text)
    floor += c == '(' ? 1 : -1;

Console.WriteLine(text);

sw.Stop();

Console.WriteLine($"Floor: {floor}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");