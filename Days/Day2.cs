namespace AoC2024.Days;

public static class Day2
{
    public static int Part1(string input)
    {
        return input.Split('\n')
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
            .Count(ValidReport);
    }

    public static int Part2(string input)
    {
        return input.Split('\n')
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
            .Count(report =>
            {
                return ValidReport(report) || Enumerable.Range(0, report.Length)
                    .Select(i => report.Take(i).Concat(report.Skip(i + 1)).ToArray())
                    .Any(ValidReport);
            });
    }

    private static bool ValidReport(int[] report)
    {
        var diffs = report.Zip(report.Skip(1), (currentLevel, nextLevel) => currentLevel - nextLevel).ToArray();
        return diffs.All(diff => Math.Abs(diff) <= 3) && (diffs.All(diff => diff > 0) || diffs.All(diff => diff < 0));
    }
}