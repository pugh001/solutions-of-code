using Utility;
using Utility.Grid;

namespace AOC2025;
//Day  4:(1411, 8557)  Time:  00:00:00.2960369

public class Day4
{
  private int _pass;
  private long _sumPart1, _sumPart2;
  public (string, string) Process(string input)
  {
    var grid = new Grid(File.ReadAllText(input).Split('\n'));
    while (true)
    {
      var atPositions = grid.FindAll('@');
      var positionsToRemove = new List<Point2D<int>>();
      _pass++;

      GetPositionsToRemove(atPositions, grid, positionsToRemove);

      if (positionsToRemove.Count == 0) break;

      foreach (var position in positionsToRemove)
      {
        grid[position] = '.';
      }
    }

    return (_sumPart1.ToString(), _sumPart2.ToString());
  }
  private void GetPositionsToRemove(List<Point2D<int>> atPositions, Grid grid, List<Point2D<int>> positionsToRemove)
  {

    foreach (var position in atPositions)
    {
      if (NeighborAtCount(grid, position) >= 4)
        continue;

      if (_pass == 1) _sumPart1++;
      _sumPart2++;
      positionsToRemove.Add(position);
    }
  }
  private static int NeighborAtCount(Grid grid, Point2D<int> position)
  {
    return grid.GetAllNeighborsOfValue(position, '@').Count();
  }
}