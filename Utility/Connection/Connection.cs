namespace Utility;

public class Connection(Coordinate3D pointA,
  Coordinate3D pointB)
{
  public Coordinate3D PointA { get; set; } = pointA;
  public Coordinate3D PointB { get; set; } = pointB;
  public double Distance { get; set; } = pointA.EuclideanDistance(pointB);

  public override string ToString()
  {
    return $"{PointA} <-> {PointB} (Distance: {Distance:F2})";
  }
}