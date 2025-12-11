using Utility;

namespace AOC2024;

public class Day10
{
  public (string, string) Process(string input)
  {
    // Load and parse input data using Grid utility
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    var grid = new Grid(data);

    long resultPart1 = ProcessGridPart1(grid);
    long resultPart2 = ProcessGridPart2(grid);
    return (resultPart1.ToString(), resultPart2.ToString());


  }
  private static long ProcessGridPart1(Grid grid)
  {
    // Find all trailheads (cells with value '0') using Grid utility
    var trailheads = grid.FindAll('0');
    long totalScore = 0;

    // Explore each trailhead
    foreach (var trailhead in trailheads)
    {
      var reachableNines = new HashSet<Point2D<int>>();
      var visited = new HashSet<Point2D<int>>();

      // Use custom method that collects 9s
      CollectReachableNines(grid, trailhead, visited, reachableNines);
      totalScore += reachableNines.Count;
    }

    return totalScore;
  }

  private static long ProcessGridPart2(Grid grid)
  {
    // Find all trailheads (cells with value '0') using Grid utility
    var trailheads = grid.FindAll('0');
    long totalRating = 0;

    // Count distinct paths for each trailhead using PathFinding utility
    foreach (var trailhead in trailheads)
    {
      totalRating += PathFinding.CountDistinctPaths(grid, trailhead, (current, next) => IsValidMove(grid, next, current),
        position => grid[position] == '9');
    }

    return totalRating;
  }

  private static bool IsValidMove(Grid grid, Point2D<int> newPos, Point2D<int> currentPos)
  {
    // Check bounds using Grid utility
    if (!grid.IsInBounds(newPos))
      return false;

    // Height constraints (must increase by exactly 1)
    int currentHeight = grid[currentPos] - '0';
    int newHeight = grid[newPos] - '0';

    return newHeight == currentHeight + 1;
  }

  private static void CollectReachableNines(Grid grid,
    Point2D<int> position,
    HashSet<Point2D<int>> visited,
    HashSet<Point2D<int>> reachableNines)
  {
    if (visited.Contains(position))
      return;

    visited.Add(position);

    if (grid[position] == '9')
    {
      reachableNines.Add(position);
      return;
    }

    // Explore neighbors using Grid utility
    foreach (var neighbor in grid.GetNeighbors(position))
    {
      if (IsValidMove(grid, neighbor, position))
      {
        CollectReachableNines(grid, neighbor, visited, reachableNines);
      }
    }
  }
}