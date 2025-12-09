using Utility;

namespace AOC2025;

public class Day6
{
  public (string, string) Process(string input)
  {
    var lines = SetupInputFile.OpenFile(input);
    var data = new List<string>();
    foreach (var line in lines)
    {
      data.Add(line);
    }
    
    var part1 = CalculatePart1(data);
    var part2 = CalculatePart2(data);

    return (part1.ToString(), part2.ToString());
  }

  private static long CalculatePart1(List<string> data)
  {
    var normalizedGrid = MathGrid.ParseNormalizedGrid(data);
    return MathGrid.CalculateColumnResults(normalizedGrid);
  }

  private static long CalculatePart2(List<string> data)
  {
    return MathGrid.CalculateVerticalProblems(data);
  }
}
