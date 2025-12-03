using Utility;

namespace AOC2024;

public class Day18
{


  private static int _rows = 70;
  private static int _cols = 70;
  private static int _enough = 1024;
  private static HashSet<(int, int)> Grid = []; // Stores corrupted locations (#)
  private static List<(int, int)> ExtraList = [];


  private static readonly (int, int)[] Directions =
  [
    (0, 1), // Right
    (1, 0), // Down
    (0, -1), // Left
    (-1, 0) // Up
  ];

  public (string, string) Process(string input)
  {
    if (input.Contains("Example"))
    {
      _rows = 6;
      _cols = 6;
      _enough = 12;
      Grid = []; // Stores corrupted locations (#)
      ExtraList = [];
    }

    // Load and parse input data
    string[] data = SetupInputFile.OpenFile(input).ToArray();

    // Initialize grid based on the input
    InitializeGrid(data);
    // Run the BFS to find the shortest path
    int steps = ProcessMaze();

    string resultPart2 = ProcessPart2();


    return (steps.ToString(), resultPart2); // Return the result (steps to the exit)
  }
  private static string ProcessPart2()
  {

    // Add extra list obstacles and check paths incrementally
    string resultPart2 = "";
    foreach (var position in ExtraList)
    {
      Grid.Add(position); // Add new obstacle
      int result = ProcessMaze();
      if (result >= 0)
        continue;

      resultPart2 = $"{position.Item1},{position.Item2}";
      break;
    }

    return resultPart2;
  }


  private static int ProcessMaze()
  {
    // BFS initialization
    Queue<(int x, int y, int steps)> queue = new();
    bool[,] visited = new bool[_rows + 1, _cols + 1]; // Grid size based on _rows and _cols

    // Check if start or end is corrupted
    if (Grid.Contains((0, 0)) || Grid.Contains((_rows, _cols)))
    {
      return -1; // Impossible to start or finish
    }

    // Starting point (0, 0)
    queue.Enqueue((0, 0, 0)); // (x, y, steps)
    visited[0, 0] = true;

    while (queue.Count > 0)
    {
      (int x, int y, int steps) = queue.Dequeue();

      // If we've reached the exit (_rows, _cols)
      if (x == _rows && y == _cols)
      {
        return steps;
      }

      // Explore all four directions
      foreach ((int dx, int dy) in Directions)
      {
        int newX = x + dx;
        int newY = y + dy;

        // Check if the new position is within bounds and not corrupted
        if (!IsValid(newX, newY) || visited[newX, newY] || Grid.Contains((newX, newY)))
          continue;

        visited[newX, newY] = true;
        queue.Enqueue((newX, newY, steps + 1));
      }
    }

    // If no path is found
    return -1;
  }

  private static bool IsValid(int x, int y)
  {
    return x >= 0 && x <= _rows && y >= 0 && y <= _cols;
  }

  private static void InitializeGrid(string[] data)
  {
    int counter = 1;

    // Initially, the grid is empty (no obstacles)
    foreach (string t in data)
    {
      string[] hashPosit = t.Split(',');
      int x = int.Parse(hashPosit[0]);
      int y = int.Parse(hashPosit[1]);

      if (counter > _enough)
      {
        ExtraList.Add((x, y));
      }
      else
      {
        Grid.Add((x, y)); // Mark as corrupted
      }

      counter++;
    }
  }
}