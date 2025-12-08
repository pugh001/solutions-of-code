using Utility;

namespace Utility;

/// <summary>
/// Example demonstrating how to use the generic Connection classes with different coordinate types
/// </summary>
public class ConnectionExample
{
  public static void Demonstrate()
  {
    // Example using 3D coordinates with generic approach
    var point3D_A = new Coordinate3D(0, 0, 0);
    var point3D_B = new Coordinate3D(3, 4, 0);
    var connection3D = new Connection<Coordinate3D>(point3D_A, point3D_B);
    Console.WriteLine($"3D Connection: {connection3D}");

    // Example using 2D coordinates with the new generic approach
    var point2D_A = new Coordinate2D(0, 0);
    var point2D_B = new Coordinate2D(3, 4);
    var connection2D = new Connection<Coordinate2D>(point2D_A, point2D_B);
    Console.WriteLine($"2D Connection: {connection2D}");

    // Example using Point2D<int>
    var pointGeneric_A = new Point2D<int>(0, 0);
    var pointGeneric_B = new Point2D<int>(3, 4);
    var connectionGeneric = new Connection<Point2D<int>>(pointGeneric_A, pointGeneric_B);
    Console.WriteLine($"Generic Point2D Connection: {connectionGeneric}");

    // Example building chains with 2D coordinates
    var coordinates2D = new List<Coordinate2D>
    {
      new(0, 0), new(1, 0), new(2, 0), new(1, 1)
    };

    var allConnections2D = GetAllConnections(coordinates2D);
    var (chains2D, _, _) = ConnectionChainBuilder.BuildChains(allConnections2D, coordinates2D);
    
    Console.WriteLine($"\n2D Chains built: {chains2D.Count}");
    foreach (var chain in chains2D)
    {
      Console.WriteLine($"  {chain}");
    }

    // Example building chains with generic Point2D
    var pointsGeneric = new List<Point2D<int>>
    {
      new(0, 0), new(1, 0), new(2, 0), new(1, 1)
    };

    var allConnectionsGeneric = GetAllConnections(pointsGeneric);
    var (chainsGeneric, _, _) = ConnectionChainBuilder.BuildChains(allConnectionsGeneric, pointsGeneric);
    
    Console.WriteLine($"\nGeneric Point2D Chains built: {chainsGeneric.Count}");
    foreach (var chain in chainsGeneric)
    {
      Console.WriteLine($"  {chain}");
    }
  }

  /// <summary>
  /// Helper method to generate all possible connections between points
  /// </summary>
  private static List<Connection<T>> GetAllConnections<T>(List<T> points) where T : IDistanceCalculable<T>
  {
    var connections = new List<Connection<T>>();
    
    for (int i = 0; i < points.Count; i++)
    {
      for (int j = i + 1; j < points.Count; j++)
      {
        connections.Add(new Connection<T>(points[i], points[j]));
      }
    }
    
    return connections.OrderBy(c => c.Distance).ToList();
  }
}
