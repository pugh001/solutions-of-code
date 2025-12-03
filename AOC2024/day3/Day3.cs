using System.Text.RegularExpressions;
using Utility;

namespace AOC2024;

public class Day3
{
  public (string, string) Process(string input)
  {
    // Load and parse input data
    var data = SetupInputFile.OpenFile(input);

    // Define regex patterns
    const string mulPattern = @"mul\(\d{1,3},\d{1,3}\)";
    const string dontDoPattern = @"don't\(\).*?do\(\)";
    const string dontPattern = @"don't\(\).*";

    // Compile regex patterns for better performance if reused
    var mulRegex = new Regex(mulPattern, RegexOptions.Compiled);
    var dontDoRegex = new Regex(dontDoPattern, RegexOptions.Compiled | RegexOptions.Singleline);
    var dontRegex = new Regex(dontPattern, RegexOptions.Compiled | RegexOptions.Singleline);

    // Extract data from the first line
    string inputLine = data.First();

    // Process for part 1
    long resultPart1 = CalculateResult(inputLine, mulRegex);

    // Process for part 2
    string part2Filtered = dontDoRegex.Replace(inputLine, string.Empty);
    string lastDontFiltered = dontRegex.Replace(part2Filtered, string.Empty);
    long resultPart2 = CalculateResult(lastDontFiltered, mulRegex);

    return (resultPart1.ToString(), resultPart2.ToString());
  }

  private static long CalculateResult(string input, Regex regex)
  {
    long result = 0;

    // Find and process matches
    foreach (Match match in regex.Matches(input))
    {
      long[] values = match.Value.Replace("mul(", "").Replace(")", "").Split(',').Select(long.Parse).ToArray();
      result += values[0] * values[1];
    }

    return result;
  }
}