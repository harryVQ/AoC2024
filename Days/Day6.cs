namespace AoC2024.Days;

public static class Day6
{
    private const char Blockade = '#';

    private static readonly Dictionary<char, (int dx, int dy)> Moves = new()
    {
        { '^', (0, -1) }, { 'v', (0, 1) }, { '<', (-1, 0) }, { '>', (1, 0) }
    };

    private static readonly Dictionary<char, char> NextDirection = new()
    {
        { '^', '>' }, { '>', 'v' }, { 'v', '<' }, { '<', '^' }
    };

    public static int Part1(string input)
    {
        var lines = input.Split('\n');
        var width = lines[0].Length;
        var height = lines.Length;

        return lines
            .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
            .Aggregate(
                new { blockades = new HashSet<(int, int)>(), Guard = (Dir: '.', X: 0, Y: 0) },
                (aggregateData, currentPos) => currentPos.c switch
                {
                    Blockade => aggregateData with { blockades = [..aggregateData.blockades, (currentPos.x, currentPos.y)] },
                    var c when Moves.ContainsKey(c) => aggregateData with { Guard = (c, currentPos.x, currentPos.y) },
                    _ => aggregateData
                })
            .PassAggregate(info => Enumerable.Range(0, int.MaxValue)
                .Aggregate(
                    new
                    {
                        Pos = (info.Guard.X, info.Guard.Y),
                        info.Guard.Dir,
                        Visited = new HashSet<(int X, int Y)> { (info.Guard.X, info.Guard.Y) }
                    },
                    (aggregateData, _) =>
                    {
                        var next = (X: aggregateData.Pos.X + Moves[aggregateData.Dir].dx, Y: aggregateData.Pos.Y + Moves[aggregateData.Dir].dy);

                        if (next.X < 0 || next.X >= width || next.Y < 0 || next.Y >= height) throw new Exception(aggregateData.Visited.Count.ToString());

                        if (info.blockades.Contains(next))
                        {
                            var newDir = NextDirection[aggregateData.Dir];
                            return aggregateData with { Dir = newDir };
                        }

                        aggregateData.Visited.Add(next);
                        return aggregateData with { Pos = next };
                    }
                ).Visited.Count);
    }

    public static int Part2(string input)
    {
        var lines = input.Split('\n');
        var width = lines[0].Length;
        var height = lines.Length;

        return lines
            .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
            .Aggregate(
                new { Blockades = new HashSet<(int, int)>(), Guard = (Dir: '.', X: 0, Y: 0) },
                (aggregateData, currentPos) => currentPos.c switch
                {
                    Blockade => aggregateData with { Blockades = [..aggregateData.Blockades, (currentPos.x, currentPos.y)] },
                    var c when Moves.ContainsKey(c) => aggregateData with { Guard = (c, currentPos.x, currentPos.y) },
                    _ => aggregateData
                })
            .PassAggregate(info => Enumerable.Range(0, height)
                .SelectMany(y => Enumerable.Range(0, width).Select(x => (x, y)))
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Count(pos => !info.Blockades.Contains(pos) &&
                              pos != (info.Guard.X, info.Guard.Y) &&
                              ((Func<bool>)(() =>
                              {
                                  try
                                  {
                                      Enumerable.Range(0, int.MaxValue)
                                          .Aggregate(
                                              new
                                              {
                                                  Pos = (info.Guard.X, info.Guard.Y),
                                                  info.Guard.Dir,
                                                  BlockadesHit = new Dictionary<(int, int), List<char>>(),
                                                  Blockades = new HashSet<(int, int)>(info.Blockades) { pos }
                                              },
                                              (aggregateData, _) =>
                                              {
                                                  var next = (X: aggregateData.Pos.X + Moves[aggregateData.Dir].dx, Y: aggregateData.Pos.Y + Moves[aggregateData.Dir].dy);

                                                  if (next.X < 0 || next.X >= width || next.Y < 0 || next.Y >= height) throw new Exception("Left Area");

                                                  if (!aggregateData.Blockades.Contains(next)) return aggregateData with { Pos = next };

                                                  aggregateData.BlockadesHit.TryGetValue((next.X, next.Y), out var hits);
                                                  hits ??= aggregateData.BlockadesHit[(next.X, next.Y)] = [];

                                                  if (hits.Contains(aggregateData.Dir)) throw new Exception("Duplicate Blockade");

                                                  hits.Add(aggregateData.Dir);
                                                  return aggregateData with { Dir = NextDirection[aggregateData.Dir] };
                                              });
                                  }
                                  catch (Exception e) when (e.Message is "Left Area" or "Duplicate Blockade")
                                  {
                                      return e.Message == "Duplicate Blockade";
                                  }

                                  return false;
                              }))()));
    }

    private static int PassAggregate<T>(this T value, Func<T, int> func) => func(value);
}