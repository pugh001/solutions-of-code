using Utility;

namespace AOC2025;

public class Day8
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    int connectionsToProcess = 1000;
    if (input.Contains("Example")) connectionsToProcess = 10;

    var space = new List<Coordinate3D>();
    foreach (string line in data)
    {
      int[] coords = line.Split(',').ToIntArray();
      space.Add(new Coordinate3D(coords[0], coords[1], coords[2]));
    }

    // Calculate all possible connections using GetCombinations
    var combinations = Algorithms.GetCombinations(space, 2);
    var allConnections = new List<Connection<Coordinate3D>>();
    foreach (var pair in combinations)
    {
      allConnections.Add(new Connection<Coordinate3D>(pair[0], pair[1]));
    }

    allConnections.Sort((a, b) => a.EuclideanDistance.CompareTo(b.EuclideanDistance));

    // Build connection chains using utility class
    var (chains, _, _) = ConnectionChainBuilder.BuildChains(allConnections, space, connectionsToProcess);
    int part1Product = CalculatePart1Product(chains);

    // Continue processing to find when all coordinates form a single circuit
    (_, int connectionsProcessed, bool allConnected) = ConnectionChainBuilder.BuildChains(allConnections, space);

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

        bool allCoordinatesConnected = true;
        foreach (bool connected in connectedCoordinates.Values)
        {
          if (!connected)
          {
            allCoordinatesConnected = false;
            break;
          }
        }

        if (wasProcessed && workingChains.Count == 1 && allCoordinatesConnected)
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

        bool allCoordinatesConnected = true;
        foreach (bool connected in connectedCoordinates.Values)
        {
          if (!connected)
          {
            allCoordinatesConnected = false;
            break;
          }
        }

        if (wasProcessed && workingChains.Count == 1 && allCoordinatesConnected)
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
    // Sort chains by size descending and take top 3
    var sortedChains = new List<ConnectionChain<Coordinate3D>>(chains);
    sortedChains.Sort((a, b) => b.ConnectedPoints.Count.CompareTo(a.ConnectedPoints.Count));

    var topChainsBySize = new List<ConnectionChain<Coordinate3D>>();
    for (int i = 0; i < Math.Min(3, sortedChains.Count); i++)
    {
      topChainsBySize.Add(sortedChains[i]);
    }

    // Calculate product
    int product = 1;
    foreach (var chain in topChainsBySize)
    {
      product *= chain.ConnectedPoints.Count;
    }

    return product;
  }
}