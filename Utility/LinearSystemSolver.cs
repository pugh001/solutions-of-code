namespace Utility;

public class LinearSystemSolver
{
  private const long MaxIterations = 50_000_000;
  private readonly List<List<int>> _buttonEffects;
  private readonly long[,] _coefficientMatrix;
  private readonly int _equationCount;
  private readonly List<int> _freeVariables;
  private readonly List<int> _goal;
  private readonly int _maxFreeVarValue;
  private readonly int[] _pivotColumn;
  private readonly long[] _rightHandSide;
  private readonly long[] _solution;
  private readonly int _variableCount;
  private long _bestSolution;
  private long _iterations;

  public LinearSystemSolver(List<int> goal, List<List<int>> buttonEffects)
  {
    _goal = goal;
    _buttonEffects = buttonEffects;
    _equationCount = goal.Count;
    _variableCount = buttonEffects.Count;

    _coefficientMatrix = BuildCoefficientMatrix(buttonEffects);
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

  private long[,] BuildCoefficientMatrix(List<List<int>> buttonEffects)
  {
    long[,] matrix = new long[_equationCount, _variableCount];

    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      foreach (int equationIndex in buttonEffects[varIndex])
        matrix[equationIndex, varIndex] = 1;

    return matrix;
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
    var rowOps = new MatrixRowOperations(_coefficientMatrix, _rightHandSide, _variableCount);
    int currentRow = 0, currentCol = 0;

    while (currentRow < _equationCount && currentCol < _variableCount)
    {
      int pivotRow = FindPivotRow(currentRow, currentCol);

      if (pivotRow == -1)
      {
        currentCol++;
        continue;
      }

      if (pivotRow != currentRow)
        rowOps.SwapRows(currentRow, pivotRow);

      _pivotColumn[currentRow] = currentCol;
      EliminateColumn(rowOps, currentRow, currentCol);

      currentRow++;
      currentCol++;
    }
  }

  private int FindPivotRow(int startRow, int column)
  {
    for (int rowIndex = startRow; rowIndex < _equationCount; rowIndex++)
      if (_coefficientMatrix[rowIndex, column] != 0)
        return rowIndex;

    return -1;
  }

  private void EliminateColumn(MatrixRowOperations rowOps, int pivotRow, int pivotColumn)
  {
    for (int rowIndex = 0; rowIndex < _equationCount; rowIndex++)
    {
      if (ShouldSkipRow(rowIndex, pivotRow, pivotColumn))
        continue;

      rowOps.EliminateRow(rowIndex, pivotRow, pivotColumn);
      rowOps.ReduceRowByGcd(rowIndex);
    }
  }

  private bool ShouldSkipRow(int rowIndex, int pivotRow, int pivotColumn)
  {
    return rowIndex == pivotRow || _coefficientMatrix[rowIndex, pivotColumn] == 0;
  }

  private void IdentifyFreeVariables()
  {
    int[] pivotVariables = new int[_variableCount];
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
    foreach (int freeVar in _freeVariables)
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
    if (ShouldStopSearch())
      return;

    if (IsSearchComplete(freeVarIndex))
    {
      TryUpdateBestSolution();
      return;
    }

    SearchFreeVariableValues(freeVarIndex);
  }

  private bool ShouldStopSearch()
  {
    _iterations++;
    return _iterations > MaxIterations;
  }

  private bool IsSearchComplete(int freeVarIndex)
  {
    return freeVarIndex == _freeVariables.Count;
  }

  private void TryUpdateBestSolution()
  {
    if (TrySolvePivotVariables() && VerifySolution())
    {
      long totalButtonPresses = _solution.Sum();
      if (totalButtonPresses < _bestSolution)
      {
        _bestSolution = totalButtonPresses;
      }
    }
  }

  private void SearchFreeVariableValues(int freeVarIndex)
  {
    int variableIndex = _freeVariables[freeVarIndex];

    for (int value = 0; value <= _maxFreeVarValue; value++)
    {
      _solution[variableIndex] = value;
      Search(freeVarIndex + 1);

      if (_iterations > MaxIterations)
        return;
    }
  }

  private bool TrySolvePivotVariables()
  {
    for (int rowIndex = 0; rowIndex < _equationCount; rowIndex++)
    {
      if (!TrySolvePivotVariable(rowIndex, out long value))
        return false;

      if (value < 0)
        return false;
    }

    return true;
  }

  private bool TrySolvePivotVariable(int rowIndex, out long value)
  {
    value = 0;

    if (_pivotColumn[rowIndex] == -1)
      return true;

    int pivotVarIndex = _pivotColumn[rowIndex];
    long pivotCoefficient = _coefficientMatrix[rowIndex, pivotVarIndex];

    if (pivotCoefficient == 0)
      return true;

    long rightHandSide = CalculateAdjustedRightHandSide(rowIndex, pivotVarIndex);

    if (rightHandSide % pivotCoefficient != 0)
      return false;

    value = rightHandSide / pivotCoefficient;
    _solution[pivotVarIndex] = value;
    return true;
  }

  private long CalculateAdjustedRightHandSide(int rowIndex, int pivotVarIndex)
  {
    long rightHandSide = _rightHandSide[rowIndex];

    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      if (varIndex != pivotVarIndex)
        rightHandSide -= _coefficientMatrix[rowIndex, varIndex] * _solution[varIndex];

    return rightHandSide;
  }

  private bool VerifySolution()
  {
    for (int equationIndex = 0; equationIndex < _equationCount; equationIndex++)
    {
      long sum = CalculateEquationSum(equationIndex);

      if (sum != _goal[equationIndex])
        return false;
    }

    return true;
  }

  private long CalculateEquationSum(int equationIndex)
  {
    long sum = 0;

    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      if (_buttonEffects[varIndex].Contains(equationIndex))
        sum += _solution[varIndex];

    return sum;
  }
}