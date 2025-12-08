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
    
    List<Coordinate3D> space = new List<Coordinate3D>();
    
    // Load all coordinates
    foreach (var line in data)
    {
      var xyz = line.Split(',').ToIntArray();
      space.Add(new Coordinate3D(xyz[0], xyz[1], xyz[2]));
    }

    // Calculate all possible connections using GetCombinations (more elegant)
    var allConnections = Algorithms.GetCombinations(space, 2)
      .Select(pair => new Connection(pair[0], pair[1]))
      .OrderBy(c => c.Distance)
      .ToList();

    // Build connection chains, processing exactly the specified number of shortest connections
    List<ConnectionChain> chains = new List<ConnectionChain>();
    int connectionsProcessed = 0;
    
    foreach (var connection in allConnections)
    {
      if (connectionsProcessed >= connectionsToProcess) break;
      
      // Find chains that contain each point
      var chainsWithPointA = chains.Where(c => c.ConnectedPoints.Contains(connection.PointA)).ToList();
      var chainsWithPointB = chains.Where(c => c.ConnectedPoints.Contains(connection.PointB)).ToList();
      
      if (chainsWithPointA.Count == 0 && chainsWithPointB.Count == 0)
      {
        // Neither point exists - create new chain
        var newChain = new ConnectionChain();
        newChain.AddConnection(connection);
        chains.Add(newChain);
      }
      else if (chainsWithPointA.Count == 1 && chainsWithPointB.Count == 0)
      {
        // Only point A exists - extend that chain
        chainsWithPointA[0].AddConnection(connection);
      }
      else if (chainsWithPointA.Count == 0 && chainsWithPointB.Count == 1)
      {
        // Only point B exists - extend that chain
        chainsWithPointB[0].AddConnection(connection);
      }
      else if (chainsWithPointA.Count == 1 && chainsWithPointB.Count == 1 && 
               chainsWithPointA[0] != chainsWithPointB[0])
      {
        // Points are in different chains - merge them
        var chainA = chainsWithPointA[0];
        var chainB = chainsWithPointB[0];
        
        // Add the connection to chainA
        chainA.AddConnection(connection);
        
        // Merge chainB into chainA
        chainA.MergeChain(chainB);
        
        // Remove chainB from the list
        chains.Remove(chainB);
      }
      // If both points are in the same chain, "nothing happens!" but we still count this connection as processed
      
      // Always count this connection as processed, regardless of whether it changed the circuit structure
      connectionsProcessed++;
    }
    
    // Sort chains by connected points count (descending) and get the top 3
    var topChainsBySize = chains.OrderByDescending(c => c.ConnectedPoints.Count).Take(3).ToList();
    
    // Calculate part1 as the product of the top 3 chains' connected points count
    int part1Product = topChainsBySize.Select(c => c.ConnectedPoints.Count).Aggregate(1, (acc, count) => acc * count);
    
    // Sort chains by total distance and get the 3 shortest for part2
    var shortestChains = chains.OrderBy(c => c.TotalDistance).Take(3).ToList();
    
    // Count unique connections in the 3 shortest chains
    var uniqueConnections = new HashSet<Connection>();
    foreach (var chain in shortestChains)
    {
      foreach (var connection in chain.Connections)
      {
        uniqueConnections.Add(connection);
      }
    }
    
    string part1 = $"{part1Product}";
    string part2 = $"3 shortest chains have {uniqueConnections.Count} unique connections";
    
    return (part1, part2);
  }
}
