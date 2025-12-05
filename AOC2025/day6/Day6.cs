using Utility;

namespace AOC2025;

public class Day6
{
  private readonly long _sumPart1 = 0;
  private readonly long _sumPart2 = 0;
  public (string, string) Process(string input)
  {

    var data = SetupInputFile.OpenFile(input);

    foreach (string line in data)
    {
    }
    return (_sumPart1.ToString(), _sumPart2.ToString());
  }
}
