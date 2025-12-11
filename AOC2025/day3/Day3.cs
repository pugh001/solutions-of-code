using Utility;

namespace AOC2025;

public class Day3
{
  //24043483400,
  public (string, string) Process(string input)
  {
    long sumPart1 = 0;
    long sumPart2 = 0;
    var data = SetupInputFile.OpenFile(input);

    foreach (string line in data)
    {

      char bestFirst = GetBest(line, 1, line[0], out int firstIndex);
      char bestSecond = GetBest(line, firstIndex + 1, line[^1], out int _);

      sumPart1 += long.Parse(string.Concat(bestFirst.ToString(), bestSecond.ToString()));
      sumPart2 += Part2Longest12(line);
    }

    return (sumPart1.ToString(), sumPart2.ToString());
  }
  private static long Part2Longest12(string line)
  {

    switch (line.Length)
    {
      case >= 12:
      {
        string highest12Digit = Sequences.GetHighestSubsequence(line, 12);
        return long.Parse(highest12Digit);
      }
      case > 0:
        return long.Parse(line);
      default:
        return 0;
    }

  }

  private static char GetBest(string line, int start, char best, out int index)
  {
    index = start - 1;
    for (int i = start; i < line.Length - 1; i++)
    {
      if (line[i] > best)
      {
        best = line[i];
        index = i;
      }

      if (best == '9') break;
    }

    return best;
  }
}