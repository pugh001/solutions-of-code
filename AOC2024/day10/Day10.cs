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
    {
      new[] { 0, 1 }, // Right
      new[] { 1, 0 }, // Down
      new[] { 0, -1 }, // Left
      new[] { -1, 0 } // Up
    };

    // Find all trailheads (cells with value '0')
    for (int r = 0; r < rowSize; r++)
    {
      for (int c = 0; c < colSize; c++)
      {
        if (grid[r, c] == '0')
          trailheads.Add((r, c));
      }
    }

    // Explore each trailhead
    foreach (var trailhead in trailheads)
    {
      var visited = new HashSet<(int, int)>(); // Track visited cells
      var reachableNines = new HashSet<(int, int)>(); // Track reachable 9s
      Dfs(grid, trailhead.Item1, trailhead.Item2, visited, reachableNines, directions);
      totalScore += reachableNines.Count; // Add the score for this trailhead
    }

    return totalScore;
  }

  private static void Dfs(char[,] grid,
    int row,
    int col,
    HashSet<(int, int)> visited,
    HashSet<(int, int)> reachableNines,
    int[][] directions)
  {
    if (visited.Contains((row, col)))
      return; // Already visited

    visited.Add((row, col));

    int currentHeight = grid[row, col] - '0'; // Convert char to int

    // Check if this is a height of 9
    if (currentHeight == 9)
    {
      reachableNines.Add((row, col));
      return; // No further exploration needed
    }

    // Explore neighbors
    foreach (int[]? direction in directions)
    {
      int newRow = row + direction[0];
      int newCol = col + direction[1];

      if (IsValidMove(grid, newRow, newCol, row, col))
      {
        Dfs(grid, newRow, newCol, visited, reachableNines, directions);
      }
    }
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
  private static long ProcessGridPart2(char[,] grid, int rowSize, int colSize)
  {
    var trailheads = new List<(int, int)>();
    long totalRating = 0;

    // Directions for right, down, left, up
    int[][] directions =
    {
      new[] { 0, 1 }, // Right
      new[] { 1, 0 }, // Down
      new[] { 0, -1 }, // Left
      new[] { -1, 0 } // Up
    };

    // Find all trailheads (cells with value '0')
    for (int r = 0; r < rowSize; r++)
    {
      for (int c = 0; c < colSize; c++)
      {
        if (grid[r, c] == '0')
          trailheads.Add((r, c));
      }
    }

    // Explore each trailhead
    foreach (var trailhead in trailheads)
    {
      var visitedPaths = new HashSet<(int, int)>(); // Track visited cells for distinct paths
      totalRating += CountDistinctTrails(grid, trailhead.Item1, trailhead.Item2, directions, visitedPaths);
    }

    return totalRating;
  }

  private static long CountDistinctTrails(char[,] grid, int row, int col, int[][] directions, HashSet<(int, int)> visitedPaths)
  {
    int currentHeight = grid[row, col] - '0';

    // Base case: If the height is 9, we've found a complete trail
    if (currentHeight == 9)
      return 1;

    long distinctTrails = 0;

    foreach (int[]? direction in directions)
    {
      int newRow = row + direction[0];
      int newCol = col + direction[1];

      // Check if the move is valid and hasn't been visited on this trail
      if (IsValidMove(grid, newRow, newCol, row, col) && !visitedPaths.Contains((newRow, newCol)))
      {
        // Mark this cell as visited
        visitedPaths.Add((newRow, newCol));

        // Recursively count distinct trails from the next position
        distinctTrails += CountDistinctTrails(grid, newRow, newCol, directions, visitedPaths);

        // Backtrack: Unmark the cell as visited for other paths
        visitedPaths.Remove((newRow, newCol));
      }
    }

    return distinctTrails;
  }
}