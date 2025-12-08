/**
 * Core navigation and neighbor utilities for coordinate systems
 */

namespace Utility;

public static class NavigationUtilities
{
  public static CompassDirection Turn(this CompassDirection value, string turnDir, int degrees = 90)
  {
    return turnDir.ToLower() switch
    {
      "l" or "ccw" => (CompassDirection)(((int)value - degrees + 360) % 360),
      "r" or "cw" => (CompassDirection)(((int)value + degrees) % 360),
      _ => throw new ArgumentException("Value must be L, R, CCW, or CW", nameof(turnDir))
    };
  }

  public static T GetDirection<T>(this Dictionary<(int, int), T> values,
    (int, int) location,
    CompassDirection direction,
    T defaultVal)
  {
    var n = location.MoveDirection(direction);
    return values.GetValueOrDefault(n, defaultVal);
  }

  public static T GetDirection<T>(this Dictionary<Coordinate2D, T> values,
    Coordinate2D location,
    CompassDirection direction,
    T defaultVal)
  {
    var n = location.MoveDirection(direction);
    return values.GetValueOrDefault(n, defaultVal);
  }

  public static List<Coordinate2D> Neighbors(this Coordinate2D val, bool includeDiagonals = false)
  {
    var tmp = new List<Coordinate2D>
    {
      new(val.X - 1, val.Y),
      new(val.X + 1, val.Y),
      new(val.X, val.Y - 1),
      new(val.X, val.Y + 1)
    };
    if (includeDiagonals)
    {
      tmp.AddRange(new List<Coordinate2D>
      {
        new(val.X - 1, val.Y - 1),
        new(val.X + 1, val.Y - 1),
        new(val.X - 1, val.Y + 1),
        new(val.X + 1, val.Y + 1)
      });
    }

    return tmp;
  }

  public static IEnumerable<Coordinate3D> GetImmediateNeighbors(this Coordinate3D self)
  {
    yield return (self.X + 1, self.Y, self.Z);
    yield return (self.X - 1, self.Y, self.Z);
    yield return (self.X, self.Y + 1, self.Z);
    yield return (self.X, self.Y - 1, self.Z);
    yield return (self.X, self.Y, self.Z + 1);
    yield return (self.X, self.Y, self.Z - 1);
  }
}
