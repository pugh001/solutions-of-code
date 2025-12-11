using Utility;

namespace AOC2015;

public class Day1 : IDay
{
  public (string, string) Process(string input)
  {
    string? data = SetupInputFile.OpenFile(input).First();

    int bracketsClose = 0;
    int bracketsOpen = 0;
    foreach (char c in data)
    {
      if (c == ')') bracketsClose++;
      if (c == '(') bracketsOpen++;
    }

    int resultPart1 = bracketsOpen - bracketsClose;

    int liftPosition = 0;
    int resultPart2 = 1;
    foreach (char move in data)
    {
      if (move == '(') liftPosition++;
      if (move == ')') liftPosition--;
      if (liftPosition < 0) break;

      resultPart2++;
    }

    return (resultPart1.ToString(), resultPart2.ToString());
  }
}