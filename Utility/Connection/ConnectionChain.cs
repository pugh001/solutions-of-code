namespace Utility;

public class ConnectionChain
{
  public List<Connection> Connections { get; set; } = new List<Connection>();
  public HashSet<Coordinate3D> ConnectedPoints { get; set; } = new HashSet<Coordinate3D>();
  public double TotalDistance => Connections.Sum(c => c.Distance);

  public bool CanConnect(Connection connection)
  {
    // Can connect if one of the points is already in the chain and the other isn't, OR if it's the first connection
    bool hasPointA = ConnectedPoints.Contains(connection.PointA);
    bool hasPointB = ConnectedPoints.Contains(connection.PointB);
    return (hasPointA && !hasPointB) || (!hasPointA && hasPointB) || ConnectedPoints.Count == 0;
  }

  public void AddConnection(Connection connection)
  {
    Connections.Add(connection);
    ConnectedPoints.Add(connection.PointA);
    ConnectedPoints.Add(connection.PointB);
  }

  public void MergeChain(ConnectionChain otherChain)
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
