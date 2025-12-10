using System.Collections.Concurrent;
using Utility;
using System.Linq;
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

  private class LinearSystemSolver
  {
    private readonly List<int> _goal;
    private readonly List<List<int>> _buttonEffects;
    private readonly int _m; // number of equations
    private readonly int _n; // number of variables
    private readonly long[,] _A; // coefficient matrix
    private readonly long[] _b; // right-hand side
    private readonly int[] _pivotCol;
    private readonly List<int> _freeVars;
    private readonly int _maxFv;
    private long[] _xSol;
    private long _best;
    private long _iterations;
    private const long MaxIterations = 50_000_000;

    public LinearSystemSolver(List<int> goal, List<List<int>> buttonEffects)
    {
      _goal = goal;
      _buttonEffects = buttonEffects;
      _m = goal.Count;
      _n = buttonEffects.Count;

      // Build coefficient matrix
      _A = new long[_m, _n];
      for (int j = 0; j < _n; j++)
        foreach (var r in buttonEffects[j])
          _A[r, j] = 1;

      _b = goal.Select(x => (long)x).ToArray();
      _pivotCol = Enumerable.Repeat(-1, _m).ToArray();
      _freeVars = new List<int>();
      _xSol = new long[_n];
      _best = long.MaxValue;
      _iterations = 0;

      PerformGaussianElimination();
      IdentifyFreeVariables();
      _maxFv = CalculateSearchBound();
    }

    public int Solve()
    {

      Search(0);

      return _best == long.MaxValue ?
        0 :
        (int)_best;
    }

    private void PerformGaussianElimination()
    {
      int row = 0, col = 0;

      while (row < _m && col < _n)
      {
        // Find pivot
        int pivot = -1;
        for (int i = row; i < _m; i++)
          if (_A[i, col] != 0)
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
          for (int j = 0; j < _n; j++)
            (_A[row, j], _A[pivot, j]) = (_A[pivot, j], _A[row, j]);
          (_b[row], _b[pivot]) = (_b[pivot], _b[row]);
        }

        _pivotCol[row] = col;

        // Eliminate other rows
        for (int i = 0; i < _m; i++)
        {
          if (i == row || _A[i, col] == 0)
            continue;

          long pivotVal = _A[row, col];
          long elimVal = _A[i, col];

          for (int j = 0; j < _n; j++)
            _A[i, j] = _A[i, j] * pivotVal - _A[row, j] * elimVal;
          _b[i] = _b[i] * pivotVal - _b[row] * elimVal;

          // Reduce by GCD
          long gcd = Math.Abs(_b[i]);
          for (int j = 0; j < _n; j++)
            if (_A[i, j] != 0)
              gcd = MathUtilities.FindGcd(gcd, Math.Abs(_A[i, j]));

          if (gcd > 1)
          {
            for (int j = 0; j < _n; j++)
              _A[i, j] /= gcd;
            _b[i] /= gcd;
          }
        }

        row++;
        col++;
      }
    }

    private void IdentifyFreeVariables()
    {
      var pivotVars = new int[_n];
      Array.Fill(pivotVars, -1);

      for (int i = 0; i < _m; i++)
        if (_pivotCol[i] != -1)
          pivotVars[_pivotCol[i]] = i;

      for (int j = 0; j < _n; j++)
        if (pivotVars[j] == -1)
          _freeVars.Add(j);
    }

    private int CalculateSearchBound()
    {
      int maxFv = _goal.Max();
      foreach (var fv in _freeVars)
      {
        if (_buttonEffects[fv].Count > 0)
        {
          int localMax = _goal.Max() / _buttonEffects[fv].Count + 10;
          maxFv = Math.Max(maxFv, localMax);
        }
      }

      return Math.Min(maxFv, 200);
    }

    private void Search(int freeVarIndex)
    {
      _iterations++;
      if (_iterations > MaxIterations)
      {
        return;
      }

      if (freeVarIndex == _freeVars.Count)
      {
        if (TrySolvePivotVariables() && VerifySolution())
        {
          long total = _xSol.Sum();
          if (total < _best)
          {
            _best = total;
          }
        }

        return;
      }

      int var = _freeVars[freeVarIndex];
      for (int v = 0; v <= _maxFv; v++)
      {
        _xSol[var] = v;
        Search(freeVarIndex + 1);
        if (_iterations > MaxIterations) return;
      }
    }

    private bool TrySolvePivotVariables()
    {
      for (int i = 0; i < _m; i++)
      {
        if (_pivotCol[i] == -1) continue;

        int c = _pivotCol[i];
        long pivotCoeff = _A[i, c];
        if (pivotCoeff == 0) continue;

        long rhs = _b[i];
        for (int j = 0; j < _n; j++)
          if (j != c)
            rhs -= _A[i, j] * _xSol[j];

        if (rhs % pivotCoeff != 0)
          return false;

        long val = rhs / pivotCoeff;
        if (val < 0)
          return false;

        _xSol[c] = val;
      }

      return true;
    }

    private bool VerifySolution()
    {
      for (int i = 0; i < _m; i++)
      {
        long sum = 0;
        for (int j = 0; j < _n; j++)
          if (_buttonEffects[j].Contains(i))
            sum += _xSol[j];

        if (sum != _goal[i])
          return false;
      }

      return true;
    }
  }
}