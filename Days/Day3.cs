using System.Text.RegularExpressions;

namespace AoC2024.Days;

public static partial class Day3
{
    public static int Part1(string input)
    {
        return Part1Regex().Matches(input)
            .Select(match => new 
            { 
                X = int.Parse(match.Groups[1].Value),
                Y = int.Parse(match.Groups[2].Value)
            }).Sum(group => group.X * group.Y);
    }
    
    public static int Part2(string input)
    {
        return Part2Regex().Matches(input)
            .Select(match => new
            {
                Match = match.Value,
                Numbers = match.Value.StartsWith("mul") 
                    ? (X: int.Parse(match.Groups[1].Value), Y: int.Parse(match.Groups[2].Value))
                    : default
            })
            .Aggregate(
                (Sum: 0, ShouldDo: true), 
                (aggregateData, group) => (
                    Sum: aggregateData.ShouldDo && group.Match.StartsWith("mul") ? aggregateData.Sum + group.Numbers.X * group.Numbers.Y : aggregateData.Sum,
                    ShouldDo: group.Match switch
                    {
                        "do()" => true,
                        "don't()" => false,
                        _ => aggregateData.ShouldDo
                    }
                )
            ).Sum;
    }

    [GeneratedRegex(@"mul\(([0-9]+),([0-9]+)\)")]
    private static partial Regex Part1Regex();
    
    [GeneratedRegex(@"mul\(([0-9]+),([0-9]+)\)|don't\(\)|do\(\)")]
    private static partial Regex Part2Regex();
}