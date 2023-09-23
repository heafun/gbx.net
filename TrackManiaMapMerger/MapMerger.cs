using GBX.NET.Engines.Game;

namespace TrackManiaMapMerger;
internal class MapMerger
{
    public static CGameCtnChallenge MergeMaps(List<CGameCtnChallenge> mapList)
    {
        CGameCtnChallenge output = mapList[0];

        for (var i = 0; i < mapList.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            Console.WriteLine($"Merging map {i + 1}...");

            output = MergeMaps(output, mapList[i]);

            Console.WriteLine(string.Empty);
        }

        return output;
    }

    public static CGameCtnChallenge MergeMaps(CGameCtnChallenge map1, CGameCtnChallenge map2)
    {
        Console.WriteLine("Merging AnchoredObjects...");
        map1.AnchoredObjects = MergeAnchoredObjects(map1.AnchoredObjects, map2.AnchoredObjects);

        Console.WriteLine("Merging BakedBlocks...");
        File.AppendAllLines("MergeLog.txt", new List<string>() { "Merging BakedBlocks..." });
        map1.BakedBlocks = MergeBlocks(map1.BakedBlocks, map2.BakedBlocks);

        Console.WriteLine("Merging Blocks...");
        File.AppendAllLines("MergeLog.txt", new List<string>() { "Merging Blocks..." });
        map1.Blocks = MergeBlocks(map1.Blocks, map2.Blocks);

        return map1;
    }

    private static IList<CGameCtnAnchoredObject>? MergeAnchoredObjects(
        IList<CGameCtnAnchoredObject>? anchoredObjects1, 
        IList<CGameCtnAnchoredObject>? anchoredObjects2)
    {
        if (anchoredObjects1 is null)
        {
            return anchoredObjects2;
        } 
        
        if (anchoredObjects2 is null)
        {
            return anchoredObjects1;
        }

        var output = anchoredObjects1;

        var previousLineLength = 0;
        var counter = 1;

        foreach (var anchoredObject in anchoredObjects2)
        {
            if (!output.Any(a => a.AbsolutePositionInMap.Equals(anchoredObject.AbsolutePositionInMap)))
            {
                output.Add(anchoredObject);
            }

            var status = $"[{counter} / {anchoredObjects2.Count}]";
            previousLineLength = status.Length;
            Console.Write("\r{0}   ", status.PadRight(previousLineLength));

            counter++;
        }

        Console.WriteLine();

        return output;
    }

    private static IList<CGameCtnBlock>? MergeBlocks(
        IList<CGameCtnBlock>? blocks1,
        IList<CGameCtnBlock>? blocks2)
    {
        if(blocks1 is null) {
            return blocks2;
        }
         
        if(blocks2 is null)
        {
            return blocks1;
        }

        var output = blocks1;

        var previousLineLength = 0;
        var counter = 1;

        var logLines = new List<string>();

        foreach (var block in blocks2)
        {
            if(!output.Any(b => b.Coord.Equals(block.Coord) &&
            b.BlockModel.Equals(block.BlockModel) &&
            b.Direction.Equals(block.Direction) &&
            b.PitchYawRoll.Equals(block.PitchYawRoll))) {
                output.Add(block);
            } else
            {
                logLines.Add($"Block skipped: {block.BlockModel} {block.Coord}");
            }

            var status = $"[{counter} / {blocks2.Count}]";
            previousLineLength = status.Length;
            Console.Write("\r{0}   ", status.PadRight(previousLineLength));

            counter++;
        }

        Console.WriteLine();

        File.AppendAllLines("MergeLog.txt", logLines);

        return output;
    }
}
