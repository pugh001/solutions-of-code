namespace Utility;

public static class MoveDirections
{
  private const CompassDirection N = CompassDirection.N;
  private const CompassDirection S = CompassDirection.S;
  private const CompassDirection E = CompassDirection.E;
  private const CompassDirection W = CompassDirection.W;
  private const CompassDirection Ne = CompassDirection.Ne;
  private const CompassDirection Nw = CompassDirection.Nw;
  private const CompassDirection Se = CompassDirection.Se;
  private const CompassDirection Sw = CompassDirection.Sw;

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
      Ne => Sw,
      Sw => Ne,
      Se => Nw,
      Nw => Se,
      _ => throw new ArgumentException()
    };
  }
  public static (int x, int y) MoveDirection(this (int, int) start,
    CompassDirection direction,
    bool flipY = false,
    int distance = 1)
  {
    if (flipY)
    {
      return direction switch
      {
        N => start.Add((0, -distance)),
        Ne => start.Add((distance, -distance)),
        E => start.Add((distance, 0)),
        Se => start.Add((distance, distance)),
        S => start.Add((0, distance)),
        Sw => start.Add((-distance, distance)),
        W => start.Add((-distance, 0)),
        Nw => start.Add((-distance, -distance)),
        _ => throw new ArgumentException("Direction is not valid", nameof(direction))
      };
    }

    return direction switch
    {
      N => start.Add((0, distance)),
      Ne => start.Add((distance, distance)),
      E => start.Add((distance, 0)),
      Se => start.Add((distance, -distance)),
      S => start.Add((0, -distance)),
      Sw => start.Add((-distance, -distance)),
      W => start.Add((-distance, 0)),
      Nw => start.Add((-distance, distance)),
      _ => throw new ArgumentException("Direction is not valid", nameof(direction))
    };
  }

  public static Coordinate2D MoveDirection(this Coordinate2D start,
    CompassDirection direction,
    bool flipY = false,
    int distance = 1)
  {
    if (flipY)
    {
      return direction switch
      {
        N => start + (0, -distance),
        Ne => start + (distance, -distance),
        E => start + (distance, 0),
        Se => start + (distance, distance),
        S => start + (0, distance),
        Sw => start + (-distance, distance),
        W => start + (-distance, 0),
        Nw => start + (-distance, -distance),
        _ => throw new ArgumentException("Direction is not valid", nameof(direction))
      };
    }

    return direction switch
    {
      N => start + (0, distance),
      Ne => start + (distance, distance),
      E => start + (distance, 0),
      Se => start + (distance, -distance),
      S => start + (0, -distance),
      Sw => start + (-distance, -distance),
      W => start + (-distance, 0),
      Nw => start + (-distance, distance),
      _ => throw new ArgumentException("Direction is not valid", nameof(direction))
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
      res.Add(values.GetDirection(location, Nw, defaultVal));
      res.Add(values.GetDirection(location, Ne, defaultVal));
      res.Add(values.GetDirection(location, Se, defaultVal));
      res.Add(values.GetDirection(location, Sw, defaultVal));
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
      res.Add(values.GetDirection(location, Nw, defaultVal));
      res.Add(values.GetDirection(location, Ne, defaultVal));
      res.Add(values.GetDirection(location, Se, defaultVal));
      res.Add(values.GetDirection(location, Sw, defaultVal));
    }

    return res;
  }
}