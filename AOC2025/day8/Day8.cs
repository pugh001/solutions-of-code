using Utility;
using Utility.Algorithms;

namespace AOC2025;

public class Day8
{

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

  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    var connectionsToProcess = 1000;
    if (input.Contains("Example")) connectionsToProcess = 10;
    
    List<Coordinate3D> space = data.Select(line => line.Split(',').ToIntArray()).Select(xyz => new Coordinate3D(xyz[0], xyz[1], xyz[2])).ToList();
    
    // Track which coordinates have been connected
    Dictionary<Coordinate3D, bool> connectedCoordinates = space.ToDictionary(coord => coord, coord => false);
    // Calculate all possible connections using GetCombinations (more elegant)
    var allConnections = Algorithms.GetCombinations(space, 2)
      .Select(pair => new Connection(pair[0], pair[1]))
      .OrderBy(c => c.Distance)
      .ToList();

     List<ConnectionChain> chains = new List<ConnectionChain>();
    int connectionsProcessed = 0;
    int part1Product = -1;
    int part2Product = -1;
    
    foreach (var connection in allConnections)
    {
      if (connectionsProcessed >= connectionsToProcess && part1Product == -1)
      {
         part1Product = Part1Product(chains);
      }
      bool connectionWasProcessed = ProcessConnection(connection, chains, connectedCoordinates);
      if (connectionWasProcessed && chains.Count == 1 && AllCoordinatesConnected(connectedCoordinates))
      {
        part2Product = connection.PointA.X * connection.PointB.X;
       break;
      }

      connectionsProcessed++;
    }
    
    


    string part1 = $"{part1Product}";
    string part2 = $"{part2Product}";
    
    return (part1, part2);
  }
  private static int Part1Product(List<ConnectionChain> chains)
  {
    // Sort chains by connected points count (descending) and get the top 3
    var topChainsBySize = chains.OrderByDescending(c => c.ConnectedPoints.Count).Take(3).ToList();
    // Calculate part1 as the product of the top 3 chains' connected points count
    int part1Product = topChainsBySize.Select(c => c.ConnectedPoints.Count).Aggregate(1, (acc, count) => acc * count);
    return part1Product;
  }

  private bool ProcessConnection(Connection connection, List<ConnectionChain> chains, Dictionary<Coordinate3D, bool> connectedCoordinates)
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
      case 1 when chainsWithPointB.Count == 1 && 
                  chainsWithPointA[0] != chainsWithPointB[0]:
        MergeChains(connection, chainsWithPointA[0], chainsWithPointB[0], chains);
        MarkCoordinatesAsConnected(connection, connectedCoordinates);
        return true;
      default:
        // If both points are in the same chain, nothing happens
        return false;
    }

  }

  private static List<ConnectionChain> GetChainsContaining(Coordinate3D point, List<ConnectionChain> chains)
  {
    return chains.Where(c => c.ConnectedPoints.Contains(point)).ToList();
  }

  private static void CreateNewChain(Connection connection, List<ConnectionChain> chains)
  {
    var newChain = new ConnectionChain();
    newChain.AddConnection(connection);
    chains.Add(newChain);
  }

  private static void MergeChains(Connection connection, ConnectionChain chainA, ConnectionChain chainB, List<ConnectionChain> chains)
  {
    // Add the connection to chainA
    chainA.AddConnection(connection);
    
    // Merge chainB into chainA
    chainA.MergeChain(chainB);
    
    // Remove chainB from the list
    chains.Remove(chainB);
  }

  private static void MarkCoordinatesAsConnected(Connection connection, Dictionary<Coordinate3D, bool> connectedCoordinates)
  {
    connectedCoordinates[connection.PointA] = true;
    connectedCoordinates[connection.PointB] = true;
  }

  private static bool AllCoordinatesConnected(Dictionary<Coordinate3D, bool> connectedCoordinates)
  {
    return connectedCoordinates.Values.All(connected => connected);
  }
}
