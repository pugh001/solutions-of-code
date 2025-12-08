using Utility;

namespace AOC2025;

public class Day8
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    var connectionsToProcess = 1000;
    if (input.Contains("Example")) connectionsToProcess = 10;
    
    List<Coordinate3D> space = data.Select(line => line.Split(',').ToIntArray())
      .Select(xyz => new Coordinate3D(xyz[0], xyz[1], xyz[2])).ToList();
    
    // Calculate all possible connections using GetCombinations
    var allConnections = Algorithms.GetCombinations(space, 2)
      .Select(pair => new Connection<Coordinate3D>(pair[0], pair[1]))
      .OrderBy(c => c.EuclideanDistance)
      .ToList();

    // Build connection chains using utility class
    var (chains, _, _) = ConnectionChainBuilder.BuildChains(allConnections, space, connectionsToProcess);
    int part1Product = CalculatePart1Product(chains);
    
    // Continue processing to find when all coordinates form a single circuit
    var (_, connectionsProcessed, allConnected) = ConnectionChainBuilder.BuildChains(allConnections, space);
    
    int part2Product = -1;
    if (allConnected)
    {
      // Find the connection that completed the circuit
      var connectedCoordinates = space.ToDictionary(coord => coord, coord => false);
      var workingChains = new List<ConnectionChain<Coordinate3D>>();
        
      for (int i = 0; i < connectionsProcessed; i++)
      {
        var connection = allConnections[i];
        bool wasProcessed = ConnectionChainBuilder.ProcessConnection(connection, workingChains, connectedCoordinates);

        if (wasProcessed && workingChains.Count == 1 && connectedCoordinates.Values.All(c => c))
        {
          part2Product = connection.PointA.X * connection.PointB.X;
          break;
        }
      }
    }
    else
    {
      // Alternative approach: find the connection that would complete the circuit
      // when all coordinates become connected for the first time
      var connectedCoordinates = space.ToDictionary(coord => coord, coord => false);
      var workingChains = new List<ConnectionChain<Coordinate3D>>();
      
      foreach (var connection in allConnections)
      {
        bool wasProcessed = ConnectionChainBuilder.ProcessConnection(connection, workingChains, connectedCoordinates);
        
        if (wasProcessed && workingChains.Count == 1 && connectedCoordinates.Values.All(c => c))
        {
          part2Product = connection.PointA.X * connection.PointB.X;
          break;
        }
      }
    }

    return (part1Product.ToString(), part2Product.ToString());
  }

  private static int CalculatePart1Product(List<ConnectionChain<Coordinate3D>> chains)
  {
    var topChainsBySize = chains.OrderByDescending(c => c.ConnectedPoints.Count).Take(3).ToList();
    return topChainsBySize.Select(c => c.ConnectedPoints.Count).Aggregate(1, (acc, count) => acc * count);
  }
}
