using Utility;

namespace AOC2024;

public class Day10
{
  public (string, string) Process(string input)
  {
    // Load and parse input data
    string[] data = SetupInputFile.OpenFile(input).ToArray();

    int rowSize = data.Length;
    int colSize = data[0].Length;
    char[,] grid = new char[rowSize, colSize];

    for (int r = 0; r < rowSize; r++)
    {
      for (int c = 0; c < colSize; c++)
      {
        grid[r, c] = data[r][c];
      }
    }

    long resultPart1 = ProcessGridPart1(grid, rowSize, colSize);
    long resultPart2 = ProcessGridPart2(grid, rowSize, colSize);
    return (resultPart1.ToString(), resultPart2.ToString());


  }
  private static long ProcessGridPart1(char[,] grid, int rowSize, int colSize)
  {
    var trailheads = new List<(int, int)>();
    long totalScore = 0;

    // Directions for right, down, left, up
    int[][] directions =
    [
      [0, 1], // Right
      [1, 0], // Down
      [0, -1], // Left
      [-1, 0] // Up
    ];

    // Find all trailheads (cells with value '0')
    for (int r = 0; r < rowSize; r++)
    {
      for (int c = 0; c < colSize; c++)
      {
        if (grid[r, c] == '0')
          trailheads.Add((r, c));
      }
    }

    // Explore each trailhead using utility Grid DFS
    foreach (var trailhead in trailheads)
    {
      var reachableNines = new HashSet<(int, int)>();
      var visited = new HashSet<(int, int)>();
      
      // Use custom method that collects 9s
      CollectReachableNines(grid, trailhead.Item1, trailhead.Item2, directions, visited, reachableNines);
      totalScore += reachableNines.Count;
    }

    return totalScore;
  }

  private static long ProcessGridPart2(char[,] grid, int rowSize, int colSize)
  {
    var trailheads = new List<(int, int)>();
    long totalRating = 0;

    // Directions for right, down, left, up
    int[][] directions =
    [
      [0, 1], // Right
      [1, 0], // Down
      [0, -1], // Left
      [-1, 0] // Up
    ];

    // Find all trailheads (cells with value '0')
    for (int r = 0; r < rowSize; r++)
    {
      for (int c = 0; c < colSize; c++)
      {
        if (grid[r, c] == '0')
          trailheads.Add((r, c));
      }
    }

    // Count distinct paths for each trailhead using utility Grid DFS
    foreach (var trailhead in trailheads)
    {
      totalRating += Graph.CountGridPaths(
        grid,
        trailhead.Item1,
        trailhead.Item2,
        directions,
        IsValidMove,
        (g, r, c) => g[r, c] == '9' // End condition: reached height 9
      );
    }

    return totalRating;
  }

  private static bool IsValidMove(char[,] grid, int newRow, int newCol, int currentRow, int currentCol)
  {
    int rowSize = grid.GetLength(0);
    int colSize = grid.GetLength(1);

    // Check bounds
    if (newRow < 0 || newRow >= rowSize || newCol < 0 || newCol >= colSize)
      return false;

    // Height constraints (must increase by exactly 1)
    int currentHeight = grid[currentRow, currentCol] - '0';
    int newHeight = grid[newRow, newCol] - '0';

    return newHeight == currentHeight + 1;
  }

  private static void CollectReachableNines(char[,] grid, int row, int col, int[][] directions, 
    HashSet<(int, int)> visited, HashSet<(int, int)> reachableNines)
  {
    if (visited.Contains((row, col)))
      return;

    visited.Add((row, col));

    if (grid[row, col] == '9')
    {
      reachableNines.Add((row, col));
      return;
    }

    // Explore neighbors
    foreach (var direction in directions)
    {
      int newRow = row + direction[0];
      int newCol = col + direction[1];

      if (IsValidMove(grid, newRow, newCol, row, col))
      {
        CollectReachableNines(grid, newRow, newCol, directions, visited, reachableNines);
      }
    }
  }
}
