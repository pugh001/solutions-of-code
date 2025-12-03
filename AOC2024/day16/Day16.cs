using Utility;

namespace AOC2024;

public class Day16
{
  private static (Dictionary<Coordinate2D, char> map, int maxX, int maxY) map;
  private static Coordinate2D Start = new(0, 0);
  private static Coordinate2D End = new(0, 0);
  private static int Shortest = int.MaxValue;
  private static readonly Queue<(Coordinate2D, CompassDirection, int, List<Coordinate2D>)> moves = new();
  private static readonly Dictionary<(Coordinate2D, CompassDirection), int> seen = new();
  private static readonly Dictionary<long, List<Coordinate2D>> Routes = new();

  public (string, string) Process(string input)
  {
    string? data = File.ReadAllText(input);
    long result1 = 0;
    long result2 = 0;
    map = data.GenerateMap(false);
    Start = map.map.First(q => q.Value == 'S').Key;
    End = map.map.First(q => q.Value == 'E').Key;
    result1 = ProcessPart1();
    result2 = ProcessInput2();
    return (result1.ToString(), result2.ToString());

    ;
  }
  private static long ProcessPart1()
  {

    long sum = 0;
    //ShortestPath(Start, CompassDirection.N, 0, new List<Coordinate2D>());
    moves.Enqueue((Start, CompassDirection.E, 0, new List<Coordinate2D> { Start }));

    while (moves.TryDequeue(out var move))
    {
      if (move.Item1.x > -1 && move.Item1.y > -1 && move.Item1.x < map.maxX && move.Item1.y < map.maxX)
      {
        if (move.Item3 > Shortest)
        {
          continue;
        }

        if (map.map[move.Item1] == 'E')
        {
          if (move.Item3 <= Shortest)
          {
            Shortest = move.Item3;
            if (!Routes.TryAdd(move.Item3, move.Item4))
            {
              Routes[move.Item3] = Routes[move.Item3].Union(move.Item4).Distinct().ToList();
            }
          }

          continue;
        }

        if (map.map[move.Item1] != '#')
        {
          var movement = move.Item1.MoveDirection(move.Item2);
          var newlist = new List<Coordinate2D>(move.Item4);
          newlist.Add(movement);
          addIfShorter((movement, move.Item2, move.Item3 + 1, newlist));
        }

        var cw = TurnClockwise(move.Item2);
        var ccw = TurnCounterClockwise(move.Item2);
        addIfShorter((move.Item1, cw, move.Item3 + 1000, move.Item4));
        addIfShorter((move.Item1, ccw, move.Item3 + 1000, move.Item4));
      }
    }

    return Shortest;
  }

  private static void addIfShorter((Coordinate2D, CompassDirection, int, List<Coordinate2D>) cur)
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
    if (cur == End)
    {
      if (visited < Shortest)
      {
        Shortest = visited;
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