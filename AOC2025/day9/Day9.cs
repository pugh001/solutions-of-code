using Utility;

namespace AOC2025;

public class Day9
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);

    // Parse all points using the Point2D.Parse utility
    var points = data.Where(line => !string.IsNullOrWhiteSpace(line)).Select(Point2D<long>.Parse).ToList();

    // Part 1: Find the pair of points with the maximum Manhattan distance
    var (pointA, pointB, maxDistance) = points.Combinations(2).Select(pair =>
    {
      var pairList = pair.ToList();
      var p1 = pairList[0];
      var p2 = pairList[1];
      var distance = p1.ManhattanDistance(p2);
      return (p1, p2, distance);
    }).MaxBy(x => x.distance);

    // Calculate the area of the axis-aligned bounding rectangle
    long rectangleArea = (Math.Abs(pointA.X - pointB.X) + 1) * (Math.Abs(pointA.Y - pointB.Y) + 1);

    // Part 2: Find largest rectangle that fits entirely within the polygon boundary
    long part2Area = FindLargestRectangleInPolygon(points);

    return (rectangleArea.ToString(), part2Area.ToString());
  }

  private static long FindLargestRectangleInPolygon(List<Point2D<long>> polygonCorners)
  {
    // Don't pre-fill the entire polygon - that's too slow for large areas
    // Instead, validate rectangles directly against the polygon boundary
    
    long maxArea = 0;

    // Process largest rectangles first for early termination
    var candidates = polygonCorners.Combinations(2)
      .Select(pair => 
      {
        var pairList = pair.ToList();
        var corner1 = pairList[0];
        var corner2 = pairList[1];
        var area = (Math.Abs(corner1.X - corner2.X) + 1) * (Math.Abs(corner1.Y - corner2.Y) + 1);
        return (corner1, corner2, area);
      })
      .OrderByDescending(x => x.area);

    foreach (var (corner1, corner2, area) in candidates)
    {
      // Early termination
      if (area <= maxArea) break;
      
      // Fast validation for rectilinear polygons
      if (IsRectangleInRectilinearPolygon(corner1, corner2, polygonCorners))
      {
        maxArea = area;
      }
    }

    return maxArea;
  }

  private static bool IsRectangleInRectilinearPolygon(Point2D<long> corner1, Point2D<long> corner2, List<Point2D<long>> corners)
  {
    long minX = Math.Min(corner1.X, corner2.X);
    long maxX = Math.Max(corner1.X, corner2.X);
    long minY = Math.Min(corner1.Y, corner2.Y);
    long maxY = Math.Max(corner1.Y, corner2.Y);

    // For rectilinear polygons, we can use a much faster approach:
    // Check if the rectangle is "carved out" by the polygon boundary
    
    // First, check if all 4 corners are valid (inside or on boundary)
    var rectCorners = new[]
    {
      new Point2D<long>(minX, minY),
      new Point2D<long>(maxX, minY),
      new Point2D<long>(minX, maxY),
      new Point2D<long>(maxX, maxY)
    };
    
    foreach (var corner in rectCorners)
    {
      if (!IsPointInOrOnRectilinearPolygon(corner, corners))
      {
        return false;
      }
    }
    
    // Check that no polygon edge cuts through the rectangle interior
    // For rectilinear polygons, this is much simpler
    for (int i = 0; i < corners.Count; i++)
    {
      var start = corners[i];
      var end = corners[(i + 1) % corners.Count];
      
      if (DoesRectilinearEdgeIntersectRectangle(start, end, minX, maxX, minY, maxY))
      {
        return false;
      }
    }
    
    return true;
  }

  private static bool IsPointInOrOnRectilinearPolygon(Point2D<long> point, List<Point2D<long>> corners)
  {
    // Quick boundary check first
    for (int i = 0; i < corners.Count; i++)
    {
      var start = corners[i];
      var end = corners[(i + 1) % corners.Count];
      
      // Check if point is on this edge (only horizontal/vertical lines)
      if (start.X == end.X && point.X == start.X) // Vertical edge
      {
        long minY = Math.Min(start.Y, end.Y);
        long maxY = Math.Max(start.Y, end.Y);
        if (point.Y >= minY && point.Y <= maxY)
        {
          return true;
        }
      }
      else if (start.Y == end.Y && point.Y == start.Y) // Horizontal edge
      {
        long minX = Math.Min(start.X, end.X);
        long maxX = Math.Max(start.X, end.X);
        if (point.X >= minX && point.X <= maxX)
        {
          return true;
        }
      }
    }
    
    // Simple ray casting for interior (optimized for rectilinear)
    int intersections = 0;
    for (int i = 0; i < corners.Count; i++)
    {
      var start = corners[i];
      var end = corners[(i + 1) % corners.Count];
      
      // Only count vertical edges that cross our horizontal ray
      if (start.X == end.X && start.X > point.X)
      {
        long minY = Math.Min(start.Y, end.Y);
        long maxY = Math.Max(start.Y, end.Y);
        if (point.Y >= minY && point.Y < maxY)
        {
          intersections++;
        }
      }
    }
    
    return intersections % 2 == 1;
  }

  private static bool DoesRectilinearEdgeIntersectRectangle(Point2D<long> start, Point2D<long> end, long minX, long maxX, long minY, long maxY)
  {
    // For rectilinear edges, intersection is much simpler
    if (start.X == end.X) // Vertical edge
    {
      long x = start.X;
      long edgeMinY = Math.Min(start.Y, end.Y);
      long edgeMaxY = Math.Max(start.Y, end.Y);
      
      // Check if vertical edge cuts through rectangle
      return x > minX && x < maxX && edgeMinY < maxY && edgeMaxY > minY;
    }
    else if (start.Y == end.Y) // Horizontal edge
    {
      long y = start.Y;
      long edgeMinX = Math.Min(start.X, end.X);
      long edgeMaxX = Math.Max(start.X, end.X);
      
      // Check if horizontal edge cuts through rectangle
      return y > minY && y < maxY && edgeMinX < maxX && edgeMaxX > minX;
    }
    
    return false;
  }

}


