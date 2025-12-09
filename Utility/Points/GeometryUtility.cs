using System.Numerics;

namespace Utility;

/// <summary>
/// Utility class for geometric calculations on point collections
/// </summary>
public static class GeometryUtility
{
    /// <summary>
    /// Finds the pair of points with the maximum Manhattan distance
    /// </summary>
    public static (Point2D<T> pointA, Point2D<T> pointB, T distance) FindMaxManhattanDistancePair<T>(IEnumerable<Point2D<T>> points) 
        where T : INumber<T>
    {
        return points.Combinations(2)
            .Select(pair =>
            {
                var pairList = pair.ToList();
                var p1 = pairList[0];
                var p2 = pairList[1];
                var distance = p1.ManhattanDistance(p2);
                return (p1, p2, distance);
            })
            .MaxBy(x => x.distance);
    }

    /// <summary>
    /// Calculates the area of an axis-aligned bounding rectangle for two points
    /// </summary>
    public static T CalculateAxisAlignedBoundingRectangleArea<T>(Point2D<T> pointA, Point2D<T> pointB) 
        where T : INumber<T>
    {
        var rectangle = new Rectangle<T>(pointA, pointB);
        return rectangle.Area;
    }
}
