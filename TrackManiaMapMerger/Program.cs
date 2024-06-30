using GBX.NET;
using GBX.NET.Engines.Game;
using TrackManiaMapMerger;

Console.WriteLine("Map Merger started!");

Console.WriteLine("How many maps would you like to merge? (Default 2)");

string? mapCountString = Console.ReadLine();

var mapCount = 2;

if (string.IsNullOrWhiteSpace(mapCountString))
{
    Console.WriteLine("Defaulted to 2");
} else
{
    try
    {
        mapCount = int.Parse(mapCountString);
    }
    catch
    {
        Console.WriteLine("Input was not valid. Defaulted to 2");
    }
}

var mapPaths = new List<string>();

for (int i = 0; i < mapCount; i++)
{
    Console.WriteLine($"Input Path of Map {i + 1}:");
    mapPaths.Add(@"" + Console.ReadLine().Trim('\"'));
}

Console.WriteLine("Input name of the output map:");
var outputFilename = Console.ReadLine();

var maps = new List<CGameCtnChallenge>();

foreach (var path in mapPaths)
{
    maps.Add((CGameCtnChallenge) GameBox.ParseNode(path));
}

Console.WriteLine("Starting Merging...");

var outputMap = MapMerger.MergeMaps(maps);

outputMap.MapName = outputFilename;

Console.WriteLine("Merge completed!");
Console.WriteLine("Saving...");

outputMap.Save($"C:\\Users\\marcr\\Desktop\\{outputFilename}.Map.Gbx");

Console.WriteLine("Saved!");