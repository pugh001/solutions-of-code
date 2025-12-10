namespace Utility;

public class LinearSystemSolver
{
  private readonly List<int> _goal;
  private readonly List<List<int>> _buttonEffects;
  private readonly int _equationCount;
  private readonly int _variableCount;
  private readonly long[,] _coefficientMatrix;
  private readonly long[] _rightHandSide;
  private readonly int[] _pivotColumn;
  private readonly List<int> _freeVariables;
  private readonly int _maxFreeVarValue;
  private long[] _solution;
  private long _bestSolution;
  private long _iterations;
  private const long MaxIterations = 50_000_000;

  public LinearSystemSolver(List<int> goal, List<List<int>> buttonEffects)
  {
    _goal = goal;
    _buttonEffects = buttonEffects;
    _equationCount = goal.Count;
    _variableCount = buttonEffects.Count;

    // Build coefficient matrix
    _coefficientMatrix = new long[_equationCount, _variableCount];
    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      foreach (var equationIndex in buttonEffects[varIndex])
        _coefficientMatrix[equationIndex, varIndex] = 1;

    _rightHandSide = goal.Select(x => (long)x).ToArray();
    _pivotColumn = Enumerable.Repeat(-1, _equationCount).ToArray();
    _freeVariables = new List<int>();
    _solution = new long[_variableCount];
    _bestSolution = long.MaxValue;
    _iterations = 0;

    PerformGaussianElimination();
    IdentifyFreeVariables();
    _maxFreeVarValue = CalculateSearchBound();
  }

  public int Solve()
  {
    Search(0);

    return _bestSolution == long.MaxValue ?
      0 :
      (int)_bestSolution;
  }

  private void PerformGaussianElimination()
  {
    int currentRow = 0, currentCol = 0;

    while (currentRow < _equationCount && currentCol < _variableCount)
    {
      // Find pivot row
      int pivotRow = -1;
      for (int rowIndex = currentRow; rowIndex < _equationCount; rowIndex++)
        if (_coefficientMatrix[rowIndex, currentCol] != 0)
        {
          pivotRow = rowIndex;
          break;
        }

      if (pivotRow == -1)
      {
        currentCol++;
        continue;
      }

      // Swap rows if needed
      if (pivotRow != currentRow)
      {
        for (int colIndex = 0; colIndex < _variableCount; colIndex++)
          (_coefficientMatrix[currentRow, colIndex], _coefficientMatrix[pivotRow, colIndex]) = 
            (_coefficientMatrix[pivotRow, colIndex], _coefficientMatrix[currentRow, colIndex]);
        (_rightHandSide[currentRow], _rightHandSide[pivotRow]) = 
          (_rightHandSide[pivotRow], _rightHandSide[currentRow]);
      }

      _pivotColumn[currentRow] = currentCol;

      // Eliminate other rows
      for (int rowIndex = 0; rowIndex < _equationCount; rowIndex++)
      {
        if (rowIndex == currentRow || _coefficientMatrix[rowIndex, currentCol] == 0)
          continue;

        long pivotValue = _coefficientMatrix[currentRow, currentCol];
        long eliminationValue = _coefficientMatrix[rowIndex, currentCol];

        for (int colIndex = 0; colIndex < _variableCount; colIndex++)
          _coefficientMatrix[rowIndex, colIndex] = 
            _coefficientMatrix[rowIndex, colIndex] * pivotValue - 
            _coefficientMatrix[currentRow, colIndex] * eliminationValue;
        _rightHandSide[rowIndex] = 
          _rightHandSide[rowIndex] * pivotValue - 
          _rightHandSide[currentRow] * eliminationValue;

        // Reduce by GCD
        long gcd = Math.Abs(_rightHandSide[rowIndex]);
        for (int colIndex = 0; colIndex < _variableCount; colIndex++)
          if (_coefficientMatrix[rowIndex, colIndex] != 0)
            gcd = MathUtilities.FindGcd(gcd, Math.Abs(_coefficientMatrix[rowIndex, colIndex]));

        if (gcd > 1)
        {
          for (int colIndex = 0; colIndex < _variableCount; colIndex++)
            _coefficientMatrix[rowIndex, colIndex] /= gcd;
          _rightHandSide[rowIndex] /= gcd;
        }
      }

      currentRow++;
      currentCol++;
    }
  }

  private void IdentifyFreeVariables()
  {
    var pivotVariables = new int[_variableCount];
    Array.Fill(pivotVariables, -1);

    for (int rowIndex = 0; rowIndex < _equationCount; rowIndex++)
      if (_pivotColumn[rowIndex] != -1)
        pivotVariables[_pivotColumn[rowIndex]] = rowIndex;

    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      if (pivotVariables[varIndex] == -1)
        _freeVariables.Add(varIndex);
  }

  private int CalculateSearchBound()
  {
    int maxValue = _goal.Max();
    foreach (var freeVar in _freeVariables)
    {
      if (_buttonEffects[freeVar].Count > 0)
      {
        int localMax = _goal.Max() / _buttonEffects[freeVar].Count + 10;
        maxValue = Math.Max(maxValue, localMax);
      }
    }

    return Math.Min(maxValue, 200);
  }

  private void Search(int freeVarIndex)
  {
    _iterations++;
    if (_iterations > MaxIterations)
    {
      return;
    }

    if (freeVarIndex == _freeVariables.Count)
    {
      if (TrySolvePivotVariables() && VerifySolution())
      {
        long totalButtonPresses = _solution.Sum();
        if (totalButtonPresses < _bestSolution)
        {
          _bestSolution = totalButtonPresses;
        }
      }

      return;
    }

    int variableIndex = _freeVariables[freeVarIndex];
    for (int value = 0; value <= _maxFreeVarValue; value++)
    {
      _solution[variableIndex] = value;
      Search(freeVarIndex + 1);
      if (_iterations > MaxIterations) return;
    }
  }

  private bool TrySolvePivotVariables()
  {
    for (int rowIndex = 0; rowIndex < _equationCount; rowIndex++)
    {
      if (_pivotColumn[rowIndex] == -1) continue;

      int pivotVarIndex = _pivotColumn[rowIndex];
      long pivotCoefficient = _coefficientMatrix[rowIndex, pivotVarIndex];
      if (pivotCoefficient == 0) continue;

      long rightHandSide = _rightHandSide[rowIndex];
      for (int varIndex = 0; varIndex < _variableCount; varIndex++)
        if (varIndex != pivotVarIndex)
          rightHandSide -= _coefficientMatrix[rowIndex, varIndex] * _solution[varIndex];

      if (rightHandSide % pivotCoefficient != 0)
        return false;

      long value = rightHandSide / pivotCoefficient;
      if (value < 0)
        return false;

      _solution[pivotVarIndex] = value;
    }

    return true;
  }

  private bool VerifySolution()
  {
    for (int equationIndex = 0; equationIndex < _equationCount; equationIndex++)
    {
      long sum = 0;
      for (int varIndex = 0; varIndex < _variableCount; varIndex++)
        if (_buttonEffects[varIndex].Contains(equationIndex))
          sum += _solution[varIndex];

      if (sum != _goal[equationIndex])
        return false;
    }

    return true;
  }
}
