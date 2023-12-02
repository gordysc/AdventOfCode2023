// See https://aka.ms/new-console-template for more information

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var total = 0;

for (var loop = 0; loop < lines.Length; loop++)
{
    if (Parser.IsPossible(lines[loop], 12, 13, 14))
        total += (loop + 1);
}

Console.WriteLine($"Total: {total}");

internal class Parser
{
    public static bool IsPossible(string line, int red, int green, int blue)
    {
        var results = line.Split(":").Last().Split(";");
        
        foreach (var result in results)
        {
            var cubes = result.Split(",");
            foreach (var cube in cubes)
            {
                var values = cube.Trim().Split(" ");
                if (values[1] == "red" && int.Parse(values[0]) > red)
                    return false;
                if (values[1] == "green" && int.Parse(values[0]) > green)
                    return false;
                if (values[1] == "blue" && int.Parse(values[0]) > blue)
                    return false;
            }
        }

        return true;
    }
}