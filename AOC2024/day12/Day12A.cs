using Utility;

namespace AOC2024;

public class Day12A
{
  private static readonly int[][] Directions =
  [
    [0, 1], // Right
    [1, 0], // Down
    [0, -1], // Left
    [-1, 0] // Up
  ];

  public (string, string) Process(string input)
  {
    string[]? data = SetupInputFile.OpenFile(input).ToArray();
    if (data.Length == 0)
      throw new ArgumentException("Input is empty. Please provide valid data.");

    char[,]? grid = ParseGrid(data);
    return (CalculateTotalFenceCost(grid, CalculateRegionOne).ToString(), // Part 1: Perimeter-based
            CalculateRegionTwo(grid).ToString()) // Part 2: Sides-based
      ;
  }

  private static char[,] ParseGrid(string[] data)
  {
    int rows = data.Length, cols = data[0].Length;
    if (data.Any(row => row.Length != cols))
      throw new ArgumentException("Input grid rows are not uniform in length.");

    char[,]? grid = new char[rows, cols];
    for (int r = 0; r < rows; r++)
      for (int c = 0; c < cols; c++)
        grid[r, c] = data[r][c];

    return grid;
  }

  private static long CalculateTotalFenceCost(char[,] grid, Func<char[,], bool[,], int, int, (int, int)> calculateRegion)
  {
    int rows = grid.GetLength(0), cols = grid.GetLength(1);
    bool[,]? visited = new bool[rows, cols];
    long totalCost = 0;

    for (int r = 0; r < rows; r++)
      for (int c = 0; c < cols; c++)
        if (!visited[r, c])
        {
          (int area, int measure) = calculateRegion(grid, visited, r, c);
          totalCost += area * measure;
        }

    return totalCost;
  }

  private static (int, int) CalculateRegionOne(char[,] grid, bool[,] visited, int startRow, int startCol)
  {
    // Part 1: Calculate using perimeter
    return CalculateRegion(grid, visited, startRow, startCol, CalculatePerimeter);
  }

  private static long CalculateRegionTwo(char[,] grid)
  {
    // Part 2: Calculate using sides
    var regionDetails = FindRegions(grid);
    long totalPrice = CalculateTotalPrice(regionDetails);
    return totalPrice;
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

    int[][]? directions = new[] { new[] { 0, 1 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { -1, 0 } };
    var queue = new Queue<(int, int)>();
    var regionCells = new List<(int, int)>();

    queue.Enqueue((startRow, startCol));
    visited[startRow, startCol] = true;

    while (queue.Count > 0)
    {
      (int row, int col) = queue.Dequeue();
      area++;
      regionCells.Add((row, col));

      foreach (int[]? direction in directions)
      {
        int newRow = row + direction[0], newCol = col + direction[1];
        if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
        {
          if (grid[newRow, newCol] == plantType && !visited[newRow, newCol])
          {
            visited[newRow, newCol] = true;
            queue.Enqueue((newRow, newCol));
          }
        }
      }
    }

    (int _, int measureValue) = measureCalculator(grid, regionCells);
    return (area, measureValue);
  }

  private static (int, int) CalculatePerimeter(char[,] grid, List<(int, int)> regionCells)
  {
    int rows = grid.GetLength(0), cols = grid.GetLength(1);
    int perimeter = 0;


    foreach ((int row, int col) in regionCells)
    {
      foreach (int[]? direction in Directions)
      {
        int newRow = row + direction[0];
        int newCol = col + direction[1];
        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || grid[newRow, newCol] != grid[row, col])
          perimeter++;
      }
    }

    return (regionCells.Count, perimeter);
  }

  // DFS/BFS to find regions and calculate area and fence
  private static List<Region> FindRegions(char[,] grid)
  {
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    bool[,]? visited = new bool[rows, cols]; // To track visited cells in the grid
    var regions = new List<Region>();

    // Go through each cell in the grid
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        if (visited[r, c]) continue;

        // Start a new region if we find an unvisited cell
        char plantType = grid[r, c];
        var region = new Region(plantType);
        DFS(grid, r, c, visited, region);
        regions.Add(region);
      }
    }

    return regions;
  }

  // Perform DFS to identify the entire region
  private static void DFS(char[,] grid, int r, int c, bool[,] visited, Region region)
  {
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    var stack = new Stack<(int, int)>();
    stack.Push((r, c));
    visited[r, c] = true;

    while (stack.Count > 0)
    {
      (int curR, int curC) = stack.Pop();
      region.Area++;

      // Check all 4 neighbors
      for (int i = 0; i < Directions.Length; i++)
      {
        int nr = curR + Directions[i][0];
        int nc = curC + Directions[i][1];

        // If the neighbor is within bounds
        if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
        {
          // If the neighbor has the same plant type and is not visited yet
          if (!visited[nr, nc] && grid[nr, nc] == grid[curR, curC])
          {
            stack.Push((nr, nc));
            visited[nr, nc] = true;
          }
        }
        // If out of bounds, it adds to the fence (only if it's not counted yet)
        else
        {
          // Add to the perimeter (this is an out-of-bounds fence)
          region.Perimeter++;
        }
      }
    }
  }

  // Calculate the total price by summing up the area * perimeter of each region
  private static long CalculateTotalPrice(List<Region> regions)
  {
    long totalPrice = 0;
    foreach (var region in regions)
    {
      totalPrice += region.Area * region.Perimeter;
    }

    return totalPrice;
  }
}

// Region class to hold the details of each region (plant type, area, and perimeter)
public class Region
{

  public Region(char plantType)
  {
    PlantType = plantType;
    Area = 0;
    Perimeter = 0;
  }
  public char PlantType { get; }
  public int Area { get; set; }
  public int Perimeter { get; set; }
}