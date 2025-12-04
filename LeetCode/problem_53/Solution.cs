using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_53;

public class Solution
{
  private readonly ITestOutputHelper _testOutputHelper;
  public Solution(ITestOutputHelper testOutputHelper)
  {
    this._testOutputHelper = testOutputHelper;
  }

  [Fact]
  public void Solution_Test1()
  {
    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[] nums = [-2, 1, -3, 4, -1, 2, 1, -5, 4];
    int expected = 6;

    // Act
    int result = MaxSubArray(nums);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void Solution_Test2()
  {
    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[] nums = [1];
    int expected = 1;

    // Act
    int result = MaxSubArray(nums);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void Solution_Test3()
  {
    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[] nums = [5, 4, -1, 7, 8];
    int expected = 23;

    // Act
    int result = MaxSubArray(nums);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }


  public int MaxSubArray(int[] nums)
  {
    //Kadane's Algorithm
    int currentSum = nums[0];
    int bestSum = nums[0];
    for (int i = 1; i < nums.Length; i++)
    {
      currentSum = Math.Max(nums[i], currentSum + nums[i]);
      bestSum = Math.Max(bestSum, currentSum);
    }

    return bestSum;
  }
}