using Utility;

namespace AOC2024;

public class Day6
{
  public (string, string) Process(string input)
  {
    // Load and parse input data
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    return (ProcessPart1(data).ToString(), ProcessPart2(data).ToString());
  }

  private static long ProcessPart1(string[] data)
  {
    (char[,] grid, int guardRow, int guardCol) = InitializeGrid(data);
    (int, int)[] deltas = { (-1, 0), (0, 1), (1, 0), (0, -1) };
    int dirIndex = 0;

    // Simulate guard movement
    while (true)
    {
      int newRow = guardRow + deltas[dirIndex].Item1;
      int newCol = guardCol + deltas[dirIndex].Item2;

      if (IsOutOfBounds(newRow, newCol, grid) || grid[newRow, newCol] == '#')
      {
        // Turn right if blocked
        dirIndex = (dirIndex + 1) % 4;
      }
      else
      {
        grid[guardRow, guardCol] = 'X'; // Mark visited
        guardRow = newRow;
        guardCol = newCol;
      }

      if (IsOutOfBounds(newRow, newCol, grid))
        break;
    }

    return grid.Cast<char>().LongCount(cell => cell == 'X');
  }

  private static (char[,], int, int) InitializeGrid(string[] data)
  {
    int rows = data.Length;
    int cols = data[0].Length;
    char[,] grid = new char[rows, cols];
    int guardRow = -1, guardCol = -1;

    for (int i = 0; i < rows; i++)
    {
      for (int j = 0; j < cols; j++)
      {
        grid[i, j] = data[i][j];
        if (grid[i, j] != '^')
          continue;

        guardRow = i;
        guardCol = j;
        grid[i, j] = '.'; // Remove guard marker
      }
    }

    return (grid, guardRow, guardCol);
  }

  private static bool[,] ComputeReachable(char[,] grid, int guardRow, int guardCol)
  {
    bool[,] reachable = new bool[grid.GetLength(0), grid.GetLength(1)];
    Queue<(int, int)> queue = new();
    queue.Enqueue((guardRow, guardCol));
    (int, int)[] deltas = { (-1, 0), (0, 1), (1, 0), (0, -1) };

    while (queue.Count > 0)
    {
      (int row, int col) = queue.Dequeue();
      if (reachable[row, col])
        continue;

      reachable[row, col] = true;

      foreach ((int dr, int dc) in deltas)
      {
        int newRow = row + dr, newCol = col + dc;
        if (!IsOutOfBounds(newRow, newCol, grid) && grid[newRow, newCol] == '.' && !reachable[newRow, newCol])
          queue.Enqueue((newRow, newCol));
      }
    }

    return reachable;
  }

  private static bool SimulateGuardMovement(int guardRow, int guardCol, char[,] grid)
  {
    (int, int)[] deltas = { (-1, 0), (0, 1), (1, 0), (0, -1) };
    int currentRow = guardRow;
    int currentCol = guardCol;
    int dirIndex = 0;

    HashSet<int> visited = new();
    while (true)
    {
      int newRow = currentRow + deltas[dirIndex].Item1;
      int newCol = currentCol + deltas[dirIndex].Item2;

      if (IsOutOfBounds(newRow, newCol, grid))
        return false;

      if (grid[newRow, newCol] == '#')
      {
        // Turn right if blocked
        dirIndex = (dirIndex + 1) % 4;
      }
      else
      {
        // Move forward
        currentRow = newRow;
        currentCol = newCol;
      }

      // Check for loop using a compact state representation
      int state = currentRow << 16 | currentCol << 8 | dirIndex;
      if (!visited.Add(state))
        return true; // Loop detected
    }
  }

  private static bool IsOutOfBounds(int row, int col, char[,] grid)
  {
    return row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1);
  }

  private static long ProcessPart2(string[] map)
  {
    int rows = map.Length;
    int cols = map[0].Length;

    var visited = new HashSet<(int, int)>();
    var directions = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
    int currentDirection = 0;

    int guardRow = 0, guardCol = 0;
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        if (map[r][c] == '^')
        {
          guardRow = r;
          guardCol = c;
          break;
        }
      }
    }

    visited.Add((guardRow, guardCol));
    bool[,] isObstacle = new bool[rows, cols];
    for (int r = 0; r < rows; r++)
      for (int c = 0; c < cols; c++)
        isObstacle[r, c] = map[r][c] == '#';

    var potentialLoopObstacles = new HashSet<(int, int)>();

    while (true)
    {
      int nextRow = guardRow + directions[currentDirection].Item1;
      int nextCol = guardCol + directions[currentDirection].Item2;

      if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= cols || isObstacle[nextRow, nextCol])
      {
        currentDirection = (currentDirection + 1) % 4;
        continue;
      }

      guardRow = nextRow;
      guardCol = nextCol;

      if (visited.Contains((guardRow, guardCol)))
      {
        for (int i = 0; i < 4; i++)
        {
          int adjRow = guardRow + directions[i].Item1;
          int adjCol = guardCol + directions[i].Item2;

          if (adjRow >= 0 &&
              adjRow < rows &&
              adjCol >= 0 &&
              adjCol < cols &&
              !isObstacle[adjRow, adjCol] &&
              !(adjRow == guardRow && adjCol == guardCol))
          {
            potentialLoopObstacles.Add((adjRow, adjCol));
          }
        }

        break;
      }

      visited.Add((guardRow, guardCol));
    }

    return potentialLoopObstacles.Count;
  }
}