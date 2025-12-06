using System.Text.RegularExpressions;
using Utility.Parsing;

namespace Utility.Grid;

/// <summary>
/// Utility for processing mathematical grids with operations and numbers
/// </summary>
public static class MathGrid
{
  /// <summary>
  /// Parses input into normalized columns (removes multiple spaces)
  /// </summary>
  public static List<List<string>> ParseNormalizedGrid(IEnumerable<string> lines)
  {
    var grid = new List<List<string>>();
    foreach (var line in lines)
    {
      var cleanedLine = Regex.Replace(line.Trim(), @" +", " ");
      var columns = cleanedLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
      grid.Add(columns);
    }
    return grid;
  }

  /// <summary>
  /// Calculates result for each column using operation from last row
  /// </summary>
  public static long CalculateColumnResults(List<List<string>> grid)
  {
    if (grid.Count == 0) return 0;
    
    var operations = grid[^1];
    long totalSum = 0;
    
    for (int col = 0; col < operations.Count; col++)
    {
      char operation = operations[col].Length > 0 ? operations[col][0] : '+';
      long result = operation == '*' ? 1 : 0;
      
      for (int row = 0; row < grid.Count - 1; row++)
      {
        if (col < grid[row].Count && long.TryParse(grid[row][col], out long number))
        {
          result = operation switch
          {
            '+' => result + number,
            '*' => result * number,
            _ => result
          };
        }
      }
      
      totalSum += result;
    }
    
    return totalSum;
  }

  /// <summary>
  /// Finds problem boundaries in a spaced grid (columns with only spaces/operations)
  /// </summary>
  public static List<int> FindProblemBoundaries(List<string> grid)
  {
    if (grid.Count == 0) return new List<int>();
    
    int maxWidth = grid.Max(line => line.Length);
    var boundaries = new List<int>();
    
    for (int col = 0; col < maxWidth; col++)
    {
      bool isAllSpacesOrOp = true;
      foreach (var line in grid)
      {
        if (col < line.Length)
        {
          char ch = line[col];
          if (!char.IsWhiteSpace(ch) && ch != '+' && ch != '*')
          {
            isAllSpacesOrOp = false;
            break;
          }
        }
      }
      if (isAllSpacesOrOp)
      {
        boundaries.Add(col);
      }
    }
    
    // Add start and end boundaries
    boundaries.Insert(0, -1);
    boundaries.Add(maxWidth);
    boundaries.Sort();
    
    return boundaries;
  }

  /// <summary>
  /// Reads numbers vertically from columns to form multi-digit numbers
  /// </summary>
  public static List<long> ReadVerticalNumbers(List<string> grid, int startCol, int endCol)
  {
    var numbers = new List<long>();
    
    for (int col = startCol; col < endCol; col++)
    {
      var digitChars = new List<char>();
      for (int row = 0; row < grid.Count - 1; row++) // Exclude last row (operations)
      {
        if (col < grid[row].Length && char.IsDigit(grid[row][col]))
        {
          digitChars.Add(grid[row][col]);
        }
      }
      
      if (digitChars.Count > 0)
      {
        string numberStr = new string(digitChars.ToArray());
        if (long.TryParse(numberStr, out long number))
        {
          numbers.Add(number);
        }
      }
    }
    
    return numbers;
  }

  /// <summary>
  /// Applies operation to a list of numbers
  /// </summary>
  public static long ApplyOperation(List<long> numbers, char operation)
  {
    if (numbers.Count == 0) return 0;
    
    long result = operation == '*' ? 1 : 0;
    foreach (var number in numbers)
    {
      result = operation switch
      {
        '+' => result + number,
        '*' => result * number,
        _ => result
      };
    }
    
    return result;
  }

  /// <summary>
  /// Finds operation character in a range of columns from the last row
  /// </summary>
  public static char FindOperation(List<string> grid, int startCol, int endCol)
  {
    if (grid.Count == 0) return '+';
    
    var lastRow = grid[^1];
    for (int col = startCol; col < endCol; col++)
    {
      if (col < lastRow.Length)
      {
        char ch = lastRow[col];
        if (ch == '+' || ch == '*')
        {
          return ch;
        }
      }
    }
    
    return '+'; // Default operation
  }

  /// <summary>
  /// Processes vertical mathematical problems in a spaced grid
  /// </summary>
  public static long CalculateVerticalProblems(List<string> grid)
  {
    var boundaries = FindProblemBoundaries(grid);
    long totalSum = 0;
    
    for (int i = 0; i < boundaries.Count - 1; i++)
    {
      int start = boundaries[i] + 1;
      int end = boundaries[i + 1];
      
      if (start >= end) continue;
      
      char operation = FindOperation(grid, start, end);
      var numbers = ReadVerticalNumbers(grid, start, end);
      long result = ApplyOperation(numbers, operation);
      
      totalSum += result;
    }
    
    return totalSum;
  }
}
