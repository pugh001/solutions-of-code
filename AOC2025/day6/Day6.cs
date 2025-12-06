using System.Text.RegularExpressions;
using Utility;
using Utility.Grid;

namespace AOC2025;

public class Day6
{
  private long _sumPart1 = 0;
  private long _sumPart2 = 0;
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input).ToList();
    
    // Parse data into 2D structure, preserving exact spacing
    var grid = new List<string>();
    foreach (var line in data)
    {
      grid.Add(line);
    }
    
    if (grid.Count == 0) return (_sumPart1.ToString(), _sumPart2.ToString());
    
    // Part 1: Original logic with space normalization
    CalculatePart1(data);
    
    // Part 2: Column-based vertical number reading
    CalculatePart2(grid);

    return (_sumPart1.ToString(), _sumPart2.ToString());
  }

  private void CalculatePart1(List<string> data)
  {
    var grid = new List<List<string>>();
    foreach (var line in data)
    {
      var cleanedLine = Regex.Replace(line.Trim(), @" +", " ");
      var columns = cleanedLine.Split(' ').ToList();
      grid.Add(columns);
    }
    
    var operations = grid[^1];
    int numCols = operations.Count;
    
    for (int col = 0; col < numCols; col++)
    {
      char operation = operations[col].Length > 0 ? operations[col][0] : '+';
      long result = operation == '*' ? 1 : 0;
      
      for (int row = 0; row < grid.Count - 1; row++)
      {
        if (col < grid[row].Count && long.TryParse(grid[row][col], out long number))
        {
          switch (operation)
          {
            case '+':
              result += number;
              break;
            case '*':
              result *= number;
              break;
          }
        }
      }
      
      _sumPart1 += result;
    }
  }

  private void CalculatePart2(List<string> grid)
  {
    if (grid.Count == 0) return;
    
    // Find the maximum width to ensure we process all columns
    int maxWidth = grid.Max(line => line.Length);
    
    // Identify problem boundaries (columns that are all spaces or contain operations)
    var problemBoundaries = new List<int>();
    for (int col = 0; col < maxWidth; col++)
    {
      bool isAllSpacesOrOp = true;
      for (int row = 0; row < grid.Count; row++)
      {
        if (col < grid[row].Length)
        {
          char ch = grid[row][col];
          if (!char.IsWhiteSpace(ch) && ch != '+' && ch != '*')
          {
            isAllSpacesOrOp = false;
            break;
          }
        }
      }
      if (isAllSpacesOrOp)
      {
        problemBoundaries.Add(col);
      }
    }
    
    // Add boundaries at start and end
    if (!problemBoundaries.Contains(-1)) problemBoundaries.Insert(0, -1);
    if (!problemBoundaries.Contains(maxWidth)) problemBoundaries.Add(maxWidth);
    problemBoundaries.Sort();
    
    // Process each problem (between boundaries)
    for (int i = 0; i < problemBoundaries.Count - 1; i++)
    {
      int start = problemBoundaries[i] + 1;
      int end = problemBoundaries[i + 1];
      
      if (start >= end) continue;
      
      // Find operation for this problem (from last row)
      char operation = '+';
      for (int col = start; col < end; col++)
      {
        if (col < grid[^1].Length)
        {
          char ch = grid[^1][col];
          if (ch == '+' || ch == '*')
          {
            operation = ch;
            break;
          }
        }
      }
      
      // Extract numbers by reading vertically within this problem
      var numbers = new List<long>();
      for (int col = start; col < end; col++)
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
      
      // Apply operation to numbers
      if (numbers.Count > 0)
      {
        long result = operation == '*' ? 1 : 0;
        foreach (var number in numbers)
        {
          switch (operation)
          {
            case '+':
              result += number;
              break;
            case '*':
              result *= number;
              break;
          }
        }
        _sumPart2 += result;
      }
    }
  }
}
