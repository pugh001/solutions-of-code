using Utility;

namespace AOC2024;

public class Day25
{
  private readonly List<List<int>> keys = new();
  private readonly List<List<int>> locks = new();

  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    ParseData(data);
    return (CountFit(), "done");
  }

  private void ParseData(IEnumerable<string> data)
  {
    int loadLine = 0;
    var pins = InitializePins();
    bool isKey = false;

    foreach (string? line in data)
    {
      if (string.IsNullOrWhiteSpace(line))
      {
        pins = InitializePins();
        loadLine = 0;
        isKey = false;
        continue;
      }

      if (IsKeySeparator(line, loadLine))
      {
        loadLine = 1;
        isKey = true;
        continue;
      }

      if (IsLockSeparator(line, loadLine, isKey))
      {
        locks.Add(pins);
        continue;
      }

      UpdatePins(line, pins);
      loadLine++;

      if (loadLine == 7 && isKey)
      {
        keys.Add(pins);
      }
    }
  }

  private List<int> InitializePins()
  {
    return Enumerable.Repeat(0, 5).ToList();
  }

  private bool IsKeySeparator(string line, int loadLine)
  {
    return line == "#####" && loadLine == 0;
  }

  private bool IsLockSeparator(string line, int loadLine, bool isKey)
  {
    return line == "#####" && loadLine == 6 && !isKey;
  }

  private void UpdatePins(string line, List<int> pins)
  {
    for (int i = 0; i < 5; i++)
    {
      if (line[i] == '#')
      {
        pins[i]++;
      }
    }
  }

  private string CountFit()
  {
    int fit = 0;
    foreach (var lck in locks)
    {
      foreach (var key in keys)
      {
        if (DoesKeyFit(lck, key))
        {
          fit++;
        }
      }
    }

    return fit.ToString();
  }

  private bool DoesKeyFit(List<int> lck, List<int> key)
  {
    for (int i = 0; i < key.Count; i++)
    {
      if (lck[i] + key[i] > 5)
      {
        return false;
      }
    }

    return true;
  }
}