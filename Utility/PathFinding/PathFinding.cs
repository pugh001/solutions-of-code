namespace Utility.PathFinding;

/// <summary>
///   Common pathfinding and graph traversal algorithms for AOC problems
/// </summary>
public static class PathFinding
{
  /// <summary>
  ///   Performs breadth-first search to find shortest path distances from a start point
  /// </summary>
  public static Dictionary<Point2D<int>, int> BFS(Point2D<int> start, Func<Point2D<int>, bool> isValidMove)
  {
    var distances = new Dictionary<Point2D<int>, int>();
    var queue = new Queue<Point2D<int>>();

    distances[start] = 0;
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
      var current = queue.Dequeue();
      int currentDistance = distances[current];

      foreach (var neighbor in current.Neighbours())
      {
        if (!isValidMove(neighbor) || distances.ContainsKey(neighbor))
          continue;

        distances[neighbor] = currentDistance + 1;
        queue.Enqueue(neighbor);
      }
    }

    return distances;
  }

  /// <summary>
  ///   Performs BFS in a grid context
  /// </summary>
  public static Dictionary<Point2D<int>, int> BFS(Grid.Grid grid, Point2D<int> start, Func<char, bool> isValidCell)
  {
    return BFS(start, point => grid.IsInBounds(point) && isValidCell(grid[point]));
  }

  /// <summary>
  ///   Finds shortest path between two points using BFS
  /// </summary>
  public static List<Point2D<int>>? FindPath(Point2D<int> start, Point2D<int> end, Func<Point2D<int>, bool> isValidMove)
  {
    var distances = new Dictionary<Point2D<int>, int>();
    var parents = new Dictionary<Point2D<int>, Point2D<int>>();
    var queue = new Queue<Point2D<int>>();

    distances[start] = 0;
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
      var current = queue.Dequeue();

      if (current.Equals(end))
      {
        // Reconstruct path
        var path = new List<Point2D<int>>();
        var node = end;

        while (!node.Equals(start))
        {
          path.Add(node);
          node = parents[node];
        }

        path.Add(start);
        path.Reverse();
        return path;
      }

      foreach (var neighbor in current.Neighbours())
      {
        if (!isValidMove(neighbor) || distances.ContainsKey(neighbor))
          continue;

        distances[neighbor] = distances[current] + 1;
        parents[neighbor] = current;
        queue.Enqueue(neighbor);
      }
    }

    return null; // No path found
  }

  /// <summary>
  ///   Dijkstra's algorithm for weighted graphs
  /// </summary>
  public static Dictionary<Point2D<int>, int> Dijkstra(Point2D<int> start,
    Func<Point2D<int>, Point2D<int>, int> getCost,
    Func<Point2D<int>, bool> isValidMove)
  {
    var distances = new Dictionary<Point2D<int>, int>();
    var visited = new HashSet<Point2D<int>>();
    var priorityQueue = new PriorityQueue<Point2D<int>, int>();

    distances[start] = 0;
    priorityQueue.Enqueue(start, 0);

    while (priorityQueue.Count > 0)
    {
      var current = priorityQueue.Dequeue();

      if (visited.Contains(current))
        continue;

      visited.Add(current);

      foreach (var neighbor in current.Neighbours())
      {
        if (!isValidMove(neighbor) || visited.Contains(neighbor))
          continue;

        int newDistance = distances[current] + getCost(current, neighbor);

        if (!distances.ContainsKey(neighbor) || newDistance < distances[neighbor])
        {
          distances[neighbor] = newDistance;
          priorityQueue.Enqueue(neighbor, newDistance);
        }
      }
    }

    return distances;
  }

  /// <summary>
  ///   A* pathfinding algorithm
  /// </summary>
  public static List<Point2D<int>>? AStar(Point2D<int> start,
    Point2D<int> goal,
    Func<Point2D<int>, bool> isValidMove,
    Func<Point2D<int>, Point2D<int>, int> getCost = null,
    Func<Point2D<int>, Point2D<int>, int> getHeuristic = null)
  {
    getCost ??= (_, _) => 1;
    getHeuristic ??= (a, b) => a.ManhattanDistance(b);

    var openSet = new PriorityQueue<Point2D<int>, int>();
    var cameFrom = new Dictionary<Point2D<int>, Point2D<int>>();
    var gScore = new Dictionary<Point2D<int>, int>();
    var fScore = new Dictionary<Point2D<int>, int>();

    gScore[start] = 0;
    fScore[start] = getHeuristic(start, goal);
    openSet.Enqueue(start, fScore[start]);

    while (openSet.Count > 0)
    {
      var current = openSet.Dequeue();

      if (current.Equals(goal))
      {
        // Reconstruct path
        var path = new List<Point2D<int>>();
        var node = goal;

        while (cameFrom.ContainsKey(node))
        {
          path.Add(node);
          node = cameFrom[node];
        }

        path.Add(start);
        path.Reverse();
        return path;
      }

      foreach (var neighbor in current.Neighbours())
      {
        if (!isValidMove(neighbor))
          continue;

        int tentativeGScore = gScore[current] + getCost(current, neighbor);

        if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
        {
          cameFrom[neighbor] = current;
          gScore[neighbor] = tentativeGScore;
          fScore[neighbor] = gScore[neighbor] + getHeuristic(neighbor, goal);
          openSet.Enqueue(neighbor, fScore[neighbor]);
        }
      }
    }

    return null; // No path found
  }

  /// <summary>
  ///   Flood fill algorithm to find all connected cells
  /// </summary>
  public static HashSet<Point2D<int>> FloodFill(Point2D<int> start, Func<Point2D<int>, bool> isValidCell)
  {
    var visited = new HashSet<Point2D<int>>();
    var queue = new Queue<Point2D<int>>();

    queue.Enqueue(start);
    visited.Add(start);

    while (queue.Count > 0)
    {
      var current = queue.Dequeue();

      foreach (var neighbor in current.Neighbours())
      {
        if (!isValidCell(neighbor) || visited.Contains(neighbor))
          continue;

        visited.Add(neighbor);
        queue.Enqueue(neighbor);
      }
    }

    return visited;
  }

  /// <summary>
  ///   Flood fill in a grid context
  /// </summary>
  public static HashSet<Point2D<int>> FloodFill(Grid.Grid grid, Point2D<int> start, char targetChar)
  {
    if (!grid.IsInBounds(start) || grid[start] != targetChar)
      return new HashSet<Point2D<int>>();

    return FloodFill(start, point => grid.IsInBounds(point) && grid[point] == targetChar);
  }
}