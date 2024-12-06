namespace AoC2024.Days;

public static class Day4
{
    
    private static readonly List<(int dx, int dy)> Directions = [(0, -1), (0, 1), (-1, 0), (1, 0), (-1, 1), (1, 1), (-1, -1), (1, -1)];
    
    private static readonly List<(int dx, int dy)> Diagonals = [(-1, 1), (1, 1), (-1, -1), (1, -1)];

    public static int Part1(string input)
    {
        var rows = input.Split('\n');
        var (width, height) = (rows[0].Length, rows.Length);

        return rows.SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
            .Aggregate(new char[height, width], (acc, cur) =>
            {
                acc[cur.y, cur.x] = cur.c;
                return acc;
            }).PassAggregate(grid => Enumerable.Range(0, width)
                .SelectMany(x => Enumerable.Range(0, height).Select(y => (x, y)))
                .Sum(pos => Directions
                    .Where(dir => pos.x + dir.dx * 3 >= 0 && 
                                  pos.x + dir.dx * 3 < width &&
                                  pos.y + dir.dy * 3 >= 0 && 
                                  pos.y + dir.dy * 3 < height)
                    .Count(dir => grid[pos.y, pos.x] == 'X' &&
                                  grid[pos.y + dir.dy, pos.x + dir.dx] == 'M' &&
                                  grid[pos.y + dir.dy * 2, pos.x + dir.dx * 2] == 'A' &&
                                  grid[pos.y + dir.dy * 3, pos.x + dir.dx * 3] == 'S')));
    }
    
    public static int Part2(string input)
    {
        var rows = input.Split('\n');
        var (width, height) = (rows[0].Length, rows.Length);

        return rows.SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
            .Aggregate(new char[height, width], (acc, cur) =>
            {
                acc[cur.y, cur.x] = cur.c;
                return acc;
            }).PassAggregate(grid => Enumerable.Range(0, width)
                .SelectMany(x => Enumerable.Range(0, height).Select(y => (x, y)))
                .Sum(pos => Diagonals
                    .Where(dir => pos.x + dir.dx >= 0 && pos.x - dir.dx >= 0 &&
                                  pos.x + dir.dx < width && pos.x - dir.dx < width &&
                                  pos.y + dir.dy >= 0 && pos.y - dir.dy >= 0 &&
                                  pos.y + dir.dy < height && pos.y - dir.dy < height)
                    .Any(dir => {
                        if (grid[pos.y, pos.x] != 'A') return false;
                        var pos1 = grid[pos.y + dir.dy, pos.x + dir.dx];
                        var pos2 = grid[pos.y - dir.dy, pos.x - dir.dx];
                        var pos3 = grid[pos.y + dir.dy, pos.x - dir.dx];
                        var pos4 = grid[pos.y - dir.dy, pos.x + dir.dx];
                        
                        return ((pos1 == 'M' && pos2 == 'S') || (pos1 == 'S' && pos2 == 'M')) && ((pos3 == 'M' && pos4 == 'S') || (pos3 == 'S' && pos4 == 'M'));
                    }) ? 1 : 0));
    }
    
    private static int PassAggregate<T>(this T value, Func<T, int> func) => func(value);
}