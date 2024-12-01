namespace AoC2024.Days;

public static class Day1
{
    public static int Part1(string input)
    {
        var lists = input.Split('\n')
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Aggregate((left: new List<int>(), right: new List<int>()), (lists, line) =>
            {
                lists.left.Add(int.Parse(line[0]));
                lists.right.Add(int.Parse(line[1]));
                return lists;
            });

        return lists.left.Order().Zip(lists.right.Order(), (a, b) => Math.Abs(a - b)).Sum();
    }
    
    public static int Part2(string input)
    {
        var lists = input.Split('\n')
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Aggregate((left: new List<int>(), right: new Dictionary<int, int>()), (lists, line) =>
            {
                var leftNum = int.Parse(line[0]);
                var rightNum = int.Parse(line[1]);
                
                lists.left.Add(leftNum);
                lists.right[rightNum] = lists.right.GetValueOrDefault(rightNum) + 1;
                
                return lists;
            });

        return lists.left.Sum(x => lists.right.GetValueOrDefault(x) * x);
    }
}