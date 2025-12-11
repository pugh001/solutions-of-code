namespace Utility;

public class Connection<T>(T pointA,
  T pointB) where T : IDistanceCalculable<T>
{
  public T PointA { get; set; } = pointA;
  public T PointB { get; set; } = pointB;
  public double EuclideanDistance { get; set; } = pointA.EuclideanDistance(pointB);

  public override string ToString()
  {
    return $"{PointA} <-> {PointB} (Distance: {EuclideanDistance:F2})";
  }
}