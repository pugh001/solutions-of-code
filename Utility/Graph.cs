// Refactored and Cleaned Code for Map and Graph functionality

namespace Utility;

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

      var neighbors = Map.GetDirectionPositions(new[] { row, col }, "4")
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

  public static Dictionary<int, List<int>> BuildGraph(List<int> update, List<(int X, int Y)> rules)
  {
    var graph = update.ToDictionary(page => page, page => new List<int>());

    foreach ((int X, int Y) in rules)
    {
      if (graph.ContainsKey(X) && graph.ContainsKey(Y))
      {
        graph[X].Add(Y);
      }
    }

    return graph;
  }
  public static List<int> TopologicalSort(Dictionary<int, List<int>> graph, List<int> nodes)
  {
    var inDegree = graph.ToDictionary(kvp => kvp.Key, kvp => 0);

    foreach (var neighbors in graph.Values)
    {
      foreach (int neighbor in neighbors)
      {
        inDegree[neighbor]++;
      }
    }

    var queue = new Queue<int>(nodes.Where(node => inDegree[node] == 0));
    var sorted = new List<int>();

    while (queue.Count > 0)
    {
      int current = queue.Dequeue();
      sorted.Add(current);

      foreach (int neighbor in graph[current])
      {
        inDegree[neighbor]--;
        if (inDegree[neighbor] == 0)
        {
          queue.Enqueue(neighbor);
        }
      }
    }

    return sorted;
  }
}