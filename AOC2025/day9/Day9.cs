using Utility;

namespace AOC2025;
//Day  9:(4767418746, 1461987144)  Time:  00:00:01.3956224
public class Day9
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);

    // Parse all points using the Point2D.Parse utility
    var points = data.Where(line => !string.IsNullOrWhiteSpace(line)).Select(Point2D<long>.Parse).ToList();

    // Part 1: Find the pair of points with the maximum Manhattan distance
    var (pointA, pointB, maxDistance) = GeometryUtility.FindMaxManhattanDistancePair(points);

    // Calculate the area of the axis-aligned bounding rectangle
    long rectangleArea = GeometryUtility.CalculateAxisAlignedBoundingRectangleArea(pointA, pointB);

    // Part 2: Find largest rectangle that fits entirely within the polygon boundary
    long part2Area = PolygonUtility.FindLargestRectangleArea(points);

    return (rectangleArea.ToString(), part2Area.ToString());
  }
}


