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
IEnumerable<SeedRange> seedRanges = null;

for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    var line = lines[lineIndex];

    Console.WriteLine($"Line: {line}");

    if (line.StartsWith("seeds:"))
    {
        seedRanges = ExtractSeedRanges(ref lineIndex);

        foreach (var seedRange in seedRanges)
        {
            
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

foreach (var seed in seedsToLocationMap!.Keys)
{
    var soil = MapValue(seedToSoilMaps!, seed);
    var fertilizer = MapValue(soilToFertilizerMaps!, soil);
    var water = MapValue(fertilizerToWaterMaps!, fertilizer);
    var light = MapValue(waterToLightMaps!, water);
    var temprature = MapValue(lightToTempratureMaps!, light);
    var humidity = MapValue(tempratureToHumidityMaps!, temprature);
    var location = MapValue(humidityToLocationMaps!, humidity);

    seedsToLocationMap[seed] = location;

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

IEnumerable<SeedRange> ExtractSeedRanges(ref int lineIndex)
{
    var seedRanges = new List<SeedRange>();
    var parts = lines[lineIndex].Replace("seeds: ", string.Empty).Split(' ');

    for (int i = 0; i < parts.Length; i += 2)
    {
        seedRanges.Add(new SeedRange(long.Parse(parts[i]), long.Parse(parts[i + 1]));
    }

    ++lineIndex;

    return seedRanges;
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

    return maps;
}

record Map(long DesinationStart, long SourceStart, long Length);

record SeedRange(long Start, long length);