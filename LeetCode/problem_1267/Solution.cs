using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_1267;

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
    int[][] grid = [[1, 0], [0, 1]];
    int expected = 0;

    // Act
    int result = CountServers(grid);

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
    int[][] grid =
    [
      [1, 0], // = 1
      [1, 1]
    ]; // = 2
    // 2 1
    //The 2nd loop fixes as will only count if == 1 & count > 1
    int expected = 3;

    // Act
    int result = CountServers(grid);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  [Fact]
  public void Solution_Test3()
  {
    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] grid =
    [
      [1, 1, 0, 0],
      [0, 0, 1, 0],
      [0, 0, 1, 0],
      [0, 0, 0, 1]
    ];
    int expected = 4;


    // Act
    int result = CountServers(grid);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }


  public int CountServers(int[][] grid)
  {
    int m = grid.Length;
    int n = grid[0].Length;
    /*
     * Count > 1 then its included.
     * But have the double count of corner as 1 in both.
     * So need to ignore, therefore the second loop needed,
     * check the grid and then add it if row or col > 1
     * Not using my sum or row and cols, so it ignores duplicate.
     * Think this faster then DFS
     */
    int result = 0;
    (int[] rowCounts, int[] colCounts) = GetCounts(grid, m, n);
    // Step 2: Identify communicating servers
    for (int i = 0; i < m; i++)
    {
      for (int j = 0; j < n; j++)
      {
        if (grid[i][j] == 1 && (rowCounts[i] > 1 || colCounts[j] > 1))
        {
          result++;
        }
      }
    }

    return result;
  }
  private static (int[] rowCounts, int[] colCounts) GetCounts(int[][] grid, int m, int n)
  {
    int[] rowCounts = new int[m];
    int[] colCounts = new int[n];
    // Step 1: Count servers in each row and column
    for (int i = 0; i < m; i++)
    {
      for (int j = 0; j < n; j++)
      {
        if (grid[i][j] != 1)
          continue;

        rowCounts[i]++;
        colCounts[j]++;
      }
    }

    return (rowCounts, colCounts);
  }
}