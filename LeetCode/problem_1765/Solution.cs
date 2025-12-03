using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_1765;

public class Solution
{

  //Create Queue outside so not pass it around.
  private static readonly Queue<(int, int)> queue = new();
  private readonly ITestOutputHelper testOutputHelper;
  public Solution(ITestOutputHelper testOutputHelper)
  {
    this.testOutputHelper = testOutputHelper;
  }


  [Fact]
  public void Solution_Test1()
  {


    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] isWater = [[0, 1], [0, 0]];
    int[][] expected = [[1, 0], [2, 1]];

    // Act
    int[][]? result = HighestPeak(isWater);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void Solution_Test2()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] isWater = [[0, 0, 1], [1, 0, 0], [0, 0, 0]];
    int[][] expected = [[1, 1, 0], [0, 1, 1], [1, 2, 2]];

    // Act
    int[][]? result = HighestPeak(isWater);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public int[][] HighestPeak(int[][] isWater)
  {
    int rows = isWater.Length;
    int cols = isWater[0].Length;
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        isWater[r][c] = UpdateMatrix(isWater[r][c], r, c);
      }
    }

    BreadthFirstSearch(isWater, rows, cols); //BFS
    return isWater;

  }
  private static void BreadthFirstSearch(int[][] isWater, int rows, int cols)
  {
    int[][] directions = [[0, 1], [1, 0], [0, -1], [-1, 0]]; // # Right, Down, Left, Up
    // BFS traversal
    while (queue.Count > 0)
    {
      (int currentRow, int currentCol) = queue.Dequeue();
      foreach (int[] direction in directions)
      {
        int newRow = currentRow + direction[0];
        int newCol = currentCol + direction[1];
        UpdateMatrix(isWater, rows, cols, newRow, newCol, currentRow, currentCol);
      }
    }
  }
  private static void UpdateMatrix(int[][] isWater, int rows, int cols, int newRow, int newCol, int currentRow, int currentCol)
  {
    //Check bounds and if new cell is unvisited
    if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || isWater[newRow][newCol] != -1)
      return;

    isWater[newRow][newCol] = isWater[currentRow][currentCol] + 1;
    queue.Enqueue((newRow, newCol));
  }
  private static int UpdateMatrix(int isWater, int r, int c)
  {
    if (isWater != 1)
      return -1; // Placeholder for unvisited cells

    queue.Enqueue((r, c)); // Add all water cells to the queue
    return 0; //water = 0 in response but was 1 on way in.
  }
}