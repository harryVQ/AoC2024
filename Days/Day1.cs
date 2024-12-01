namespace AoC2024.Days;

public static class Day1
{
    public static int Part1(string input)
    {
        var lists = input.Split("\n").Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Aggregate((left: new List<int>(), right: new List<int>()), (lists, line) =>
            {
                lists.left.Add(int.Parse(line[0].Trim()));
                lists.right.Add(int.Parse(line[1].Trim()));
                return lists;
            });

        return lists.left.Order().Zip(lists.right.Order(), (a, b) => Math.Abs(a - b)).Sum();
    }
    
    public static int Part2(string input)
    {
        var lists = input.Split("\n").Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Aggregate((left: new List<int>(), right: new Dictionary<int, int>()), (lists, line) =>
            {
                lists.left.Add(int.Parse(line[0].Trim()));
                if(!lists.right.ContainsKey(int.Parse(line[1].Trim())))
                {
                    lists.right.Add(int.Parse(line[1].Trim()), 1);
                }
                else
                {
                    lists.right[int.Parse(line[1].Trim())]++;
                }
                return lists;
            });

        return lists.left.Select(x =>  lists.right.TryGetValue(x, out var value) ? x * value : 0).Sum();
    }
}