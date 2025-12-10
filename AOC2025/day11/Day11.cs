using System.Collections.Concurrent;
using Utility;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2025;

public class Day11
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
    int m = goal.Count;
    int n = buttonEffects.Count;

    // Build matrix A (m x n)
    long[,] A = new long[m, n];
    for (int j = 0; j < n; j++)
      foreach (var r in buttonEffects[j])
        A[r, j] = 1;

    // Copy RHS
    long[] b = goal.Select(x => (long)x).ToArray();

    // Row reduce to find pivot columns
    int row = 0;
    int col = 0;
    int[] pivotCol = Enumerable.Repeat(-1, m).ToArray();

    while (row < m && col < n)
    {
      // Find pivot
      int pivot = -1;
      for (int i = row; i < m; i++)
        if (A[i, col] != 0)
        {
          pivot = i;
          break;
        }

      if (pivot == -1)
      {
        col++;
        continue;
      }

      // Swap rows
      if (pivot != row)
      {
        for (int j = 0; j < n; j++)
          (A[row, j], A[pivot, j]) = (A[pivot, j], A[row, j]);
        (b[row], b[pivot]) = (b[pivot], b[row]);
      }

      pivotCol[row] = col;

      // Eliminate other rows (keeping integers)
      for (int i = 0; i < m; i++)
      {
        if (i == row) continue;
        if (A[i, col] == 0)
          continue;

        long pivotVal = A[row, col];
        long elimVal = A[i, col];

        for (int j = 0; j < n; j++)
          A[i, j] = A[i, j] * pivotVal - A[row, j] * elimVal;
        b[i] = b[i] * pivotVal - b[row] * elimVal;

        // Reduce by GCD to keep numbers manageable
        long gcd = Math.Abs(b[i]);
        for (int j = 0; j < n; j++)
          if (A[i, j] != 0)
            gcd = MathUtilities.FindGcd(gcd, Math.Abs(A[i, j]));

        if (gcd <= 1)
          continue;

        {
          for (int j = 0; j < n; j++)
            A[i, j] /= gcd;
          b[i] /= gcd;
        }
      }

      row++;
      col++;
    }

    // Check for inconsistency
    for (int i = 0; i < m; i++)
    {
      bool allZero = true;
      for (int j = 0; j < n; j++)
        if (A[i, j] != 0)
        {
          allZero = false;
          break;
        }

      if (allZero && b[i] != 0)
        return 0;
    }

    // Identify free and pivot variables
    var freeVars = new List<int>();
    var pivotVars = new int[n];
    Array.Fill(pivotVars, -1);

    for (int i = 0; i < m; i++)
      if (pivotCol[i] != -1)
        pivotVars[pivotCol[i]] = i;

    for (int j = 0; j < n; j++)
      if (pivotVars[j] == -1)
        freeVars.Add(j);

    Console.WriteLine($"Free variables: {freeVars.Count}, Pivot variables: {pivotVars.Count(x => x != -1)}");

    // Calculate a reasonable upper bound for each free variable
    // The max value any variable can take is limited by the max goal value divided by 
    // the minimum number of positions that variable affects
    int maxFv = goal.Max();
    foreach (var fv in freeVars)
    {
        if (buttonEffects[fv].Count > 0)
        {
            int localMax = goal.Max() / buttonEffects[fv].Count + 10;
            maxFv = Math.Max(maxFv, localMax);
        }
    }
    if (maxFv > 200) maxFv = 200;
    
    Console.WriteLine($"Search bound: maxFv = {maxFv}");

    long best = long.MaxValue;
    long[] xSol = new long[n];
    long iterations = 0;
    const long MAX_ITERATIONS = 50_000_000;

    Search(0, freeVars, m, pivotCol, A, b, n, xSol, buttonEffects, goal, ref best, maxFv, ref iterations, MAX_ITERATIONS);

    Console.WriteLine($"Final result: {(best == long.MaxValue ? 0 : best)}, iterations: {iterations}");
    return best == long.MaxValue ?
      0 :
      (int)best;

  }
  private static void Search(int fi,
    List<int>? freeVars,
    int m,
    int[] pivotCol,
    long[,] A,
    long[] b,
    int n,
    long[] xSol,
    List<List<int>> buttonEffects,
    List<int> goal,
    ref long best,
    int maxFv,
    ref long iterations,
    long maxIterations)
  {
    iterations++;
    if (iterations > maxIterations)
    {
        Console.WriteLine($"Max iterations reached: {iterations}");
        return;
    }
    
    if (fi == freeVars.Count)
    {
      // Solve for pivot vars based on free vars
      for (int i = 0; i < m; i++)
      {
        if (pivotCol[i] == -1) continue;

        int c = pivotCol[i];
        long pivotCoeff = A[i, c];
        if (pivotCoeff == 0) continue;

        long rhs = b[i];

        for (int j = 0; j < n; j++)
        {
          if (j != c)
            rhs -= A[i, j] * xSol[j];
        }

        if (rhs % pivotCoeff != 0) return;

        long val = rhs / pivotCoeff;
        if (val < 0) return;

        xSol[c] = val;
      }

      // Verify the solution
      for (int i = 0; i < m; i++)
      {
        long sum = 0;
        for (int j = 0; j < n; j++)
          sum += buttonEffects[j].Contains(i) ?
            xSol[j] :
            0;
        if (sum != goal[i]) return;
      }

      long total = xSol.Sum();
      if (total >= best)
        return;

      best = total;
      Console.WriteLine($"New best: {best}, x = [{string.Join(",", xSol)}]");
      return;
    }

    int var = freeVars[fi];
    for (int v = 0; v <= maxFv; v++)
    {
      xSol[var] = v;
      Search(fi + 1, freeVars, m, pivotCol, A, b, n, xSol, buttonEffects, goal, ref best, maxFv, ref iterations, maxIterations);
      if (iterations > maxIterations) return;
    }
  }

}
