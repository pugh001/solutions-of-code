using System.Text.RegularExpressions;
using Utility;

namespace AOC2024;

public class Day5
{


  public (string, string) Process(string input)
  {
    long resultPart1 = 0;
    long resultPart2 = 0;
    // Load and parse input data
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    int position = 0;
    var orderingRules = new List<(int X, int Y)>();
    foreach (string line in data)
    {
      position++;
      if (line == "") break;

      if (!Regex.IsMatch(line, @"^\d{1,2}|\d{1,2}$"))
        continue;

      string[] parts = line.Split('|');

      orderingRules.Add((X: int.Parse(parts[0]), Y: int.Parse(parts[1])));
    }

    List<List<int>> updates = new();
    for (int i = position; i < data.Length; i++)
    {
      string line = data[i];
      //split as page list into array.
      var pages = line.Split(',').Select(int.Parse).ToList();
      updates.Add(pages);

    }


    // Part 1: Check correctly ordered updates and sum their middle values
    int correctMiddleSum = 0;
    List<List<int>> incorrectUpdates = new();

    foreach (var update in updates)
    {
      if (IsCorrectlyOrdered(update, orderingRules))
      {
        correctMiddleSum += GetMiddleValue(update);
      }
      else
      {
        incorrectUpdates.Add(update);
      }
    }

    resultPart1 = correctMiddleSum;

    // Part 2: Fix incorrect updates and sum their middle values
    int fixedMiddleSum = incorrectUpdates.Select(update => FixOrdering(update, orderingRules))
      .Select(correctedUpdate => GetMiddleValue(correctedUpdate)).Sum();

    resultPart2 = fixedMiddleSum;
    return (resultPart1.ToString(), resultPart2.ToString());
  }

  private static bool IsCorrectlyOrdered(List<int> update, List<(int X, int Y)> rules)
  {
    var position = update.Select((value, index) => (value, index)).ToDictionary(x => x.value, x => x.index);

    foreach ((int X, int Y) in rules)
    {
      if (!position.ContainsKey(X) || !position.ContainsKey(Y))
        continue;

      if (position[X] < position[Y])
        continue;

      return false;
    }

    return true;
  }

  private static List<int> FixOrdering(List<int> update, List<(int X, int Y)> rules)
  {
    var graph = Graph.BuildGraph(update, rules);
    return Graph.TopologicalSort(graph, update);
  }


  private static int GetMiddleValue(List<int> update)
  {
    return update[update.Count / 2];
  }
}