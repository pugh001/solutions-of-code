namespace Utility;

public class ConnectionChain<T> where T : IDistanceCalculable<T>
{
  public List<Connection<T>> Connections { get; set; } = new();
  public HashSet<T> ConnectedPoints { get; set; } = new();
  public double TotalDistance => Connections.Sum(c => c.EuclideanDistance);

  public bool CanConnect(Connection<T> connection)
  {
    // Can connect if one of the points is already in the chain and the other isn't, OR if it's the first connection
    bool hasPointA = ConnectedPoints.Contains(connection.PointA);
    bool hasPointB = ConnectedPoints.Contains(connection.PointB);
    return hasPointA && !hasPointB || !hasPointA && hasPointB || ConnectedPoints.Count == 0;
  }

  public void AddConnection(Connection<T> connection)
  {
    Connections.Add(connection);
    ConnectedPoints.Add(connection.PointA);
    ConnectedPoints.Add(connection.PointB);
  }

  public void MergeChain(ConnectionChain<T> otherChain)
  {
    Connections.AddRange(otherChain.Connections);
    foreach (var point in otherChain.ConnectedPoints)
    {
      ConnectedPoints.Add(point);
    }
  }

  public override string ToString()
  {
    return $"Chain with {Connections.Count} connections, Total Distance: {TotalDistance:F2}";
  }
}