/**
 * Graph algorithms and data structures for complex problem solving
 */

namespace Utility;

/// <summary>
/// Graph utilities for topological sorting and graph construction
/// </summary>
public static class GraphUtilities
{
  public static Dictionary<int, List<int>> BuildGraph(List<int> update, List<(int X, int Y)> rules)
  {
    var graph = update.ToDictionary(page => page, page => new List<int>());

    foreach ((int x, int y) in rules)
    {
      if (graph.ContainsKey(x) && graph.ContainsKey(y))
      {
        graph[x].Add(y);
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
