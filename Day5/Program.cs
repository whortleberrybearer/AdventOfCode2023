var input = File.ReadAllText("Input.txt");
var lines = input.Split("\r\n");
var seedsToLocationMap = new Dictionary<long, long>();
IEnumerable<Map> seedToSoilMaps = null;
IEnumerable<Map> soilToFertilizerMaps = null;
IEnumerable<Map> fertilizerToWaterMaps = null;
IEnumerable<Map> waterToLightMaps = null;
IEnumerable<Map> lightToTempratureMaps = null;
IEnumerable<Map> tempratureToHumidityMaps = null;
IEnumerable<Map> humidityToLocationMaps = null;
IEnumerable<Range> seedRanges = null;

for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    var line = lines[lineIndex];

    Console.WriteLine($"Line: {line}");

    if (line.StartsWith("seeds:"))
    {
        seedRanges = ExtractSeedRanges(ref lineIndex);
        }
    }
    else if (line.StartsWith("seed-to-soil map:"))
    {
        seedToSoilMaps = ExtractMaps(ref lineIndex);
    }
    else if (line.StartsWith("soil-to-fertilizer map:"))
    {
        soilToFertilizerMaps = ExtractMaps(ref lineIndex);
    }
    else if (line.StartsWith("fertilizer-to-water map:"))
    {
        fertilizerToWaterMaps = ExtractMaps(ref lineIndex);
    }
    else if (line.StartsWith("water-to-light map:"))
    {
        waterToLightMaps = ExtractMaps(ref lineIndex);
    }
    else if (line.StartsWith("light-to-temperature map:"))
    {
        lightToTempratureMaps = ExtractMaps(ref lineIndex);
    }
    else if (line.StartsWith("temperature-to-humidity map:"))
    {
        tempratureToHumidityMaps = ExtractMaps(ref lineIndex);
    }
    else if (line.StartsWith("humidity-to-location map:"))
    {
        humidityToLocationMaps = ExtractMaps(ref lineIndex);
    }
}

var soilRanges = MapRanges(seedToSoilMaps!, seedRanges);
var fertilizerRanges = MapRanges(soilToFertilizerMaps!, soilRanges);
var waterRanges = MapRanges(fertilizerToWaterMaps!, fertilizerRanges);
var lightRanges = MapRanges(waterToLightMaps!, waterRanges);
var tempratureRanges = MapRanges(lightToTempratureMaps!, lightRanges);
var humidityRanges = MapRanges(tempratureToHumidityMaps!, tempratureRanges);
var locationRanges = MapRanges(humidityToLocationMaps!, humidityRanges);

Console.WriteLine($"Lowest location: {locationRanges.Select(r => r.Start).Min()}");

    Console.WriteLine($"Seed {seed} is for location {location}");
}

Console.WriteLine($"Lowest location: {seedsToLocationMap!.Values.Min()}");

long MapValue(IEnumerable<Map> maps, long source)
{
    foreach (var map in maps)
    {
        if (source >= map.SourceStart && source < (map.SourceStart + map.Length))
        {
            return map.DesinationStart + (source - map.SourceStart);
        }
    }    

    // No maps found, so return the value.
    return source;
}

IEnumerable<Range> MapRanges(IEnumerable<Map> maps, IEnumerable<Range> sourceRanges)
{
    var destinationRanges = new List<Range>();

    foreach (var sourceRange in sourceRanges)
    {
        var rangeStart = sourceRange.Start;
        var rangeEnd = rangeStart + sourceRange.Length - 1;

        foreach (var map in maps)
        {
            var mapStart = map.SourceStart;
            var mapEnd = map.SourceStart + map.Length - 1;

            var length = (rangeEnd - rangeStart + 1) + (map.Length - 1);

            if (length > (Math.Max(rangeEnd, mapEnd) - Math.Min(rangeStart, mapStart)))
            {
                if (rangeStart < mapStart)
                {
                    // These do not belong to the map, so map to the same values.
                    destinationRanges.Add(new Range(rangeStart, mapStart - rangeStart));

                    rangeStart = mapStart;
                }

                if (mapEnd > rangeEnd)
                {
                    // Map goes beyond the range, so can fit it all in and stop checking.
                    destinationRanges.Add(new Range(map.DesinationStart + (rangeStart - map.SourceStart), (rangeEnd - rangeStart) + 1));

                    // Set the start to the end so the range is covered.
                    rangeStart = rangeEnd + 1;

                    break;
                }

                // Range must go beyond the map, so map values up to the end.
                destinationRanges.Add(new Range(map.DesinationStart + (rangeStart - map.SourceStart), mapEnd - rangeStart));

                rangeStart = mapEnd + 1;
            }
        }

        if (rangeStart <= rangeEnd)
        {
            // This has not been covered by any of the ranges, so map to the same values again.
            destinationRanges.Add(new Range(rangeStart, (rangeEnd - rangeStart) + 1));
        }

        // TODO. Need to cover when start not in range.
        // TODO. Need to cover when part of the list is in a range.
    }

    return destinationRanges;
}

IEnumerable<Map> ExtractMaps(ref int lineIndex)
{
    var maps = new List<Map>();

    while ((lineIndex < lines.Length - 1) && !string.IsNullOrEmpty(lines[++lineIndex]))
    {
        var line = lines[lineIndex];

        Console.WriteLine($"Line: {line}");

        var parts = line.Split(' ');

        maps.Add(new Map(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
    }

    return maps.OrderBy(m => m.SourceStart).ToArray();
}

IEnumerable<Range> ExtractSeedRanges(ref int lineIndex)
{
    var seedRanges = new List<Range>();
    var parts = lines[lineIndex].Replace("seeds: ", string.Empty).Split(' ');

    for (int i = 0; i < parts.Length; i += 2)
    {
        seedRanges.Add(new Range(long.Parse(parts[i]), long.Parse(parts[i + 1])));
    }

    ++lineIndex;

    return seedRanges.OrderBy(r => r.Start).ToArray();
}

record Map(long DesinationStart, long SourceStart, long Length);
record Range(long Start, long Length);