using System.Text;

namespace Utility.Grid;

/// <summary>
///   A utility class for working with 2D character grids, commonly used in Advent of Code problems
/// </summary>
public class Grid
{
  private readonly char[,] _grid;

  public Grid(string[] lines)
  {
    if (lines == null || lines.Length == 0)
      throw new ArgumentException("Lines cannot be null or empty");

    Rows = lines.Length;
    Cols = lines[0].Length;
    _grid = new char[Rows, Cols];

    for (int r = 0; r < Rows; r++)
    {
      if (lines[r].Length != Cols)
        throw new ArgumentException("All lines must have the same length");

      for (int c = 0; c < Cols; c++)
      {
        _grid[r, c] = lines[r][c];
      }
    }
  }

  public Grid(char[,] grid)
  {
    Rows = grid.GetLength(0);
    Cols = grid.GetLength(1);
    _grid = new char[Rows, Cols];
    Array.Copy(grid, _grid, grid.Length);
  }

  public int Rows { get; }

  public int Cols { get; }

  /// <summary>
  ///   Gets or sets the character at the specified position
  /// </summary>
  public char this[int row, int col]
  {
    get => _grid[row, col];
    set => _grid[row, col] = value;
  }

  /// <summary>
  ///   Gets or sets the character at the specified point
  /// </summary>
  public char this[Point2D<int> point]
  {
    get => _grid[point.Y, point.X];
    set => _grid[point.Y, point.X] = value;
  }

  /// <summary>
  ///   Checks if the given coordinates are within the grid bounds
  /// </summary>
  public bool IsInBounds(int row, int col)
  {
    return row >= 0 && row < Rows && col >= 0 && col < Cols;
  }

  /// <summary>
  ///   Checks if the given point is within the grid bounds
  /// </summary>
  public bool IsInBounds(Point2D<int> point)
  {
    return IsInBounds(point.Y, point.X);
  }

  /// <summary>
  ///   Finds all positions of a specific character in the grid
  /// </summary>
  public List<Point2D<int>> FindAll(char character)
  {
    var positions = new List<Point2D<int>>();
    for (int r = 0; r < Rows; r++)
    {
      for (int c = 0; c < Cols; c++)
      {
        if (_grid[r, c] == character)
          positions.Add(new Point2D<int>(c, r));
      }
    }

    return positions;
  }

  /// <summary>
  ///   Finds the first position of a specific character in the grid
  /// </summary>
  public Point2D<int>? FindFirst(char character)
  {
    for (int r = 0; r < Rows; r++)
    {
      for (int c = 0; c < Cols; c++)
      {
        if (_grid[r, c] == character)
          return new Point2D<int>(c, r);
      }
    }

    return null;
  }

  /// <summary>
  ///   Gets all valid neighbors of a position (4-directional)
  /// </summary>
  public IEnumerable<Point2D<int>> GetNeighbors(Point2D<int> point)
  {
    return point.Neighbours().Where(IsInBounds);
  }

  /// <summary>
  ///   Gets all valid neighbors of a position (8-directional including diagonals)
  /// </summary>
  public IEnumerable<Point2D<int>> GetAllNeighbors(Point2D<int> point)
  {
    return point.Neighbours(true).Where(IsInBounds);
  }

  /// <summary>
  ///   Creates a copy of the grid
  /// </summary>
  public Grid Clone()
  {
    return new Grid(_grid);
  }

  /// <summary>
  ///   Converts the grid back to an array of strings
  /// </summary>
  public string[] ToStringArray()
  {
    string[] result = new string[Rows];
    for (int r = 0; r < Rows; r++)
    {
      var sb = new StringBuilder(Cols);
      for (int c = 0; c < Cols; c++)
      {
        sb.Append(_grid[r, c]);
      }

      result[r] = sb.ToString();
    }

    return result;
  }

  /// <summary>
  ///   Gets the underlying 2D array (read-only access recommended)
  /// </summary>
  public char[,] GetRawGrid()
  {
    char[,] copy = new char[Rows, Cols];
    Array.Copy(_grid, copy, _grid.Length);
    return copy;
  }

  /// <summary>
  ///   Counts occurrences of a specific character
  /// </summary>
  public int Count(char character)
  {
    int count = 0;
    for (int r = 0; r < Rows; r++)
    {
      for (int c = 0; c < Cols; c++)
      {
        if (_grid[r, c] == character)
          count++;
      }
    }

    return count;
  }

  /// <summary>
  ///   Prints the grid to console (useful for debugging)
  /// </summary>
  public void Print()
  {
    for (int r = 0; r < Rows; r++)
    {
      for (int c = 0; c < Cols; c++)
      {
        Console.Write(_grid[r, c]);
      }

      Console.WriteLine();
    }
  }

  public override string ToString()
  {
    var sb = new StringBuilder();
    for (int r = 0; r < Rows; r++)
    {
      for (int c = 0; c < Cols; c++)
      {
        sb.Append(_grid[r, c]);
      }

      if (r < Rows - 1) sb.AppendLine();
    }

    return sb.ToString();
  }
}