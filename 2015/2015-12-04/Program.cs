using System.Security.Cryptography;
using System.Text;

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

const string text = "ckczppom";
var loop = 1L;

do
{
    var hash = MD5.HashData(Encoding.ASCII.GetBytes($"{text}{loop}"));
    var hex = BitConverter.ToString(hash).Replace("-", "");

    if (hex.StartsWith("000000"))
        break;

    loop++;

} while (true);

sw.Stop();

Console.WriteLine($"Answer: {text}{loop}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");
