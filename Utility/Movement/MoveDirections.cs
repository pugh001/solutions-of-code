namespace Utility;

public static class MoveDirections
{
  private const CompassDirection N = CompassDirection.N;
  private const CompassDirection S = CompassDirection.S;
  private const CompassDirection E = CompassDirection.E;
  private const CompassDirection W = CompassDirection.W;
  private const CompassDirection NE = CompassDirection.NE;
  private const CompassDirection NW = CompassDirection.NW;
  private const CompassDirection SE = CompassDirection.SE;
  private const CompassDirection SW = CompassDirection.SW;

  public static CompassDirection CompassDirectionFromArrow(char c)
  {
    return c switch
    {
      '^' => CompassDirection.N,
      'v' => CompassDirection.S,
      '<' => CompassDirection.W,
      '>' => CompassDirection.E,
      _ => throw new Exception("No Direction")
    };
  }
  public static CompassDirection Flip(this CompassDirection dir)
  {
    return dir switch
    {
      N => S,
      S => N,
      E => W,
      W => E,
      NE => SW,
      SW => NE,
      SE => NW,
      NW => SE,
      _ => throw new ArgumentException()
    };
  }
  public static (int x, int y) MoveDirection(this (int, int) start,
    CompassDirection Direction,
    bool flipY = false,
    int distance = 1)
  {
    if (flipY)
    {
      return Direction switch
      {
        N => start.Add((0, -distance)),
        NE => start.Add((distance, -distance)),
        E => start.Add((distance, 0)),
        SE => start.Add((distance, distance)),
        S => start.Add((0, distance)),
        SW => start.Add((-distance, distance)),
        W => start.Add((-distance, 0)),
        NW => start.Add((-distance, -distance)),
        _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
      };
    }

    return Direction switch
    {
      N => start.Add((0, distance)),
      NE => start.Add((distance, distance)),
      E => start.Add((distance, 0)),
      SE => start.Add((distance, -distance)),
      S => start.Add((0, -distance)),
      SW => start.Add((-distance, -distance)),
      W => start.Add((-distance, 0)),
      NW => start.Add((-distance, distance)),
      _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
    };
  }

  public static Coordinate2D MoveDirection(this Coordinate2D start,
    CompassDirection Direction,
    bool flipY = false,
    int distance = 1)
  {
    if (flipY)
    {
      return Direction switch
      {
        N => start + (0, -distance),
        NE => start + (distance, -distance),
        E => start + (distance, 0),
        SE => start + (distance, distance),
        S => start + (0, distance),
        SW => start + (-distance, distance),
        W => start + (-distance, 0),
        NW => start + (-distance, -distance),
        _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
      };
    }

    return Direction switch
    {
      N => start + (0, distance),
      NE => start + (distance, distance),
      E => start + (distance, 0),
      SE => start + (distance, -distance),
      S => start + (0, -distance),
      SW => start + (-distance, -distance),
      W => start + (-distance, 0),
      NW => start + (-distance, distance),
      _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
    };
  }
  public static List<T> Get2dNeighborVals<T>(this Dictionary<(int, int), T> values,
    (int, int) location,
    T defaultVal,
    bool includeDiagonals = false)
  {
    List<T> res =
    [
      values.GetDirection(location, N, defaultVal),
      values.GetDirection(location, E, defaultVal),
      values.GetDirection(location, S, defaultVal),
      values.GetDirection(location, W, defaultVal)
    ];

    if (includeDiagonals)
    {
      res.Add(values.GetDirection(location, NW, defaultVal));
      res.Add(values.GetDirection(location, NE, defaultVal));
      res.Add(values.GetDirection(location, SE, defaultVal));
      res.Add(values.GetDirection(location, SW, defaultVal));
    }


    return res;
  }

  public static List<T> Get2dNeighborVals<T>(this Dictionary<Coordinate2D, T> values,
    Coordinate2D location,
    T defaultVal,
    bool includeDiagonals = false)
  {
    List<T> res = new()
    {
      values.GetDirection(location, N, defaultVal),
      values.GetDirection(location, E, defaultVal),
      values.GetDirection(location, S, defaultVal),
      values.GetDirection(location, W, defaultVal)
    };

    if (includeDiagonals)
    {
      res.Add(values.GetDirection(location, NW, defaultVal));
      res.Add(values.GetDirection(location, NE, defaultVal));
      res.Add(values.GetDirection(location, SE, defaultVal));
      res.Add(values.GetDirection(location, SW, defaultVal));
    }

    return res;
  }
}