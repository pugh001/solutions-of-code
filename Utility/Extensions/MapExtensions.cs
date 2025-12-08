/**
 * Map generation and coordinate mapping utilities
 */

using System.Text;

namespace Utility;

public static class MapExtensions
{
  public static (Dictionary<Coordinate2D, char> map, int maxX, int maxY) GenerateMap(this string self, bool discardDot = true)
  {
    var lines = self.SplitByNewline();
    int maxX = 0;
    int maxY = lines.Count - 1;
    Dictionary<Coordinate2D, char> res = new();

    for (int i = 0; i < lines.Count; i++)
    {
      maxX = Math.Max(maxX, lines[i].Length - 1);
      for (int j = 0; j < lines[i].Length; j++)
      {
        if (!discardDot || lines[i][j] != '.')
        {
          res[(j, i)] = lines[i][j];
        }
      }
    }

    return (res, maxX, maxY);
  }

  public static string StringFromMap<TValue>(this Dictionary<Coordinate2D, TValue> self,
    int maxX,
    int maxY,
    bool assumeEmptyIsDot = true)
  {
    StringBuilder sb = new();
    for (int y = 0; y <= maxY; y++)
    {
      for (int x = 0; x <= maxX; x++)
      {
        if (self.TryGetValue((x, y), out var val))
        {
          sb.Append(val);
        }
        else if (assumeEmptyIsDot)
        {
          sb.Append(".");
        }
        else
        {
          sb.Append(string.Empty);
        }
      }

      sb.Append("\n");
    }

    return sb.ToString();
  }

  public static Dictionary<Point2D<int>, char> Cells(this string str, Func<char, bool> filter = null)
  {
    return str.Cells(c => c, filter);
  }

  public static Dictionary<Point2D<int>, T> Cells<T>(this string str, Func<char, T> selector, Func<char, bool> filter = null)
  {
    return str.Lines().SelectMany((l, y) => l.Select((c, x) => (x, y, c))).Where(n => filter?.Invoke(n.c) ?? true)
      .ToDictionary(n => new Point2D<int>(n.x, n.y), n => selector(n.c));
  }

  public static StringMap<T> AsMap<T>(this string str, Func<char, T> selector)
  {
    return new StringMap<T>(str, selector);
  }

  public static StringMap<char> AsMap(this string str)
  {
    return new StringMap<char>(str, c => c);
  }
}
