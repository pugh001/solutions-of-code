namespace Utility;

public interface IDistanceCalculable<T>
{
  double DistanceTo(T other);
}

public class Connection<T>(T pointA, T pointB) where T : IDistanceCalculable<T>
{
  public T PointA { get; set; } = pointA;
  public T PointB { get; set; } = pointB;
  public double Distance { get; set; } = pointA.DistanceTo(pointB);

  public override string ToString()
  {
    return $"{PointA} <-> {PointB} (Distance: {Distance:F2})";
  }
}
