namespace Utility;

/// <summary>
///   Common pathfinding and graph traversal algorithms for AOC problems
/// </summary>
public static class PathFinding
{
  /// <summary>
  ///   Performs breadth-first search to find shortest path distances from a start point
  /// </summary>
  public static Dictionary<Point2D<int>, int> Bfs(Point2D<int> start, Func<Point2D<int>, bool> isValidMove)
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
  public static Dictionary<Point2D<int>, int> Bfs(Grid grid, Point2D<int> start, Func<char, bool> isValidCell)
  {
    return Bfs(start, point => grid.IsInBounds(point) && isValidCell(grid[point]));
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
  ///   A* pathfinding algorithm using Point2D
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
  ///   A* pathfinding algorithm using Coordinate2D for compatibility
  /// </summary>
  public static List<Coordinate2D> AStar(Coordinate2D start,
    Coordinate2D goal,
    Dictionary<Coordinate2D, long> map,
    out long cost,
    bool includeDiagonals = false,
    bool includePath = true)
  {
    PriorityQueue<Coordinate2D, long> openSet = new();
    Dictionary<Coordinate2D, Coordinate2D> cameFrom = new();

    Dictionary<Coordinate2D, long> gScore = new()
    {
      [start] = 0
    };

    openSet.Enqueue(start, 0);

    while (openSet.TryDequeue(out var cur, out long _))
    {
      if (cur.Equals(goal))
      {
        cost = gScore[cur];
        return includePath ?
          ReconstructPath(cameFrom, cur) :
          null;
      }

      foreach (var n in cur.Neighbors(includeDiagonals).Where(a => map.ContainsKey(a)))
      {
        long tentGScore = gScore[cur] + map[n];
        if (tentGScore < gScore.GetValueOrDefault(n, int.MaxValue))
        {
          cameFrom[n] = cur;
          gScore[n] = tentGScore;
          openSet.Enqueue(n, tentGScore + cur.ManDistance(goal));
        }
      }
    }

    cost = long.MaxValue;
    return null;
  }

  private static List<Coordinate2D> ReconstructPath(Dictionary<Coordinate2D, Coordinate2D> cameFrom, Coordinate2D current)
  {
    List<Coordinate2D> res = [current];
    while (cameFrom.ContainsKey(current))
    {
      current = cameFrom[current];
      res.Add(current);
    }

    res.Reverse();
    return res;
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
  public static HashSet<Point2D<int>> FloodFill(Grid grid, Point2D<int> start, char targetChar)
  {
    if (!grid.IsInBounds(start) || grid[start] != targetChar)
      return new HashSet<Point2D<int>>();

    return FloodFill(start, point => grid.IsInBounds(point) && grid[point] == targetChar);
  }

  /// <summary>
  ///   Counts all distinct paths from start to any position that satisfies the end condition
  ///   Uses backtracking to explore all possible paths without revisiting nodes in the same path
  /// </summary>
  /// <param name="start">Starting position</param>
  /// <param name="isValidMove">Function to validate if a move from current to next position is allowed</param>
  /// <param name="isEndCondition">Function to check if current position satisfies the end condition</param>
  /// <param name="getNeighbors">Function to get valid neighbors of current position</param>
  /// <param name="visitedPaths">Set to track visited positions in current path (for backtracking)</param>
  /// <returns>Number of distinct valid paths</returns>
  public static long CountDistinctPaths(Point2D<int> start,
    Func<Point2D<int>, Point2D<int>, bool> isValidMove,
    Func<Point2D<int>, bool> isEndCondition,
    Func<Point2D<int>, IEnumerable<Point2D<int>>> getNeighbors,
    HashSet<Point2D<int>>? visitedPaths = null)
  {
    visitedPaths ??= new HashSet<Point2D<int>>();

    if (isEndCondition(start))
      return 1;

    long distinctPaths = 0;

    foreach (var neighbor in getNeighbors(start))
    {
      if (isValidMove(start, neighbor) && !visitedPaths.Contains(neighbor))
      {
        visitedPaths.Add(neighbor);
        distinctPaths += CountDistinctPaths(neighbor, isValidMove, isEndCondition, getNeighbors, visitedPaths);
        visitedPaths.Remove(neighbor);
      }
    }

    return distinctPaths;
  }

  /// <summary>
  ///   Counts all distinct paths in a grid from start to positions satisfying end condition
  /// </summary>
  /// <param name="grid">The grid to traverse</param>
  /// <param name="start">Starting position</param>
  /// <param name="isValidMove">Function to validate moves between adjacent positions</param>
  /// <param name="isEndCondition">Function to check if position satisfies end condition</param>
  /// <returns>Number of distinct valid paths</returns>
  public static long CountDistinctPaths(Grid grid,
    Point2D<int> start,
    Func<Point2D<int>, Point2D<int>, bool> isValidMove,
    Func<Point2D<int>, bool> isEndCondition)
  {
    return CountDistinctPaths(start, isValidMove, isEndCondition, grid.GetNeighbors);
  }
}