namespace Utility;

/// <summary>
///   Utility class for building and managing connection chains from a set of connections
/// </summary>
public class ConnectionChainBuilder
{
  /// <summary>
  ///   Builds connection chains by processing connections in order until a specified count or completion
  /// </summary>
  /// <param name="connections">Ordered list of connections to process</param>
  /// <param name="coordinates">All coordinates that should eventually be connected</param>
  /// <param name="maxConnections">Maximum number of connections to process (optional)</param>
  /// <returns>Tuple of (chains, connectionsProcessed, allConnected)</returns>
  public static (List<ConnectionChain<T>> chains, int connectionsProcessed, bool allConnected) BuildChains<T>(
    List<Connection<T>> connections,
    List<T> coordinates,
    int? maxConnections = null) where T : IDistanceCalculable<T>
  {
    var chains = new List<ConnectionChain<T>>();
    var connectedCoordinates = coordinates.ToDictionary(coord => coord, coord => false);
    int connectionsProcessed = 0;

    foreach (var connection in connections)
    {
      if (maxConnections.HasValue && connectionsProcessed >= maxConnections.Value)
        break;

      ProcessConnection(connection, chains, connectedCoordinates);
      connectionsProcessed++;

      // Check if all coordinates are connected in a single circuit
      if (chains.Count == 1 && AllCoordinatesConnected(connectedCoordinates))
      {
        return (chains, connectionsProcessed, true);
      }
    }

    return (chains, connectionsProcessed, AllCoordinatesConnected(connectedCoordinates));
  }

  /// <summary>
  ///   Process a single connection and update chains accordingly
  /// </summary>
  public static bool ProcessConnection<T>(Connection<T> connection,
    List<ConnectionChain<T>> chains,
    Dictionary<T, bool> connectedCoordinates) where T : IDistanceCalculable<T>
  {
    var chainsWithPointA = GetChainsContaining(connection.PointA, chains);
    var chainsWithPointB = GetChainsContaining(connection.PointB, chains);

    switch (chainsWithPointA.Count)
    {
      // Neither point exists - create new chain
      case 0 when chainsWithPointB.Count == 0:
        CreateNewChain(connection, chains);
        MarkCoordinatesAsConnected(connection, connectedCoordinates);
        return true;

      // Only point A exists - extend that chain
      case 1 when chainsWithPointB.Count == 0:
        chainsWithPointA[0].AddConnection(connection);
        MarkCoordinatesAsConnected(connection, connectedCoordinates);
        return true;

      // Only point B exists - extend that chain
      case 0 when chainsWithPointB.Count == 1:
        chainsWithPointB[0].AddConnection(connection);
        MarkCoordinatesAsConnected(connection, connectedCoordinates);
        return true;

      // Points are in different chains - merge them
      case 1 when chainsWithPointB.Count == 1 && chainsWithPointA[0] != chainsWithPointB[0]:
        MergeChains(connection, chainsWithPointA[0], chainsWithPointB[0], chains);
        MarkCoordinatesAsConnected(connection, connectedCoordinates);
        return true;

      default:
        // If both points are in the same chain, nothing happens
        return false;
    }
  }

  private static List<ConnectionChain<T>> GetChainsContaining<T>(T point, List<ConnectionChain<T>> chains)
    where T : IDistanceCalculable<T>
  {
    return chains.Where(c => c.ConnectedPoints.Contains(point)).ToList();
  }

  private static void CreateNewChain<T>(Connection<T> connection, List<ConnectionChain<T>> chains)
    where T : IDistanceCalculable<T>
  {
    var newChain = new ConnectionChain<T>();
    newChain.AddConnection(connection);
    chains.Add(newChain);
  }

  private static void MergeChains<T>(Connection<T> connection,
    ConnectionChain<T> chainA,
    ConnectionChain<T> chainB,
    List<ConnectionChain<T>> chains) where T : IDistanceCalculable<T>
  {
    chainA.AddConnection(connection);
    chainA.MergeChain(chainB);
    chains.Remove(chainB);
  }

  private static void MarkCoordinatesAsConnected<T>(Connection<T> connection, Dictionary<T, bool> connectedCoordinates)
    where T : IDistanceCalculable<T>
  {
    connectedCoordinates[connection.PointA] = true;
    connectedCoordinates[connection.PointB] = true;
  }

  private static bool AllCoordinatesConnected<T>(Dictionary<T, bool> connectedCoordinates) where T : IDistanceCalculable<T>
  {
    return connectedCoordinates.Values.All(connected => connected);
  }
}