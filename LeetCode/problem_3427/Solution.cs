using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_3427;

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
    int[] nums = [2, 3, 1];
    int expected = 11;

    // Act
    int result = SubarraySum(nums);

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
    int[] nums = [3, 1, 1, 2];
    int expected = 13;

    // Act
    int result = SubarraySum(nums);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  public int SubarraySum(int[] nums)
  {
    int n = nums.Length;
    int[] prefix = new int[n + 1];

    for (int i = 0; i < n; i++)
    {
      prefix[i + 1] = prefix[i] + nums[i];
    }

    int totalSum = 0;
    for (int i = 0; i < n; i++)
    {
      int start = Math.Max(0, i - nums[i]);
      totalSum += prefix[i + 1] - prefix[start];
    }

    return totalSum;
  }
}