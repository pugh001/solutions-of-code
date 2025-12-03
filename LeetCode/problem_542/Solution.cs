using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_542;

public class Solution
{
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
    int[][] mat = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
    int[][] expected = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];

    // Act
    int[][]? result = UpdateMatrix(mat);

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
    int[][] mat = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];
    int[][] expected = [[0, 0, 0], [0, 1, 0], [0, 0, 0]];

    // Act
    int[][]? result = UpdateMatrix(mat);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }


  public int[][] UpdateMatrix(int[][] mat)
  {

    int rows = mat.Length;
    int cols = mat[0].Length;
    var queue = new Queue<(int, int)>();

    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        if (mat[r][c] == 0)
          queue.Enqueue((r, c)); // Add all 0 cells to the queue
        else
          mat[r][c] = -1; // Placeholder for unvisited cells
      }
    }

    int[][] directions = [[0, 1], [1, 0], [0, -1], [-1, 0]]; // # Right, Down, Left, Up

    // BFS traversal
    while (queue.Count > 0)
    {
      (int currentRow, int currentCol) = queue.Dequeue();
      foreach (int[] direction in directions)
      {
        int newRow = currentRow + direction[0];
        int newCol = currentCol + direction[1];

        //Check bounds and if new cell is unvisited
        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || mat[newRow][newCol] != -1)
          continue;

        mat[newRow][newCol] = mat[currentRow][currentCol] + 1;
        queue.Enqueue((newRow, newCol));
      }
    }

    return mat;

  }
}