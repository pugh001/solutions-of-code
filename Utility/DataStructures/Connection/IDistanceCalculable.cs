namespace Utility;

public interface IDistanceCalculable<T>
{
  double EuclideanDistance(T other);
}