using Utility;
using Utility.Grid;

namespace AOC2025;
//Day  4:(1411, 8557)  Time:  00:00:00.2960369

public class Day4
{
  public (string, string) Process(string input)
  {
    var grid = new Grid(File.ReadAllText(input).Split('\n'));
    
    var removalCondition = GridSimulation.CreateNeighborCountCondition(grid, '@', 4, includeDiagonals: true);
    var (firstPassRemovals, totalRemovals) = GridSimulation.RunSimulation(grid, removalCondition);

    return (firstPassRemovals.ToString(), totalRemovals.ToString());
  }
}
