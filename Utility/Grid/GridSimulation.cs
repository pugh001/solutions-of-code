namespace Utility.Grid;

/// <summary>
/// Utility for grid-based simulations with iterative processing
/// </summary>
public static class GridSimulation
{
  /// <summary>
  /// Runs a simulation until no more changes occur, tracking removals per pass
  /// </summary>
  public static (int firstPassRemovals, int totalRemovals) RunSimulation(
    Grid grid,
    Func<Point2D<int>, bool> shouldRemove,
    char targetChar = '@',
    char replacementChar = '.')
  {
    int pass = 0;
    int firstPassRemovals = 0;
    int totalRemovals = 0;
    
    while (true)
    {
      var positions = grid.FindAll(targetChar);
      var positionsToRemove = positions.Where(shouldRemove).ToList();
      
      pass++;
      
      if (positionsToRemove.Count == 0) 
        break;
      
      if (pass == 1) 
        firstPassRemovals = positionsToRemove.Count;
      
      totalRemovals += positionsToRemove.Count;
      
      // Remove positions
      foreach (var position in positionsToRemove)
      {
        grid[position] = replacementChar;
      }
    }
    
    return (firstPassRemovals, totalRemovals);
  }

  /// <summary>
  /// Runs a simulation with custom processing logic per pass
  /// </summary>
  public static TResult RunSimulation<TResult>(
    Grid grid,
    Func<Grid, int, (List<Point2D<int>> toRemove, bool shouldContinue, TResult result)> processPass,
    char replacementChar = '.')
  {
    int pass = 0;
    TResult result = default(TResult);
    
    while (true)
    {
      pass++;
      var (positionsToRemove, shouldContinue, passResult) = processPass(grid, pass);
      result = passResult;
      
      if (!shouldContinue || positionsToRemove.Count == 0)
        break;
      
      // Apply removals
      foreach (var position in positionsToRemove)
      {
        grid[position] = replacementChar;
      }
    }
    
    return result;
  }

  /// <summary>
  /// Counts neighbors of a specific character for a position
  /// </summary>
  public static int CountNeighborsOfType(Grid grid, Point2D<int> position, char targetChar, bool includeDiagonals = false)
  {
    var neighbors = includeDiagonals 
      ? grid.GetAllNeighborsOfValue(position, targetChar)
      : grid.GetNeighborsOfValue(position, targetChar);
    
    return neighbors.Count();
  }
  
  /// <summary>
  /// Creates a removal condition based on neighbor count
  /// </summary>
  public static Func<Point2D<int>, bool> CreateNeighborCountCondition(
    Grid grid, 
    char targetChar, 
    int minNeighbors, 
    bool includeDiagonals = false)
  {
    return position => CountNeighborsOfType(grid, position, targetChar, includeDiagonals) < minNeighbors;
  }
}
