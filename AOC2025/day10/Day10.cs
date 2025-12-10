using Utility;
using System.Text.RegularExpressions;

namespace AOC2025;

public class Day10
{
  public (string, string) Process(string input)
  {
    int part1 = 0;
    long part2 = 0;
    var data = SetupInputFile.OpenFile(input);

    foreach (var line in data)
    {
      // Parse targetState as binary integer
      int target = Target(line, out int bitLength);

      // Parse buttons as binary integers (for part 1)
      var buttons = Buttons(line, bitLength);

      // Parse goal counters (for part 2)
      var goalCounters = GoalCounters(line);

      // Parse button effects for part 2 (which positions each button affects)
      var buttonEffects = ButtonEffects(line);

      // Part 1: BFS to find minimum presses to reach target
      part1 = Part1BFS(target, buttons, part1);

      // Part 2: Solve linear system to minimize button presses
      part2 += SolvePart2(goalCounters, buttonEffects);
    }

    return (part1.ToString(), part2.ToString());
  }

  private static int Part1BFS(int target, List<int> buttons, int part1)
  {

    var queue = new Queue<(int state, int steps)>();
    var visited = new HashSet<int>();
    queue.Enqueue((0, 0));
    visited.Add(0);
    int minSteps = -1;
    while (queue.Count > 0)
    {
      var (state, steps) = queue.Dequeue();
      if (state == target)
      {
        minSteps = steps;
        break;
      }

      foreach (var btn in buttons)
      {
        int next = state ^ btn;
        if (visited.Contains(next))
          continue;

        visited.Add(next);
        queue.Enqueue((next, steps + 1));
      }
    }

    if (minSteps >= 0)
      part1 += minSteps;
    return part1;
  }

  private static List<int> Buttons(string line, int bitLength)
  {

    var buttonMatches = Regex.Matches(line, "\\((.*?)\\)");
    var buttons = new List<int>();
    foreach (Match m in buttonMatches)
    {
      int btn = 0;
      var nums = m.Groups[1].Value.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s.Trim()));
      foreach (var idx in nums)
      {
        btn |= (1 << (bitLength - idx - 1));
      }

      buttons.Add(btn);
    }

    return buttons;
  }
  private static int Target(string line, out int bitLength)
  {

    var targetStateMatch = Regex.Match(line, "\\[(.*?)\\]");
    string targetStr = targetStateMatch.Success ?
      targetStateMatch.Groups[1].Value :
      "";
    int target = 0;
    bitLength = targetStr.Length;
    for (int i = 0; i < bitLength; i++)
    {
      if (targetStr[i] == '#')
        target |= (1 << (bitLength - i - 1));
    }

    return target;
  }

  private static List<int> GoalCounters(string line)
  {
    var goalMatch = Regex.Match(line, "\\{(.*?)\\}");
    if (!goalMatch.Success) return new List<int>();

    return goalMatch.Groups[1].Value.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s.Trim()))
      .ToList();
  }

  private static List<List<int>> ButtonEffects(string line)
  {
    var buttonMatches = Regex.Matches(line, "\\((.*?)\\)");
    var buttonEffects = new List<List<int>>();

    foreach (Match m in buttonMatches)
    {
      var positions = m.Groups[1].Value.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s.Trim()))
        .ToList();
      buttonEffects.Add(positions);
    }

    return buttonEffects;
  }
  private static int SolvePart2(List<int> goal, List<List<int>> buttonEffects)
  {
    var solver = new LinearSystemSolver(goal, buttonEffects);
    return solver.Solve();
  }
  
}