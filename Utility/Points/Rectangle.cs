using System.Numerics;

namespace Utility;

/// <summary>
///   Represents an axis-aligned rectangle with generic numeric coordinates
/// </summary>
public record Rectangle<T> where T : INumber<T>
{

  public Rectangle(T minX, T maxX, T minY, T maxY)
  {
    MinX = minX;
    MaxX = maxX;
    MinY = minY;
    MaxY = maxY;
  }

  public Rectangle(Point2D<T> corner1, Point2D<T> corner2)
  {
    MinX = corner1.X < corner2.X ?
      corner1.X :
      corner2.X;
    MaxX = corner1.X > corner2.X ?
      corner1.X :
      corner2.X;
    MinY = corner1.Y < corner2.Y ?
      corner1.Y :
      corner2.Y;
    MaxY = corner1.Y > corner2.Y ?
      corner1.Y :
      corner2.Y;
  }
  public T MinX { get; }
  public T MaxX { get; }
  public T MinY { get; }
  public T MaxY { get; }

  /// <summary>
  ///   Calculates the area of the rectangle
  /// </summary>
  public T Area => (MaxX - MinX + T.One) * (MaxY - MinY + T.One);

  /// <summary>
  ///   Gets all four corners of the rectangle
  /// </summary>
  public IEnumerable<Point2D<T>> Corners
  {
    get
    {
      yield return new Point2D<T>(MinX, MinY);
      yield return new Point2D<T>(MaxX, MinY);
      yield return new Point2D<T>(MinX, MaxY);
      yield return new Point2D<T>(MaxX, MaxY);
    }
  }

  /// <summary>
  ///   Checks if a point is inside or on the boundary of the rectangle
  /// </summary>
  public bool Contains(Point2D<T> point)
  {
    return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
  }

  /// <summary>
  ///   Checks if this rectangle intersects with another rectangle
  /// </summary>
  public bool Intersects(Rectangle<T> other)
  {
    return !(MaxX < other.MinX || MinX > other.MaxX || MaxY < other.MinY || MinY > other.MaxY);
  }
}