using Utility;

namespace AOC2024;

public class Day19
{
  private static long _part2Counter;
  private static List<string> _pattern = [];
  private static List<string> _towels = [];
  private static Dictionary<string, bool> _repeats = new(); //day 11 learning! :-)
  private static Dictionary<string, long> _repeatsCount = new(); //day 11 learning! :-)

  public (string, string) Process(string input)
  {

    if (input.Contains("Example"))
    {
      _part2Counter = 0;
      _repeats = new Dictionary<string, bool>();
      _repeatsCount = new Dictionary<string, long>();
      _pattern = new List<string>();
      _towels = new List<string>();
    }

    // Load and parse input data
    string[] data = SetupInputFile.OpenFile(input).ToArray();

    // Initialize grid based on the input
    InitializeGrid(data);
    long countPatterns = ProcessTowels();
    return (countPatterns.ToString(), _part2Counter.ToString());

  }
  private static long ProcessTowels()
  {
    long countPossible = 0;
    foreach (string currentPattern in _pattern)
    {
      if (IsPossible(currentPattern)) countPossible++;
      _part2Counter += CountPossible(currentPattern);
    }

    return countPossible;

  }
  private static bool IsPossible(string design)
  {
    if (string.IsNullOrEmpty(design))
    {
      return true;
    }

    if (_repeats.TryGetValue(design, out bool possible))
    {
      return possible;
    }

    foreach (string t in _towels)
    {
      if (!design.StartsWith(t))
        continue;

      string remainDesign = design[t.Length..];
      if (!IsPossible(remainDesign))
        continue;

      _repeats[remainDesign] = true;
      return true;
    }

    _repeats[design] = false;
    return false;
  }
  private static long CountPossible(string design)
  {
    if (string.IsNullOrEmpty(design))
    {
      return 1;
    }

    if (_repeatsCount.TryGetValue(design, out long possible)) return possible;

    long ways = (from t in _towels
                 where design.StartsWith(t)
                 select design[t.Length..]
                 into remainDesign
                 select CountPossible(remainDesign)).Sum();

    _repeatsCount[design] = ways;
    return ways;

  }
  private static void InitializeGrid(string[] data)
  {

    bool isFirstLine = true;
    // Initially, the grid is empty (no obstacles)
    foreach (string t in data)
    {
      if (isFirstLine)
      {
        _towels = t.Split(',', StringSplitOptions.TrimEntries).ToList();
        isFirstLine = false;
        continue;
      }

      if (t.Trim() == "") continue;

      _pattern.Add(t.Trim());
    }

  }
}