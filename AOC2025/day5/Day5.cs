using Utility;
using Range = Utility.Range;


namespace AOC2025;

public class Day5
{
  private long _sumPart1 = 0;
  private long _sumPart2 = 0;
  public (string, string) Process(string input)
  {

    var data = SetupInputFile.OpenFile(input);
var ranges = new MultiRange();
    bool isRange = true;
    foreach (string line in data)
    {
      if (line.Trim() == "")
      {
        isRange = false;
        continue;
      }
      if (isRange)
      {
        var rangeValues = line.Split("-").ToList();
        var range = new Range(long.Parse(rangeValues[0]), long.Parse(rangeValues[1]));
        ranges.AddRange(range);
        continue;
      }

      var inRangeCheckValue = long.Parse(line);
      if (ranges.Contains(inRangeCheckValue))
      {
        _sumPart1++;
      }
    }

    _sumPart2 = ranges.SumOfNoneOverlappingRanges();
    return (_sumPart1.ToString(), _sumPart2.ToString());
  }
}
