namespace Utility;

internal class MatrixRowOperations
{
  private readonly int _columnCount;
  private readonly long[,] _matrix;
  private readonly long[] _rightHandSide;
  /// <summary>
  ///   Handles row operations for Gaussian elimination
  /// </summary>
  internal MatrixRowOperations(long[,] matrix, long[] rightHandSide, int columnCount)
  {
    _matrix = matrix;
    _rightHandSide = rightHandSide;
    _columnCount = columnCount;
  }

  public void SwapRows(int row1, int row2)
  {
    for (int colIndex = 0; colIndex < _columnCount; colIndex++)
      (_matrix[row1, colIndex], _matrix[row2, colIndex]) = (_matrix[row2, colIndex], _matrix[row1, colIndex]);

    (_rightHandSide[row1], _rightHandSide[row2]) = (_rightHandSide[row2], _rightHandSide[row1]);
  }

  public void EliminateRow(int targetRow, int pivotRow, int pivotColumn)
  {
    long pivotValue = _matrix[pivotRow, pivotColumn];
    long eliminationValue = _matrix[targetRow, pivotColumn];

    for (int colIndex = 0; colIndex < _columnCount; colIndex++)
      _matrix[targetRow, colIndex] = _matrix[targetRow, colIndex] * pivotValue - _matrix[pivotRow, colIndex] * eliminationValue;

    _rightHandSide[targetRow] = _rightHandSide[targetRow] * pivotValue - _rightHandSide[pivotRow] * eliminationValue;
  }

  public void ReduceRowByGcd(int rowIndex)
  {
    long gcd = CalculateRowGcd(rowIndex);

    if (gcd <= 1)
      return;

    for (int colIndex = 0; colIndex < _columnCount; colIndex++)
      _matrix[rowIndex, colIndex] /= gcd;
    _rightHandSide[rowIndex] /= gcd;
  }

  private long CalculateRowGcd(int rowIndex)
  {
    long gcd = Math.Abs(_rightHandSide[rowIndex]);

    for (int colIndex = 0; colIndex < _columnCount; colIndex++)
      if (_matrix[rowIndex, colIndex] != 0)
        gcd = MathUtilities.FindGcd(gcd, Math.Abs(_matrix[rowIndex, colIndex]));

    return gcd;
  }
}