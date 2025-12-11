using Utility;

namespace AOC2025;

public class Day2
{
  //24043483400,
  public (string, string) Process(string input)
  {
    string part1 = "", part2 = "";
    long sumPart1 = 0;
    long sumPart2 = 0;
    var data = SetupInputFile.OpenFile(input);
    string firstLine = data.First();
    var parts = firstLine.Split(',', StringSplitOptions.TrimEntries).ToList();

    foreach (string part in parts)
    {
      var numbers = part.ExtractPosLongs().ToList();
      long start = numbers[0];
      long end = numbers[1];

      // Part 1: Use the optimized generation method
      sumPart1 += MathUtilities.GeneratePalindromicHalfNumbers(start, end).Sum();

      // Part 2: Find numbers with repeated sequences
      for (long i = start; i <= end; i++)
      {
        if (Sequences.HasRepeatedSequence(i.ToString()))
        {
          sumPart2 += i;
        }
      }
    }

    return (sumPart1.ToString(), sumPart2.ToString());
  }
}