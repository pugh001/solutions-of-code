using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_2017;

public class Solution
{
  private readonly ITestOutputHelper _testOutputHelper;
  public Solution(ITestOutputHelper testOutputHelper)
  {
    this._testOutputHelper = testOutputHelper;
  }


  [Fact]
  public void Solution_Test2()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] grid = [[1, 3, 1, 15], [1, 3, 3, 1]];
    long expected = 7;

    // Act
    long result = GridGame(grid);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void Solution_Test1()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] grid = [[2, 5, 4], [1, 5, 1]];
    long expected = 4;

    // Act
    long result = GridGame(grid);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public long GridGame(int[][] grid)
  {
    int n = grid[0].Length;

    // Compute the total sum of points in the top row
    long topRowSum = 0;
    for (int i = 0; i < n; i++)
    {
      topRowSum += grid[0][i];
    }

    long minSecondRobotPoints = long.MaxValue;
    long currentBottomRowPoints = 0;

    // Simulate the first robot's path
    for (int i = 0; i < n; i++)
    {
      // Points left in the top row if the first robot moves down after column i
      topRowSum -= grid[0][i];

      // Points collected by the second robot
      minSecondRobotPoints = Math.Min(minSecondRobotPoints, Math.Max(topRowSum, currentBottomRowPoints));

      // Update bottom row points for the first robot
      currentBottomRowPoints += grid[1][i];
    }

    return minSecondRobotPoints;

  }
}