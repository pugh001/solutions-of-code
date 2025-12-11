
// Utility namespace for helper classes
namespace Utility;


/// <summary>
/// Solves a system of linear equations where each variable (button) can affect multiple equations (lights/goals).
/// Designed for problems like "pressing buttons to reach a goal state" (e.g., puzzles).
/// </summary>
public class LinearSystemSolver
{
  // Maximum number of search iterations to avoid infinite loops
  private const long MaxIterations = 50_000_000;
  // List of which equations (e.g., lights) each variable (button) affects
  private readonly List<List<int>> _buttonEffects;
  // Coefficient matrix for the system of equations
  private readonly long[,] _coefficientMatrix;
  // Number of equations (e.g., number of lights)
  private readonly int _equationCount;
  // Indices of variables that are free (not determined by Gaussian elimination)
  private readonly List<int> _freeVariables;
  // The target state for each equation (e.g., desired light state)
  private readonly List<int> _goal;
  // Maximum value to try for each free variable during brute-force search
  private readonly int _maxFreeVarValue;
  // For each row, the index of the pivot variable (or -1 if none)
  private readonly int[] _pivotColumn;
  // Right-hand side of the equations
  private readonly long[] _rightHandSide;
  // Current solution vector (number of presses for each button)
  private readonly long[] _solution;
  // Number of variables (e.g., number of buttons)
  private readonly int _variableCount;
  // Best (minimal) solution found so far
  private long _bestSolution;
  // Number of search iterations performed
  private long _iterations;

  /// <summary>
  /// Initializes the solver with the goal state and the effects of each button.
  /// </summary>
  /// <param name="goal">Target values for each equation (e.g., desired light states)</param>
  /// <param name="buttonEffects">For each button, a list of equations it affects</param>
  public LinearSystemSolver(List<int> goal, List<List<int>> buttonEffects)
  {
    _goal = goal;
    _buttonEffects = buttonEffects;
    _equationCount = goal.Count;
    _variableCount = buttonEffects.Count;

    // Build the coefficient matrix for the system
    _coefficientMatrix = BuildCoefficientMatrix(buttonEffects);
    _rightHandSide = goal.Select(x => (long)x).ToArray();
    _pivotColumn = Enumerable.Repeat(-1, _equationCount).ToArray();
    _freeVariables = new List<int>();
    _solution = new long[_variableCount];
    _bestSolution = long.MaxValue;
    _iterations = 0;

    // Reduce the system to row-echelon form
    PerformGaussianElimination();
    // Identify which variables are free (not determined by elimination)
    IdentifyFreeVariables();
    // Calculate the maximum value to try for each free variable
    _maxFreeVarValue = CalculateSearchBound();
  }

  /// <summary>
  /// Builds the coefficient matrix from the button effects list.
  /// </summary>
  private long[,] BuildCoefficientMatrix(List<List<int>> buttonEffects)
  {
    long[,] matrix = new long[_equationCount, _variableCount];

    // For each variable, set matrix entry to 1 if it affects the equation
    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      foreach (int equationIndex in buttonEffects[varIndex])
        matrix[equationIndex, varIndex] = 1;

    return matrix;
  }

  /// <summary>
  /// Attempts to solve the system for the minimal total button presses.
  /// </summary>
  /// <returns>Minimal number of presses, or 0 if no solution found</returns>
  public int Solve()
  {
    Search(0);

    return _bestSolution == long.MaxValue ?
      0 :
      (int)_bestSolution;
  }

  /// <summary>
  /// Performs Gaussian elimination to reduce the system to row-echelon form.
  /// </summary>
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

      // Swap to bring pivot row to the top
      if (pivotRow != currentRow)
        rowOps.SwapRows(currentRow, pivotRow);

      _pivotColumn[currentRow] = currentCol;
      // Eliminate below and above the pivot
      EliminateColumn(rowOps, currentRow, currentCol);

      currentRow++;
      currentCol++;
    }
  }

  /// <summary>
  /// Finds the first row with a nonzero entry in the given column, starting from startRow.
  /// </summary>
  private int FindPivotRow(int startRow, int column)
  {
    for (int rowIndex = startRow; rowIndex < _equationCount; rowIndex++)
      if (_coefficientMatrix[rowIndex, column] != 0)
        return rowIndex;

    return -1;
  }

  /// <summary>
  /// Eliminates the pivotColumn in all rows except the pivotRow.
  /// </summary>
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

  /// <summary>
  /// Determines if a row should be skipped during elimination (if it's the pivot row or already zero in the pivot column).
  /// </summary>
  private bool ShouldSkipRow(int rowIndex, int pivotRow, int pivotColumn)
  {
    return rowIndex == pivotRow || _coefficientMatrix[rowIndex, pivotColumn] == 0;
  }

  /// <summary>
  /// Identifies which variables are free (not determined by a pivot in any row).
  /// </summary>
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

  /// <summary>
  /// Calculates the upper bound for the value of each free variable to limit brute-force search.
  /// </summary>
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

    // Clamp to 200 to avoid excessive search
    return Math.Min(maxValue, 200);
  }

  /// <summary>
  /// Recursively searches all possible values for free variables to find the minimal solution.
  /// </summary>
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

  /// <summary>
  /// Returns true if the search should stop (iteration limit reached).
  /// </summary>
  private bool ShouldStopSearch()
  {
    _iterations++;
    return _iterations > MaxIterations;
  }

  /// <summary>
  /// Returns true if all free variables have been assigned.
  /// </summary>
  private bool IsSearchComplete(int freeVarIndex)
  {
    return freeVarIndex == _freeVariables.Count;
  }

  /// <summary>
  /// If the current assignment is valid, update the best solution found so far.
  /// </summary>
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

  /// <summary>
  /// Tries all possible values for the current free variable and recurses.
  /// </summary>
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

  /// <summary>
  /// Attempts to solve for all pivot variables given the current free variable assignments.
  /// </summary>
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

  /// <summary>
  /// Solves for the pivot variable in the given row, if possible.
  /// </summary>
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

    // If not divisible, no integer solution
    if (rightHandSide % pivotCoefficient != 0)
      return false;

    value = rightHandSide / pivotCoefficient;
    _solution[pivotVarIndex] = value;
    return true;
  }

  /// <summary>
  /// Calculates the right-hand side for a row, subtracting the effect of all variables except the pivot.
  /// </summary>
  private long CalculateAdjustedRightHandSide(int rowIndex, int pivotVarIndex)
  {
    long rightHandSide = _rightHandSide[rowIndex];

    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      if (varIndex != pivotVarIndex)
        rightHandSide -= _coefficientMatrix[rowIndex, varIndex] * _solution[varIndex];

    return rightHandSide;
  }

  /// <summary>
  /// Checks if the current solution satisfies all equations.
  /// </summary>
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

  /// <summary>
  /// Calculates the sum for a given equation using the current solution.
  /// </summary>
  private long CalculateEquationSum(int equationIndex)
  {
    long sum = 0;

    for (int varIndex = 0; varIndex < _variableCount; varIndex++)
      if (_buttonEffects[varIndex].Contains(equationIndex))
        sum += _solution[varIndex];

    return sum;
  }
}
