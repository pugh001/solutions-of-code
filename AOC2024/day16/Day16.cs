using Utility;

namespace AOC2024;

public class Day16
{
  private static (Dictionary<Coordinate2D, char> map, int maxX, int maxY) _map;
  private static Coordinate2D _start = new(0, 0);
  private static Coordinate2D _end = new(0, 0);
  private static int _shortest = int.MaxValue;
  private static readonly Queue<(Coordinate2D, CompassDirection, int, List<Coordinate2D>)> moves = new();
  private static readonly Dictionary<(Coordinate2D, CompassDirection), int> seen = new();
  private static readonly Dictionary<long, List<Coordinate2D>> Routes = new();

  public (string, string) Process(string input)
  {
    string? data = File.ReadAllText(input);
    long result1 = 0;
    long result2 = 0;
    _map = data.GenerateMap(false);
    _start = _map.map.First(q => q.Value == 'S').Key;
    _end = _map.map.First(q => q.Value == 'E').Key;
    result1 = ProcessPart1();
    result2 = ProcessInput2();
    return (result1.ToString(), result2.ToString());

    ;
  }
  private static long ProcessPart1()
  {

    long sum = 0;
    //ShortestPath(Start, CompassDirection.N, 0, new List<Coordinate2D>());
    moves.Enqueue((_start, CompassDirection.E, 0, [_start]));

    while (moves.TryDequeue(out var move))
    {
      if (move.Item1.X > -1 && move.Item1.Y > -1 && move.Item1.X < _map.maxX && move.Item1.Y < _map.maxX)
      {
        if (move.Item3 > _shortest)
        {
          continue;
        }

        if (_map.map[move.Item1] == 'E')
        {
          if (move.Item3 <= _shortest)
          {
            _shortest = move.Item3;
            if (!Routes.TryAdd(move.Item3, move.Item4))
            {
              Routes[move.Item3] = Routes[move.Item3].Union(move.Item4).Distinct().ToList();
            }
          }

          continue;
        }

        if (_map.map[move.Item1] != '#')
        {
          var movement = move.Item1.MoveDirection(move.Item2);
          var newlist = new List<Coordinate2D>(move.Item4);
          newlist.Add(movement);
          AddIfShorter((movement, move.Item2, move.Item3 + 1, newlist));
        }

        var cw = TurnClockwise(move.Item2);
        var ccw = TurnCounterClockwise(move.Item2);
        AddIfShorter((move.Item1, cw, move.Item3 + 1000, move.Item4));
        AddIfShorter((move.Item1, ccw, move.Item3 + 1000, move.Item4));
      }
    }

    return _shortest;
  }

  private static void AddIfShorter((Coordinate2D, CompassDirection, int, List<Coordinate2D>) cur)
  {
    if (!seen.TryAdd((cur.Item1, cur.Item2), cur.Item3))
    {
      if (seen[(cur.Item1, cur.Item2)] >= cur.Item3)
      {
        seen[(cur.Item1, cur.Item2)] = cur.Item3;
        moves.Enqueue(cur);
      }
    }
    else
    {
      moves.Enqueue(cur);
    }
  }
  private static CompassDirection TurnClockwise(CompassDirection currentDirection)
  {
    return currentDirection switch
    {
      CompassDirection.N => CompassDirection.E,
      CompassDirection.E => CompassDirection.S,
      CompassDirection.S => CompassDirection.W,
      CompassDirection.W => CompassDirection.N
    };
  }

  private static CompassDirection TurnCounterClockwise(CompassDirection currentDirection)
  {
    return currentDirection switch
    {
      CompassDirection.N => CompassDirection.W,
      CompassDirection.E => CompassDirection.N,
      CompassDirection.S => CompassDirection.E,
      CompassDirection.W => CompassDirection.S
    };
  }
  private void ShortestPath(Coordinate2D cur, CompassDirection dir, int visited, List<Coordinate2D> seenRoutes)
  {
    //if (seenRoutes.Contains(cur)) return;
    //if (map.map[cur] == '#') return;
    if (cur == _end)
    {
      if (visited < _shortest)
      {
        _shortest = visited;
      }

      return;
    }

    var move = cur.MoveDirection(dir);
    ShortestPath(move, dir, visited + 1, seenRoutes);
    var cw = TurnClockwise(dir);
    ShortestPath(cur.MoveDirection(cw), cw, visited + 1001, seenRoutes);
    var ccw = TurnCounterClockwise(dir);
    ShortestPath(cur.MoveDirection(ccw), ccw, visited + 1001, seenRoutes);
  }

  private static long ProcessInput2()
  {
    long sum = 0;
    sum = Routes[Routes.Keys.Min()].Distinct().Count();
    return sum;
  }
}