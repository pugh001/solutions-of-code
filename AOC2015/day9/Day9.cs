using Utility;

namespace AOC2015;

public class Day9
{
  private long _sumPart1 = 0;
  private long _sumPart2 = 0;
  public (string, string) Process(string input)
  {

    var data = SetupInputFile.OpenFile(input);

    foreach (string line in data)
    {
    }
    return (_sumPart1.ToString(), _sumPart2.ToString());
  }
}
