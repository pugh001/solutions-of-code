// Specialized Graph class for pathfinding and maze analysis

namespace Utility;

/// <summary>
/// Graph class specialized for maze/pathfinding problems with start/end points
/// </summary>
public class Graph
{
  // Constructor to initialize graph and calculate initial paths
  public Graph(List<string> lines)
  {
    Map = new Map(lines);
    Start = Map.FindAll('S').FirstOrDefault() ?? throw new Exception("Start not found");
    End = Map.FindAll('E').FirstOrDefault() ?? throw new Exception("End not found");

    var walls = FindInnerWalls('#');
    (int steps, var parents) = FindShortestPath(Start, End);

    SavedP1 = CalculateSavedWalls(walls, parents);
    SavedP2 = CalculateSavedPaths(parents, steps);
  }

  private Map Map { get; }
  private int[] Start { get; }
  private int[] End { get; }
  public List<int> SavedP1 { get; private set; }
  public List<int> SavedP2 { get; private set; }

  // Calculate savings by removing specific walls
  private List<int> CalculateSavedWalls(List<int[]> walls, HashSet<(int Cost, int Row, int Col)> parents)
  {
    return walls.Select(wall =>
    {
      var directions = Map.GetDirectionPositions(wall, "4");
      var costs = directions.Select(dir =>
      {
        var matches = parents.Where(p => p.Row == dir[0] && p.Col == dir[1]).ToList();
        return matches.Count > 0 ?
          matches.First().Cost :
          -1;
      }).ToList();

      return costs.All(c => c >= 0) ?
        Math.Abs(costs.Max() - costs.Min()) :
        0;
    }).ToList();
  }

  // Calculate savings based on path adjustments
  private List<int> CalculateSavedPaths(HashSet<(int Cost, int Row, int Col)> parents, int steps)
  {
    var result = new List<int>();

    foreach (var parent1 in parents)
    {
      foreach (var parent2 in parents)
      {
        if (parent1 == parent2) continue;

        int distance = Math.Abs(parent1.Row - parent2.Row) + Math.Abs(parent1.Col - parent2.Col);
        if (distance > steps) continue;

        int saved = steps - (distance + parent1.Cost + parent2.Cost);
        result.Add(saved);
      }
    }

    return result;
  }

  // Find the shortest path between start and end positions
  private (int Steps, HashSet<(int Cost, int Row, int Col)> Parents) FindShortestPath(int[] start, int[] end)
  {
    var queue = new Queue<(int Cost, int Row, int Col)>();
    var visited = new HashSet<(int, int)>();
    var parents = new HashSet<(int Cost, int Row, int Col)>();

    queue.Enqueue((0, start[0], start[1]));
    visited.Add((start[0], start[1]));

    while (queue.Count > 0)
    {
      (int cost, int row, int col) = queue.Dequeue();

      if (row == end[0] && col == end[1])
      {
        parents.Add((cost, row, col));
        return (cost, parents);
      }

      var neighbors = Map.GetDirectionPositions([row, col], "4")
        .Where(pos => !visited.Contains((pos[0], pos[1])) && Map.OnBoard(pos));

      foreach (int[]? neighbor in neighbors)
      {
        visited.Add((neighbor[0], neighbor[1]));
        queue.Enqueue((cost + 1, neighbor[0], neighbor[1]));
        parents.Add((cost + 1, neighbor[0], neighbor[1]));
      }
    }

    return (-1, parents);
  }

  // Find all inner walls on the map
  private List<int[]> FindInnerWalls(char wallChar)
  {
    return Map.FindAll(wallChar).Where(pos => pos[0] > 0 && pos[0] < Map.Rows - 1 && pos[1] > 0 && pos[1] < Map.Columns - 1)
      .ToList();
  }
  /// <summary>
    /// Builds a graph from a list of nodes and dependency rules
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="nodes">Collection of nodes to include in graph</param>
    /// <param name="rules">Dependency rules as (parent, child) tuples</param>
    /// <returns>Dictionary representing adjacency list graph structure</returns>
    public static Dictionary<T, List<T>> BuildGraph<T>(IEnumerable<T> nodes, IEnumerable<(T X, T Y)> rules) where T : notnull
    {
        var graph = new Dictionary<T, List<T>>();
        
        // Initialize all nodes with empty adjacency lists
        foreach (var node in nodes)
        {
            graph[node] = new List<T>();
        }
        
        // Add edges based on rules
        foreach (var (x, y) in rules)
        {
            if (graph.ContainsKey(x) && graph.ContainsKey(y))
            {
                graph[x].Add(y);
            }
        }
        
        return graph;
    }

