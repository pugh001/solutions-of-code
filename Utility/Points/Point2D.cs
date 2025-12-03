using System.Globalization;
using System.Numerics;

namespace Utility;

public record Point2D<T>(T X,
  T Y) : IComparable<Point2D<T>> where T : INumber<T>, INumberBase<T>
{
  private static readonly T TwoT = T.One + T.One;
  public static readonly Point2D<T> Origin = (T.Zero, T.Zero);
  public static readonly Point2D<T> Up = (T.Zero, -T.One);
  public static readonly Point2D<T> Down = (T.Zero, T.One);
  public static readonly Point2D<T> Left = (-T.One, T.Zero);
  public static readonly Point2D<T> Right = (T.One, T.Zero);

  public int CompareTo(Point2D<T> other)
  {
    var dy = Y - other.Y;
    if (dy == T.Zero)
      return T.Sign(X - other.X);

    return T.Sign(dy);
  }

  public override string ToString()
  {
    return $"{X},{Y}";
  }

  public static implicit operator (T, T)(Point2D<T> pt)
  {
    return (pt.X, pt.Y);
  }
  public static implicit operator Point2D<T>((T, T) t)
  {
    return new Point2D<T>(t.Item1, t.Item2);
  }

  public static Point2D<T> operator -(Point2D<T> a)
  {
    return new Point2D<T>(-a.X, -a.Y);
  }
  public static Point2D<T> operator -(Point2D<T> a, Point2D<T> b)
  {
    return new Point2D<T>(a.X - b.X, a.Y - b.Y);
  }
  public static Point2D<T> operator +(Point2D<T> a, Point2D<T> b)
  {
    return new Point2D<T>(a.X + b.X, a.Y + b.Y);
  }
  public static Point2D<T> operator *(Point2D<T> a, T b)
  {
    return new Point2D<T>(a.X * b, a.Y * b);
  }
  public static Point2D<T> operator *(T a, Point2D<T> b)
  {
    return b * a;
  }
  public static Point2D<T> operator /(Point2D<T> a, T b)
  {
    return new Point2D<T>(a.X / b, a.Y / b);
  }
  public static Point2D<T> operator %(Point2D<T> a, T b)
  {
    return new Point2D<T>(a.X % b, a.Y % b);
  }
  public static Point2D<T> operator %(Point2D<T> a, Point2D<T> b)
  {
    return new Point2D<T>(a.X % b.X, a.Y % b.Y);
  }

  public IEnumerable<Point2D<T>> Neighbours(bool withDiagonal = false)
  {
    yield return this + Right;
    yield return this + Down;
    yield return this + Up;
    yield return this + Left;

    if (withDiagonal)
    {
      yield return this + Up + Left;
      yield return this + Up + Right;
      yield return this + Down + Left;
      yield return this + Down + Right;
    }
  }
  public IEnumerable<Point2D<T>> DiagonalNeighbours()
  {
    yield return this + Up + Left;
    yield return this + Up + Right;
    yield return this + Down + Left;
    yield return this + Down + Right;
  }

  public T ManhattanDistance(Point2D<T> other)
  {
    return T.Abs(other.X - X) + T.Abs(other.Y - Y);
  }

  public static IEnumerable<Point2D<T>> Range(Point2D<T> min, Point2D<T> max)
  {
    var (minX, maxX) = min.X < max.X ?
      (min.X, max.X) :
      (max.X, min.X);
    var (minY, maxY) = min.Y < max.Y ?
      (min.Y, max.Y) :
      (max.Y, min.Y);
    for (var x = minX; x <= maxX; x++)
      for (var y = minY; y <= maxY; y++)
        yield return new Point2D<T>(x, y);
  }

  public static Point2D<T> Parse(string s)
  {
    string[] parts = s.Split(',');
    var fp = CultureInfo.InvariantCulture;
    return new Point2D<T>(T.Parse(parts[0], fp), T.Parse(parts[1], fp));
  }

  /// <summary>
  ///   Maps '<', '>', '^' and 'v' into directions. Any other char returns (0,0)
  /// </summary>
  /// <param name="c"></param>
  /// <returns></returns>
  public static Point2D<T> DirectionFromArrow(char c)
  {
    return c switch
    {
      '^' => Up,
      'v' => Down,
      '<' => Left,
      '>' => Right,
      _ => Origin
    };
  }
}