using Utility;

namespace AOC2024;

public class Day12
{
  private static readonly Dictionary<(long, long), LineChecker> GridLine = new();

  private static readonly int[][] Directions = new[]
  {
    new[] { 0, 1 }, // Right
    new[] { 1, 0 }, // Down
    new[] { 0, -1 }, // Left
    new[] { -1, 0 } // Up
  };

  private static readonly string DirectionList = "RDLU";

  public (string, string) Process(string input)
  {
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    if (data.Length == 0)
      throw new ArgumentException("Input is empty. Please provide valid data.");

    char[,] grid = ParseGrid(data);
    long result1 = CalculateTotalFenceCost(grid, CalculateRegionOne);
    long result2 = CalculateTotalFenceCost(grid, CalculateRegionTwo);
    return (result1.ToString(), result2.ToString());
  }

  private static char[,] ParseGrid(string[] data)
  {
    int rows = data.Length, cols = data[0].Length;
    if (data.Any(row => row.Length != cols))
      throw new ArgumentException("Input grid rows are not uniform in length.");

    char[,] grid = new char[rows, cols];
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        grid[r, c] = data[r][c];
        GridLine[(r, c)] = new LineChecker { value = data[r][c], visited = "    " };

      }
    }

    return grid;
  }


  private static long CalculateTotalFenceCost(char[,] grid, Func<char[,], bool[,], int, int, (int, int)> calculateRegion)
  {
    int rows = grid.GetLength(0), cols = grid.GetLength(1);
    bool[,] visited = new bool[rows, cols];
    long totalCost = 0;

    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        if (!visited[r, c])
        {
          (int area, int measure) = calculateRegion(grid, visited, r, c);
          totalCost += area * measure;
        }
      }
    }

    return totalCost;
  }

  private static (int, int) CalculateRegionOne(char[,] grid, bool[,] visited, int startRow, int startCol)
  {
    return CalculateRegion(grid, visited, startRow, startCol, CalculatePerimeter);
  }

  private static (int, int) CalculateRegionTwo(char[,] grid, bool[,] visited, int startRow, int startCol)
  {
    return CalculateRegion(grid, visited, startRow, startCol, CalculateSides);
  }

  private static (int, int) CalculateRegion(char[,] grid,
    bool[,] visited,
    int startRow,
    int startCol,
    Func<char[,], List<(int, int)>, (int, int)> measureCalculator)
  {
    int rows = grid.GetLength(0), cols = grid.GetLength(1);
    char plantType = grid[startRow, startCol];
    int area = 0;

    var queue = new Queue<(int, int)>();
    var regionCells = new List<(int, int)>();

    queue.Enqueue((startRow, startCol));
    visited[startRow, startCol] = true;

    while (queue.Count > 0)
    {
      (int row, int col) = queue.Dequeue();
      area++;
      regionCells.Add((row, col));

      foreach (int[] direction in Directions)
      {
        int newRow = row + direction[0], newCol = col + direction[1];
        if (newRow >= 0 &&
            newRow < rows &&
            newCol >= 0 &&
            newCol < cols &&
            grid[newRow, newCol] == plantType &&
            !visited[newRow, newCol])
        {
          visited[newRow, newCol] = true;
          queue.Enqueue((newRow, newCol));
        }
      }
    }

    return measureCalculator(grid, regionCells);
  }

  private static (int, int) CalculateSides(char[,] grid, List<(int, int)> regionCells)
  {
    int rows = grid.GetLength(0), cols = grid.GetLength(1);
    int lineEdge = 0;

    foreach ((int row, int col) in regionCells)
    {
      int counter = 0;
      string updateDirection = "";
      var sameDirection = new HashSet<(int, int)>();

      foreach (int[] direction in Directions)
      {
        char goingIn = DirectionList[counter];
        bool skip = GridLine[(row, col)].visited[counter] == goingIn;
        int newRow = row + direction[0], newCol = col + direction[1];
        bool isOutOfBounds = newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols;

        if (isOutOfBounds || grid[newRow, newCol] != grid[row, col])
        {
          updateDirection += goingIn.ToString();

          if (!skip)
          {

            lineEdge++;
          }
        }
        else
        {
          updateDirection += " ";
          sameDirection.Add((newRow, newCol));
        }

        counter++;
      }

      foreach ((int sameRow, int sameCol) in sameDirection)
      {
        GridLine[(sameRow, sameCol)].visited = CombineStrings(GridLine[(sameRow, sameCol)].visited, updateDirection);
      }
    }

    return (regionCells.Count, lineEdge);
  }

  private static string CombineStrings(string str1, string str2)
  {
    char[] result = str1.ToCharArray();
    for (int i = 0; i < str1.Length && i < str2.Length; i++)
    {
      if (!char.IsWhiteSpace(str2[i]))
      {
        result[i] = str2[i];
      }
    }

    return new string(result);
  }

  private static (int, int) CalculatePerimeter(char[,] grid, List<(int, int)> regionCells)
  {
    int perimeter = 0;

    foreach ((int row, int col) in regionCells)
    {
      foreach (int[] direction in Directions)
      {
        int newRow = row + direction[0], newCol = col + direction[1];
        if (newRow < 0 ||
            newRow >= grid.GetLength(0) ||
            newCol < 0 ||
            newCol >= grid.GetLength(1) ||
            grid[newRow, newCol] != grid[row, col])
          perimeter++;
      }
    }

    return (regionCells.Count, perimeter);
  }
}

public class LineChecker
{
  public char value { get; set; }
  public string visited { get; set; }
}