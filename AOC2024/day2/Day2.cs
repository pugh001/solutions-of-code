using Utility;

namespace AOC2024;

public class Day2
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    long resultPart1 = 0, resultPart2 = 0;


    foreach (string? line in data)
    {

      long[] x = line.Split(' ').Select(long.Parse).ToArray();

      if (IsSafe(x, -1))
      {
        resultPart1++;
        resultPart2++;
      }
      else
      {
        resultPart2 += AddSkippy(x);
      }
    }

    return (resultPart1.ToString(), resultPart2.ToString());
  }

  private static int AddSkippy(long[] x)
  {
    return x.Where((t, i) => IsSafe(x, i)).Any() ?
      1 :
      0;
  }

  private static bool IsSafe(long[] x, int skip)
  {
    bool desc = true, set = false;
    int startAt = skip == 0 ?
      1 :
      0;
    long last = x[startAt];

    for (int i = startAt + 1; i < x.Length; i++)
    {
      if (i == skip) continue;

      long current = x[i];
      if (last == current || Math.Abs(last - current) > 3) return false;

      if (last < current)
      {
        if (set && !desc) return false;

        desc = true;
      }
      else
      {
        if (set && desc) return false;

        desc = false;
      }

      set = true;

      last = current;
    }

    return true;
  }
}