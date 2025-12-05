using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_15;

public class Solution
{
  private readonly ITestOutputHelper _testOutputHelper;
  public Solution(ITestOutputHelper testOutputHelper)
  {
    _testOutputHelper = testOutputHelper;
  }

  [Fact]
  public void Solution_Test1()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[] nums = { -1, 0, 1, 2, -1, -4 };
    IList<IList<int>> expected = new List<IList<int>> { new List<int> { -1, -1, 2 }, new List<int> { -1, 0, 1 } };

    // Act
    var result = ThreeSum(nums);

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
    int[] nums = { 0, 1, 1 };
    IList<IList<int>> expected = new List<IList<int>>();

    // Act
    var result = ThreeSum(nums);

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
    int[] nums = { 0, 0, 0 };
    IList<IList<int>> expected = new List<IList<int>> { new List<int> { 0, 0, 0 } };

    // Act
    var result = ThreeSum(nums);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  public IList<IList<int>> ThreeSum(int[] nums)
  {
    Array.Sort(nums); // Step 1: Sort the array.
    var result = new List<IList<int>>();

    for (int i = 0; i < nums.Length - 2; i++)
    {
      if (ShouldSkipFixedIndex(nums, i)) continue;

      FindPairs(nums, i, result);
    }

    return result;
  }

  private bool ShouldSkipFixedIndex(int[] nums, int i)
  {
    return i > 0 && nums[i] == nums[i - 1];
  }

  private void FindPairs(int[] nums, int fixedIndex, List<IList<int>> result)
  {
    int left = fixedIndex + 1, right = nums.Length - 1;

    while (left < right)
    {
      int sum = nums[fixedIndex] + nums[left] + nums[right];

      if (sum == 0)
      {
        result.Add(new List<int> { nums[fixedIndex], nums[left], nums[right] });
        SkipDuplicates(ref left, ref right, nums);
      }
      else if (sum < 0)
      {
        left++;
      }
      else
      {
        right--;
      }
    }
  }

  private void SkipDuplicates(ref int left, ref int right, int[] nums)
  {
    int currentLeft = nums[left];
    int currentRight = nums[right];

    while (left < right && nums[left] == currentLeft) left++;
    while (left < right && nums[right] == currentRight) right--;
  }
}