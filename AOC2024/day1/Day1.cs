using Utility;

namespace AOC2024;

public class Day1
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    long resultPart1 = 0, resultPart2 = 0;

    var list1 = new List<long>();
    var list2 = new List<long>();

    foreach (string line in data)
    {
      var numbers = Parsing.ExtractLongs(line);
      list1.Add(numbers[0]);
      list2.Add(numbers[1]);
    }

    list1.Sort();
    list2.Sort();

    var list2Frequencies = Parsing.CountFrequencies(list2);

    for (int i = 0; i < list1.Count; i++)
    {
      resultPart1 += Math.Abs(list1[i] - list2[i]);
      resultPart2 += list1[i] * list2Frequencies.GetValueOrDefault(list1[i], 0);
    }

    return (resultPart1.ToString(), resultPart2.ToString());
  }
}