    /// <summary>
    /// Performs topological sort on a graph using Kahn's algorithm
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <param name="nodes">Collection of nodes to sort</param>
    /// <returns>Topologically sorted list of nodes</returns>
    public static List<T> TopologicalSort<T>(Dictionary<T, List<T>> graph, IEnumerable<T> nodes) where T : notnull
    {
        var result = new List<T>();
        var inDegree = new Dictionary<T, int>();
        var queue = new Queue<T>();
        
        // Calculate in-degrees
        foreach (var node in nodes)
        {
            inDegree[node] = 0;
        }
        
        foreach (var kvp in graph)
        {
            foreach (var neighbor in kvp.Value)
            {
                if (inDegree.ContainsKey(neighbor))
                {
                    inDegree[neighbor]++;
                }
            }
        }
        
        // Find all nodes with no incoming edges
        foreach (var kvp in inDegree)
        {
            if (kvp.Value == 0)
            {
                queue.Enqueue(kvp.Key);
            }
        }
        
        // Process nodes
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            
            if (graph.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (inDegree.ContainsKey(neighbor))
                    {
                        inDegree[neighbor]--;
                        if (inDegree[neighbor] == 0)
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// Performs depth-first search traversal
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <param name="start">Starting node for traversal</param>
    /// <param name="visited">Optional set to track visited nodes</param>
    /// <returns>List of nodes visited during DFS</returns>
    public static List<T> DepthFirstSearch<T>(Dictionary<T, List<T>> graph, T start, HashSet<T>? visited = null) where T : notnull
    {
        visited ??= new HashSet<T>();
        var result = new List<T>();
        var stack = new Stack<T>();
        
        stack.Push(start);
        
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            
            if (!visited.Add(current))
                continue;
                
            result.Add(current);
            
            if (graph.TryGetValue(current, out var neighbors))
            {
                // Push in reverse order to maintain left-to-right traversal
                for (int i = neighbors.Count - 1; i >= 0; i--)
                {
                    if (!visited.Contains(neighbors[i]))
                    {
                        stack.Push(neighbors[i]);
                    }
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// Performs breadth-first search traversal
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <param name="start">Starting node for traversal</param>
    /// <returns>List of nodes visited during BFS</returns>
    public static List<T> BreadthFirstSearch<T>(Dictionary<T, List<T>> graph, T start) where T : notnull
    {
        var visited = new HashSet<T>();
        var result = new List<T>();
        var queue = new Queue<T>();
        
        queue.Enqueue(start);
        visited.Add(start);
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            
            if (graph.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (visited.Add(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// Checks if the graph contains cycles using DFS
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <returns>True if graph contains cycles, false otherwise</returns>
    public static bool HasCycle<T>(Dictionary<T, List<T>> graph) where T : notnull
    {
        var visited = new HashSet<T>();
        var recursionStack = new HashSet<T>();
        
        foreach (var node in graph.Keys)
        {
            if (!visited.Contains(node))
            {
                if (HasCycleDFS(graph, node, visited, recursionStack))
                    return true;
            }
        }
        
        return false;
    }
    
    private static bool HasCycleDFS<T>(Dictionary<T, List<T>> graph, T node, HashSet<T> visited, HashSet<T> recursionStack) where T : notnull
    {
        visited.Add(node);
        recursionStack.Add(node);
        
        if (graph.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    if (HasCycleDFS(graph, neighbor, visited, recursionStack))
                        return true;
                }
                else if (recursionStack.Contains(neighbor))
                {
                    return true;
                }
            }
        }
        
        recursionStack.Remove(node);
        return false;
    }

}
