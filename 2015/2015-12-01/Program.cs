var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var text = File.ReadAllText(@"../../../Input.txt");

var floor = 0;
var position = 0;

for (var loop = 0; loop < text.Length; loop++)
{
    floor += text[loop] == '(' ? 1 : -1;
    position++;

    if (floor < 0) break;
}

Console.WriteLine(text);

sw.Stop();

Console.WriteLine($"Position: {position}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");