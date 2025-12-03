using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_57;

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
    int[][] intervals = [[1, 3], [6, 9]];
    int[] newInterval = [2, 5];
    int[][] expected = [[1, 5], [6, 9]];

    // Act
    int[][]? result = Insert(intervals, newInterval);

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
    int[][] intervals = [[1, 2], [3, 5], [6, 7], [8, 10], [12, 16]];
    int[] newInterval = [4, 8];
    int[][] expected = [[1, 2], [3, 10], [12, 16]];

    // Act
    int[][]? result = Insert(intervals, newInterval);

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
    int[][] intervals = [];
    int[] newInterval = [5, 7];
    int[][] expected = [[5, 7]];

    // Act
    int[][]? result = Insert(intervals, newInterval);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }


  public int[][] Insert(int[][] intervals, int[] newInterval)
  {
    if (intervals.Length == 0) return [newInterval];

    var result = new List<int[]>(); //Dynamic sizing on a list
    int i = 0;
    int n = intervals.Length;

    // Add all intervals that end before the new interval starts.
    while (i < n && intervals[i][1] < newInterval[0])
    {
      result.Add(intervals[i]);
      i++;
    }

    if (i == n) //not really needed but allows skipping next 2 loops
    {
      result.Add(newInterval);
      return result.ToArray();
    }

    // Merge all overlapping intervals,
    // take the start of current and if less than current use it.
    // then keep looping for the end point.
    newInterval[0] = Math.Min(newInterval[0], intervals[i][0]);
    while (i < n && intervals[i][0] <= newInterval[1])
    {
      newInterval[1] = Math.Max(newInterval[1], intervals[i][1]);
      i++;
    }

    // Add the merged new interval to the result.
    result.Add(newInterval);

    // Add all intervals that start after the new interval ends.
    while (i < n)
    {
      result.Add(intervals[i]);
      i++;
    }

    return result.ToArray();
  }
}