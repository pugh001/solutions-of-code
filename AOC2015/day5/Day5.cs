using Utility;

namespace AOC2015;

public class Day5
{

  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input).ToList();

    return (NiceStrings(data), ExtraNice(data));

  }
  private static string ExtraNice(List<string> data)
  {
    int counter = data.Where(Letters.ContainsNoneOverlappingPair).Count(LetterRepeatWithLetterInbetween);

    return $"{counter}";
  }
  private static bool LetterRepeatWithLetterInbetween(string input)
  {
    if (string.IsNullOrEmpty(input) || input.Length < 3)
      return false;

    for (int i = 0; i < input.Length - 2; i++)
    {
      if (input[i] == input[i + 2]) return true;
    }

    return false;

  }

  private static string NiceStrings(List<string> data)
  {
    int counter = 0;
    foreach (string letters in data)
    {
      if (ContainsDisallowedPairs(letters)) continue;
      if (Letters.CountVowels(letters) < 3) continue;
      if (!Letters.IsRepeatedLetter(letters)) continue;

      counter++;

    }

    return $"{counter}";

  }
  private static bool ContainsDisallowedPairs(string letters)
  {
    if (letters.Contains("ab")) return true;
    if (letters.Contains("cd")) return true;
    if (letters.Contains("pq")) return true;
    if (letters.Contains("xy")) return true;

    return false;
  }
}