using System.Numerics;

namespace Utility;

/// <summary>
///   Utility class for polygon operations, optimized for rectilinear (axis-aligned) polygons
/// </summary>
public static class PolygonUtility
{
    /// <summary>
    ///   Determines if a point is inside or on the boundary of a rectilinear polygon
    /// </summary>
    public static bool IsPointInOrOnRectilinearPolygon<T>(Point2D<T> point, IList<Point2D<T>> corners) where T : INumber<T>
  {
    // Quick boundary check first
    if (IsPointOnPolygonBoundary(point, corners))
    {
      return true;
    }

    // Ray casting for interior points (optimized for rectilinear)
    return IsPointInsidePolygon(point, corners);
  }

    /// <summary>
    ///   Checks if a point lies on the boundary of a rectilinear polygon
    /// </summary>
    public static bool IsPointOnPolygonBoundary<T>(Point2D<T> point, IList<Point2D<T>> corners) where T : INumber<T>
  {
    for (int i = 0; i < corners.Count; i++)
    {
      var start = corners[i];
      var end = corners[(i + 1) % corners.Count];

      if (IsPointOnRectilinearEdge(point, start, end))
      {
        return true;
      }
    }

    return false;
  }

    /// <summary>
    ///   Checks if a point lies on a rectilinear edge (horizontal or vertical line)
    /// </summary>
    public static bool IsPointOnRectilinearEdge<T>(Point2D<T> point, Point2D<T> start, Point2D<T> end) where T : INumber<T>
  {
    // Check if point is on vertical edge
    if (start.X == end.X && point.X == start.X)
    {
      var minY = start.Y < end.Y ?
        start.Y :
        end.Y;
      var maxY = start.Y > end.Y ?
        start.Y :
        end.Y;
      return point.Y >= minY && point.Y <= maxY;
    }

    // Check if point is on horizontal edge
    if (start.Y == end.Y && point.Y == start.Y)
    {
      var minX = start.X < end.X ?
        start.X :
        end.X;
      var maxX = start.X > end.X ?
        start.X :
        end.X;
      return point.X >= minX && point.X <= maxX;
    }

    return false;
  }

    /// <summary>
    ///   Uses ray casting to determine if a point is inside a polygon
    /// </summary>
    public static bool IsPointInsidePolygon<T>(Point2D<T> point, IList<Point2D<T>> corners) where T : INumber<T>
  {
    int intersections = 0;

    for (int i = 0; i < corners.Count; i++)
    {
      var start = corners[i];
      var end = corners[(i + 1) % corners.Count];

      // Only count vertical edges that cross our horizontal ray
      if (start.X == end.X && start.X > point.X)
      {
        var minY = start.Y < end.Y ?
          start.Y :
          end.Y;
        var maxY = start.Y > end.Y ?
          start.Y :
          end.Y;

        if (point.Y >= minY && point.Y < maxY)
        {
          intersections++;
        }
      }
    }

    return intersections % 2 == 1;
  }

    /// <summary>
    ///   Checks if a rectilinear edge cuts through the interior of a rectangle
    /// </summary>
    public static bool DoesRectilinearEdgeIntersectRectangleInterior<T>(Point2D<T> start, Point2D<T> end, Rectangle<T> rectangle)
    where T : INumber<T>
  {
    // For vertical edge
    if (start.X == end.X)
    {
      var x = start.X;
      var edgeMinY = start.Y < end.Y ?
        start.Y :
        end.Y;
      var edgeMaxY = start.Y > end.Y ?
        start.Y :
        end.Y;

      // Check if vertical edge cuts through rectangle
      return x > rectangle.MinX && x < rectangle.MaxX && edgeMinY < rectangle.MaxY && edgeMaxY > rectangle.MinY;
    }

    // For horizontal edge
    if (start.Y == end.Y)
    {
      var y = start.Y;
      var edgeMinX = start.X < end.X ?
        start.X :
        end.X;
      var edgeMaxX = start.X > end.X ?
        start.X :
        end.X;

      // Check if horizontal edge cuts through rectangle
      return y > rectangle.MinY && y < rectangle.MaxY && edgeMinX < rectangle.MaxX && edgeMaxX > rectangle.MinX;
    }

    return false;
  }

    /// <summary>
    ///   Checks if a rectangle fits entirely within a rectilinear polygon
    /// </summary>
    public static bool IsRectangleInRectilinearPolygon<T>(Rectangle<T> rectangle, IList<Point2D<T>> polygonCorners)
    where T : INumber<T>
  {
    // Check if all 4 corners are valid (inside or on boundary)
    foreach (var corner in rectangle.Corners)
    {
      if (!IsPointInOrOnRectilinearPolygon(corner, polygonCorners))
      {
        return false;
      }
    }

    // Check that no polygon edge cuts through the rectangle interior
    for (int i = 0; i < polygonCorners.Count; i++)
    {
      var start = polygonCorners[i];
      var end = polygonCorners[(i + 1) % polygonCorners.Count];

      if (DoesRectilinearEdgeIntersectRectangleInterior(start, end, rectangle))
      {
        return false;
      }
    }

    return true;
  }

    /// <summary>
    ///   Finds the largest rectangle that fits entirely within a polygon
    /// </summary>
    public static T FindLargestRectangleArea<T>(IList<Point2D<T>> polygonCorners) where T : INumber<T>
  {
    var maxArea = T.Zero;

    // Process largest rectangles first for early termination
    var candidates = polygonCorners.Combinations(2).Select(pair =>
    {
      var pairList = pair.ToList();
      var corner1 = pairList[0];
      var corner2 = pairList[1];
      var rectangle = new Rectangle<T>(corner1, corner2);
      return (rectangle, rectangle.Area);
    }).OrderByDescending(x => x.Area);

    foreach (var (rectangle, area) in candidates)
    {
      // Early termination
      if (area <= maxArea)
      {
        break;
      }

      if (IsRectangleInRectilinearPolygon(rectangle, polygonCorners))
      {
        maxArea = area;
      }
    }

    return maxArea;
  }
}