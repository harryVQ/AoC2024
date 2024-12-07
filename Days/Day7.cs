public static class Day7
{
    public static long Part1(string input)
    {
        return input.Split("\n")
            .Select(line =>
            {
                var parts = line.Split(":");
                return (target: long.Parse(parts[0]),
                    values: parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray());
            })
            .Where(x =>
                Enumerable.Range(0, (int)Math.Pow(2, x.values.Length - 1))
                    .Select(i => string.Join("",
                        Enumerable.Range(0, x.values.Length - 1)
                            .Select(pos => i / (int)Math.Pow(2, pos) % 2 == 1 ? '+' : '*')))
                    .Any(ops => x.values
                        .Skip(1)
                        .Zip(ops, (value, opChar) => (value, op: opChar))
                        .Aggregate((long)x.values[0],
                            (currentValue, next) => next.op == '*'
                                ? currentValue * next.value
                                : currentValue + next.value) == x.target))
            .Sum(x => x.target);
    }

    public static long Part2(string input)
    {
        return input.Split("\n")
            .Select(line =>
            {
                var parts = line.Split(":");
                return (target: long.Parse(parts[0]),
                    values: parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray());
            })
            .Where(x =>
                Enumerable.Range(0, (int)Math.Pow(3, x.values.Length - 1))
                    .Select(i => string.Join("",
                        Enumerable.Range(0, x.values.Length - 1)
                            .Select(pos => (i / (int)Math.Pow(3, pos) % 3) switch
                            {
                                0 => '+',
                                1 => '|',
                                2 => '*'
                            })))
                    .Any(ops => x.values
                        .Skip(1)
                        .Zip(ops, (value, op) => (value, op))
                        .Aggregate((long)x.values[0],
                            (currentValue, next) => next.op switch
                            {
                                '+' => currentValue + next.value,
                                '|' => long.Parse(currentValue.ToString() + next.value),
                                '*' => currentValue * next.value
                            }) == x.target))
            .Sum(x => x.target);
    }
}