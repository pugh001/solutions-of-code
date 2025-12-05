using Utility;

namespace AOC2015;

public class Day11
{
  private readonly long _sumPart2 = 0;

  private string _sumPart1 = "";
  public (string, string) Process(string input)
  {

    string? data = SetupInputFile.OpenFile(input).First();
    _sumPart1 = data;
    bool isValid = false;
    while (!isValid)
    {
      _sumPart1 = Letters.AddLetterToString(_sumPart1);
      if (!Letters.DoesItContainInvalidLetters(_sumPart1, "iol"))
        continue;
      if (!Letters.DoesItContainStraight(_sumPart1))
        continue;

      if (Letters.DoesItContainNoneOverlappingDifferentPairs(_sumPart1))
      {
        isValid = true;
      }
    }

    return (_sumPart1, _sumPart2.ToString());
  }
}