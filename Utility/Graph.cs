// Specialized Graph class for pathfinding and maze analysis

using System.Drawing;

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
  /// Finds all paths from start node to end node using DFS traversal.
  /// </summary>
  /// <typeparam name="T">Type of graph nodes</typeparam>
  /// <param name="graph">Graph as adjacency list</param>
  /// <param name="start">Start node</param>
  /// <param name="end">End node</param>
  /// <returns>List of paths, each path is a list of nodes from start to end</returns>
  /// <example>
  /// var graph = Graph.BuildGraph(new[] {"A", "B", "C", "D"}, new[] { ("A", "B"), ("A", "C"), ("B", "D"), ("C", "D") });
  /// var allPaths = Graph.FindAllPaths(graph, "A", "D");
  /// // allPaths will be: [ ["A", "B", "D"], ["A", "C", "D"] ]
  /// </example>
  public static List<List<T>> FindAllPaths<T>(Dictionary<T, List<T>> graph, T start, T end) where T : notnull
  {
    var result = new List<List<T>>();
    var path = new List<T>();
    FindAllPathsDfs(graph, start, end, path, result);
    return result;
  }

  private static void FindAllPathsDfs<T>(Dictionary<T, List<T>> graph, T current, T end, List<T> path, List<List<T>> result)
    where T : notnull
  {
    path.Add(current);
    
    if (current.Equals(end))
    {
      result.Add(new List<T>(path));
    }
    else if (graph.TryGetValue(current, out var neighbors))
    {
      foreach (var neighbor in neighbors)
      {
        if (!path.Contains(neighbor)) // avoid cycles
        {
          FindAllPathsDfs(graph, neighbor, end, path, result);
        }
      }
    }

    path.RemoveAt(path.Count - 1);
  }
  /// <summary>
  /// Counts all paths from start to end with memoization for performance
  /// </summary>
  /// <typeparam name="T">Type of graph nodes</typeparam>
  /// <param name="graph">Graph as adjacency list</param>
  /// <param name="start">Start node</param>
  /// <param name="end">End node</param>
  /// <returns>Number of paths from start to end</returns>
  public static int CountPaths<T>(Dictionary<T, List<T>> graph, T start, T end) where T : notnull
  {
    var memo = new Dictionary<(T current, string pathKey), int>();
    var currentPath = new HashSet<T>();
    
    return CountPathsDfs(graph, start, end, currentPath, memo);
  }

  private static int CountPathsDfs<T>(Dictionary<T, List<T>> graph, T current, T end, HashSet<T> currentPath, Dictionary<(T, string), int> memo)
    where T : notnull
  {
    // Check for cycles - if we've already visited this node in current path
    if (currentPath.Contains(current))
      return 0;

    // Create memoization key based on current node and visited path
    var pathKey = string.Join(",", currentPath.OrderBy(x => x));
    var key = (current, pathKey);
    
    if (memo.TryGetValue(key, out int cachedResult))
      return cachedResult;

    if (current.Equals(end))
      return 1;

    currentPath.Add(current);
    int totalPaths = 0;

    if (graph.TryGetValue(current, out var neighbors))
    {
      foreach (var neighbor in neighbors)
      {
        totalPaths += CountPathsDfs(graph, neighbor, end, currentPath, memo);
      }
    }

    currentPath.Remove(current);
    memo[key] = totalPaths;
    return totalPaths;
  }

  /// <summary>
  /// Counts paths from start to end that pass through specific required nodes
  /// </summary>
  /// <typeparam name="T">Type of graph nodes</typeparam>
  /// <param name="graph">Graph as adjacency list</param>
  /// <param name="start">Start node</param>
  /// <param name="end">End node</param>
  /// <param name="requiredNodes">Nodes that must be visited</param>
  /// <returns>Number of paths passing through all required nodes</returns>
  public static int CountPathsWithRequiredNodes<T>(Dictionary<T, List<T>> graph, T start, T end, params T[] requiredNodes) where T : notnull
  {
    var memo = new Dictionary<(T current, string pathKey, string visitedKey), int>();
    var currentPath = new HashSet<T>();
    var visitedRequired = new HashSet<T>();
    
    return CountPathsWithRequiredDfs(graph, start, end, requiredNodes.ToHashSet(), currentPath, visitedRequired, memo);
  }

  /// <summary>
  /// Counts paths from start to any end node that contains endPattern, visiting nodes containing all required patterns
  /// Optimized for problems where you need to visit any node containing specific substrings (like "dac" and "fft")
  /// </summary>
  /// <param name="graph">Graph as adjacency list</param>
  /// <param name="start">Start node</param>
  /// <param name="endPattern">Pattern that end nodes must contain</param>
  /// <param name="requiredPatterns">Patterns that must be visited during the path</param>
  /// <returns>Number of valid paths</returns>
  public static int CountPathsWithPatterns(Dictionary<string, List<string>> graph, string start, string endPattern, params string[] requiredPatterns)
  {
    // Find all possible end nodes
    var endNodes = graph.Keys.Where(node => node.Contains(endPattern)).ToHashSet();
    if (!endNodes.Any()) return 0;

    // Use a more efficient state representation
    var memo = new Dictionary<(string current, int visitedMask), int>();
    var requiredPatternsList = requiredPatterns.ToList();
    
    return CountPathsWithPatternsDfs(graph, start, endNodes, requiredPatternsList, new HashSet<string>(), 0, memo);
  }

  private static int CountPathsWithPatternsDfs(Dictionary<string, List<string>> graph, string current, 
    HashSet<string> endNodes, List<string> requiredPatterns, HashSet<string> currentPath, int visitedMask, 
    Dictionary<(string, int), int> memo)
  {
    // Check for cycles
    if (currentPath.Contains(current))
      return 0;

    var key = (current, visitedMask);
    if (memo.TryGetValue(key, out int cachedResult))
      return cachedResult;

    // Update visited mask for current node
    int newVisitedMask = visitedMask;
    for (int i = 0; i < requiredPatterns.Count; i++)
    {
      if (current.Contains(requiredPatterns[i]))
      {
        newVisitedMask |= (1 << i);
      }
    }

    // Check if we've reached an end node
    if (endNodes.Contains(current))
    {
      // Check if all required patterns have been visited
      int allRequiredMask = (1 << requiredPatterns.Count) - 1;
      return (newVisitedMask == allRequiredMask) ? 1 : 0;
    }

    currentPath.Add(current);
    int totalPaths = 0;

    if (graph.TryGetValue(current, out var neighbors))
    {
      foreach (var neighbor in neighbors)
      {
        totalPaths += CountPathsWithPatternsDfs(graph, neighbor, endNodes, requiredPatterns, currentPath, newVisitedMask, memo);
      }
    }

    currentPath.Remove(current);
    memo[key] = totalPaths;
    return totalPaths;
  }

  /// <summary>
  /// Highly optimized algorithm for counting paths from start to nodes containing endPattern,
  /// that visit at least one node containing each required pattern.
  /// Uses dynamic programming with bitmasks for O(N * 2^k) complexity instead of exponential.
  /// </summary>
  /// <param name="graph">Graph as adjacency list</param>
  /// <param name="start">Start node for path traversal</param>
  /// <param name="endPattern">Pattern that end nodes must contain (e.g., "out")</param>
  /// <param name="requiredPatterns">Patterns that must be visited (e.g., ["dac", "fft"])</param>
  /// <returns>Number of valid paths that visit all required patterns</returns>
  public static long CountPathsOptimized(Dictionary<string, List<string>> graph, string start, string endPattern, params string[] requiredPatterns)
  {
    // STEP 1: Collect all nodes in the graph (both sources and destinations)
    // This is necessary because destination nodes like "out" may not appear as keys
    var allNodes = new HashSet<string>(graph.Keys);
    foreach (var neighbors in graph.Values)
    {
      foreach (var neighbor in neighbors)
      {
        allNodes.Add(neighbor);
      }
    }
    
    // STEP 2: Pre-compute pattern masks for efficient lookup
    // Each node gets a bitmask indicating which required patterns it contains
    // For example: if node "dac123" contains "dac", bit 0 is set
    var nodePatternMask = new Dictionary<string, int>();
    var endNodes = new HashSet<string>();
    
    foreach (var node in allNodes)
    {
      int mask = 0;
      // Check each required pattern and set corresponding bit if found
      for (int i = 0; i < requiredPatterns.Length; i++)
      {
        if (node.Contains(requiredPatterns[i]))
        {
          mask |= (1 << i); // Set bit i if pattern i is found
        }
      }
      nodePatternMask[node] = mask;
      
      // Also track which nodes are valid end points
      if (node.Contains(endPattern))
      {
        endNodes.Add(node);
      }
    }
    
    // STEP 3: Initialize dynamic programming structures
    // memo[node][visitedMask] = number of paths from node with visitedMask patterns seen
    var memo = new Dictionary<(string, int), long>();
    var visited = new HashSet<string>(); // For cycle detection
    
    // Create mask representing "all required patterns visited"
    // For 2 patterns: allRequiredMask = 11 (binary) = 3 (decimal)
    int allRequiredMask = (1 << requiredPatterns.Length) - 1;
    
    // STEP 4: Start DFS with empty pattern mask
    return DfsLinear(graph, start, endNodes, nodePatternMask, allRequiredMask, visited, 0, memo);
  }
  
  /// <summary>
  /// Core DFS algorithm with memoization for counting valid paths.
  /// Uses bitmasks to efficiently track which required patterns have been visited.
  /// </summary>
  /// <param name="graph">Adjacency list representation of the graph</param>
  /// <param name="current">Current node being processed</param>
  /// <param name="endNodes">Set of valid end nodes (containing endPattern)</param>
  /// <param name="nodePatternMask">Pre-computed bitmask for each node indicating which patterns it contains</param>
  /// <param name="allRequiredMask">Bitmask representing all required patterns (target state)</param>
  /// <param name="visited">Set of nodes in current path (for cycle detection)</param>
  /// <param name="currentMask">Bitmask of patterns visited so far in current path</param>
  /// <param name="memo">Memoization cache: (node, visitedMask) -> path count</param>
  /// <returns>Number of valid paths from current node with current pattern state</returns>
  private static long DfsLinear(Dictionary<string, List<string>> graph, string current, 
    HashSet<string> endNodes, Dictionary<string, int> nodePatternMask, int allRequiredMask,
    HashSet<string> visited, int currentMask, Dictionary<(string, int), long> memo)
  {
    // CYCLE DETECTION: If we've already visited this node in the current path, it's a cycle
    if (visited.Contains(current))
      return 0;
    
    // UPDATE PATTERN MASK: Add any new patterns found at current node
    // Use bitwise OR to combine current mask with this node's pattern mask
    int newMask = currentMask | nodePatternMask[current];
    
    // MEMOIZATION CHECK: Have we already computed paths from this (node, pattern state)?
    var key = (current, newMask);
    if (memo.TryGetValue(key, out long cached))
      return cached;
    
    // BASE CASE: If we've reached an end node, check if all patterns were visited
    if (endNodes.Contains(current))
    {
      // Return 1 if newMask equals allRequiredMask (all patterns visited), 0 otherwise
      return (newMask == allRequiredMask) ? 1 : 0;
    }
    
    // RECURSIVE EXPLORATION: Mark current node as visited and explore neighbors
    visited.Add(current);
    long total = 0;
    
    // Explore all neighbors, passing the updated pattern mask
    if (graph.TryGetValue(current, out var neighbors))
    {
      foreach (var neighbor in neighbors)
      {
        total += DfsLinear(graph, neighbor, endNodes, nodePatternMask, allRequiredMask, visited, newMask, memo);
      }
    }
    
    // BACKTRACK: Remove current node from visited set for other paths
    visited.Remove(current);
    
    // MEMOIZE RESULT: Cache the result for this (node, pattern state) combination
    memo[key] = total;
    return total;
  }

  private static long DfsOptimized(Dictionary<string, List<string>> graph, string current, HashSet<string> endNodes,
    Dictionary<string, HashSet<string>> patternNodes, string[] requiredPatterns, HashSet<string> visited, int mask,
    Dictionary<(string, int), long> dp)
  {
    if (visited.Contains(current))
      return 0;

    // Update mask for current node
    int newMask = mask;
    for (int i = 0; i < requiredPatterns.Length; i++)
    {
      if (patternNodes[requiredPatterns[i]].Contains(current))
      {
        newMask |= (1 << i);
      }
    }

    // Use the NEW mask for memoization key
    var key = (current, newMask);
    if (dp.TryGetValue(key, out long cached))
      return cached;

    if (endNodes.Contains(current))
    {
      int allRequired = (1 << requiredPatterns.Length) - 1;
      return (newMask == allRequired) ? 1 : 0;
    }

    visited.Add(current);
    long total = 0;

    if (graph.TryGetValue(current, out var neighbors))
    {
      foreach (var neighbor in neighbors)
      {
        total += DfsOptimized(graph, neighbor, endNodes, patternNodes, requiredPatterns, visited, newMask, dp);
      }
    }

    visited.Remove(current);
    dp[key] = total;
    return total;
  }

  private static int CountPathsWithRequiredDfs<T>(Dictionary<T, List<T>> graph, T current, T end, HashSet<T> requiredNodes, 
    HashSet<T> currentPath, HashSet<T> visitedRequired, Dictionary<(T, string, string), int> memo)
    where T : notnull
  {
    // Check for cycles
    if (currentPath.Contains(current))
      return 0;

    // Create memoization keys
    var pathKey = string.Join(",", currentPath.OrderBy(x => x));
    var visitedKey = string.Join(",", visitedRequired.OrderBy(x => x));
    var key = (current, pathKey, visitedKey);
    
    if (memo.TryGetValue(key, out int cachedResult))
      return cachedResult;

    // Track if this node is required
    var newVisitedRequired = new HashSet<T>(visitedRequired);
    if (requiredNodes.Contains(current))
      newVisitedRequired.Add(current);

    if (current.Equals(end))
    {
      // Only count if all required nodes were visited
      return requiredNodes.IsSubsetOf(newVisitedRequired) ? 1 : 0;
    }

    currentPath.Add(current);
    int totalPaths = 0;

    if (graph.TryGetValue(current, out var neighbors))
    {
      foreach (var neighbor in neighbors)
      {
        totalPaths += CountPathsWithRequiredDfs(graph, neighbor, end, requiredNodes, currentPath, newVisitedRequired, memo);
      }
    }

    currentPath.Remove(current);
    memo[key] = totalPaths;
    return totalPaths;
  }

  private static bool checkBeenVisited<T>(List<T> path, List<T>? visit) where T : notnull
  {
    var pathOn = new HashSet<T>(path);
    return visit!.All(item => pathOn.Contains(item));
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
  /// Performs grid-based DFS with customizable validation and result collection
  /// </summary>
  /// <typeparam name="T">Type to collect as results</typeparam>
  /// <param name="grid">2D grid to traverse</param>
  /// <param name="startRow">Starting row position</param>
  /// <param name="startCol">Starting column position</param>
  /// <param name="directions">Array of direction vectors for movement</param>
  /// <param name="isValidMove">Function to validate if a move is allowed</param>
  /// <param name="shouldCollect">Function to determine if current position should be collected</param>
  /// <param name="shouldContinue">Function to determine if DFS should continue from current position</param>
  /// <returns>Set of collected results</returns>
  public static HashSet<T> GridDfs<T>(
    char[,] grid,
    int startRow,
    int startCol,
    int[][] directions,
    Func<char[,], int, int, int, int, bool> isValidMove,
    Func<char[,], int, int, T?> shouldCollect,
    Func<char[,], int, int, bool> shouldContinue) where T : notnull
  {
    var visited = new HashSet<(int, int)>();
    var results = new HashSet<T>();
    
    GridDfsRecursive(grid, startRow, startCol, directions, isValidMove, shouldCollect, shouldContinue, visited, results);
    return results;
  }

  /// <summary>
  /// Counts distinct paths in a grid using DFS with backtracking
  /// </summary>
  /// <param name="grid">2D grid to traverse</param>
  /// <param name="startRow">Starting row position</param>
  /// <param name="startCol">Starting column position</param>
  /// <param name="directions">Array of direction vectors for movement</param>
  /// <param name="isValidMove">Function to validate if a move is allowed</param>
  /// <param name="isEndCondition">Function to check if we've reached an end state</param>
  /// <returns>Number of distinct paths to end conditions</returns>
  public static long CountGridPaths(
    char[,] grid,
    int startRow,
    int startCol,
    int[][] directions,
    Func<char[,], int, int, int, int, bool> isValidMove,
    Func<char[,], int, int, bool> isEndCondition)
  {
    var visitedPaths = new HashSet<(int, int)>();
    return CountGridPathsRecursive(grid, startRow, startCol, directions, isValidMove, isEndCondition, visitedPaths);
  }

  private static void GridDfsRecursive<T>(
    char[,] grid,
    int row,
    int col,
    int[][] directions,
    Func<char[,], int, int, int, int, bool> isValidMove,
    Func<char[,], int, int, T?> shouldCollect,
    Func<char[,], int, int, bool> shouldContinue,
    HashSet<(int, int)> visited,
    HashSet<T> results) where T : notnull
  {
    if (visited.Contains((row, col)))
      return;

    visited.Add((row, col));

    // Check if we should collect this position
    var collectResult = shouldCollect(grid, row, col);
    if (collectResult != null)
    {
      results.Add(collectResult);
    }

    // Check if we should continue exploring from this position
    if (!shouldContinue(grid, row, col))
      return;

    // Explore neighbors
    foreach (var direction in directions)
    {
      int newRow = row + direction[0];
      int newCol = col + direction[1];

      if (isValidMove(grid, newRow, newCol, row, col))
      {
        GridDfsRecursive(grid, newRow, newCol, directions, isValidMove, shouldCollect, shouldContinue, visited, results);
      }
    }
  }

  private static long CountGridPathsRecursive(
    char[,] grid,
    int row,
    int col,
    int[][] directions,
    Func<char[,], int, int, int, int, bool> isValidMove,
    Func<char[,], int, int, bool> isEndCondition,
    HashSet<(int, int)> visitedPaths)
  {
    if (isEndCondition(grid, row, col))
      return 1;

    long distinctPaths = 0;

    foreach (var direction in directions)
    {
      int newRow = row + direction[0];
      int newCol = col + direction[1];

      if (isValidMove(grid, newRow, newCol, row, col) && !visitedPaths.Contains((newRow, newCol)))
      {
        visitedPaths.Add((newRow, newCol));
        distinctPaths += CountGridPathsRecursive(grid, newRow, newCol, directions, isValidMove, isEndCondition, visitedPaths);
        visitedPaths.Remove((newRow, newCol));
      }
    }

    return distinctPaths;
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

  private static bool HasCycleDFS<T>(Dictionary<T, List<T>> graph, T node, HashSet<T> visited, HashSet<T> recursionStack)
    where T : notnull
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
