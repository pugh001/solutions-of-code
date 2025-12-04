using Utility;
using Utility.Grid;

namespace AOC2025;
//Day  4:(1411, 8557)  Time:  00:00:00.2960369

public class Day4
{
  long sumPart1 = 0, sumPart2 = 0;
  int pass = 0;
  public (string, string) Process(string input)
  {
    var grid = new Grid(File.ReadAllText(input).Split('\n'));
    while (true)
    {
      var atPositions = grid.FindAll('@');
      var positionsToRemove = new List<Point2D<int>>();
      pass++;

      GetPositionsToRemove(atPositions, grid, positionsToRemove);

      if (positionsToRemove.Count == 0) break;

      foreach (var position in positionsToRemove)
      {
        grid[position] = '.';
      }
    }

    return (sumPart1.ToString(), sumPart2.ToString());
  }
  private void GetPositionsToRemove(List<Point2D<int>> atPositions, Grid grid, List<Point2D<int>> positionsToRemove)
  {

    foreach (var position in atPositions)
    {
      if (NeighborAtCount(grid, position) >= 4)
        continue;

      if (pass == 1) sumPart1++;
      sumPart2++;
      positionsToRemove.Add(position);
    }
  }
  private static int NeighborAtCount(Grid grid, Point2D<int> position)
  {
    return grid.GetAllNeighborsOfValue(position, '@').Count();
  }
}