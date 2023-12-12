// See https://aka.ms/new-console-template for more information

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var total = 0;

foreach (var line in lines)
    total += Parser.CalculatePower(line);

sw.Stop();

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total Time: {sw.ElapsedMilliseconds}ms");

internal class GameBag
{
    private int _red;
    private int _green;
    private int _blue;

    public void SetRed(int value)
    {
        if (_red == 0 || value > _red)
            _red = value;
    }
    
    public void SetGreen(int value)
    {
        if (_green == 0 || value > _green)
            _green = value;
    }
    
    public void SetBlue(int value)
    {
        if (_blue == 0 || value > _blue)
            _blue = value;
    }

    public int Power => _red * _green * _blue;
}

internal class Parser
{
    public static int CalculatePower(string line)
    {
        var bag = new GameBag();
        var results = line.Split(":").Last().Split(";");
        
        foreach (var result in results)
        {
            var cubes = result.Split(",");
            foreach (var cube in cubes)
            {
                var values = cube.Trim().Split(" ");

                if (values[1] == "red")
                    bag.SetRed(int.Parse(values[0]));
                else if (values[1] == "green")
                    bag.SetGreen(int.Parse(values[0]));
                else if (values[1] == "blue")
                    bag.SetBlue(int.Parse(values[0]));
            }
        }

        return bag.Power;
    }
}