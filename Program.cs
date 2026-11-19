using System.Text.Json;

try
{
    if (args.Length != 1) throw new ArgumentException("Pass one JSON input file");
    using var data = JsonDocument.Parse(File.ReadAllText(args[0]));
    Console.WriteLine(JsonSerializer.Serialize(Analyze(data.RootElement)));
    return 0;
}
catch (Exception error) when (error is IOException or JsonException or FormatException
                               or ArgumentException or InvalidOperationException
                               or KeyNotFoundException or OverflowException)
{
    Console.Error.WriteLine(error.Message);
    return 2;
}

static object Analyze(JsonElement input) {
 var vector=input.GetProperty("vector").EnumerateArray().Select(x=>x.GetDouble()).ToArray();var direction=input.GetProperty("direction").EnumerateArray().Select(x=>x.GetDouble()).ToArray();
 if(vector.Length==0||vector.Length!=direction.Length||vector.Concat(direction).Any(x=>!double.IsFinite(x)))throw new ArgumentException("Use matching finite vectors");
 var largest=direction.Max(x=>Math.Abs(x));if(largest==0)throw new ArgumentException("Direction must be nonzero");
 var unit=direction.Select(x=>x/largest).ToArray();var norm=Math.Sqrt(unit.Sum(x=>x*x));unit=unit.Select(x=>x/norm).ToArray();var projection=vector.Select((x,i)=>x*unit[i]).Sum();
 var parallel=unit.Select(x=>x*projection).ToArray();var perpendicular=vector.Select((x,i)=>x-parallel[i]).ToArray();if(parallel.Concat(perpendicular).Any(x=>!double.IsFinite(x)))throw new ArgumentException("Calculation exceeds numeric range");
 return new{parallel=parallel.Select(x=>Math.Round(x,8)),perpendicular=perpendicular.Select(x=>Math.Round(x,8)),componentDot=Math.Round(parallel.Select((x,i)=>x*perpendicular[i]).Sum(),8)};
}
