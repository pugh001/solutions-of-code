using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_2425;

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
    int[] nums1 = [2, 1, 3];
    int[] nums2 = [10, 2, 5, 0];
    int expected = 13;

    // Act
    int result = XorAllNums(nums1, nums2);

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
    int[] nums1 = [1, 2];
    int[] nums2 = [3, 4];
    int expected = 0;

    // Act
    int result = XorAllNums(nums1, nums2);

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
    int[] nums1 =
    [
      365, 744, 407, 833, 993, 455, 904, 808, 116, 853, 121, 380, 137, 53, 846, 50, 338, 460, 630, 276, 509, 48, 530, 440, 975,
      434, 556, 875, 795, 317, 749, 164, 736, 554, 887, 455, 706, 311, 682, 548, 56, 632, 818, 538, 681, 312, 837, 833, 565, 842,
      725, 27, 330, 0, 572, 701, 343, 967, 287, 959, 113, 136, 538, 752, 454, 22, 805, 421, 281, 906, 119, 51, 152, 632, 848, 158,
      19, 997, 184, 447, 38, 515, 440, 540, 195, 743, 939, 476, 860, 77, 66
    ];
    int[] nums2 =
    [
      537, 817, 983, 527, 547, 804, 300, 486, 96, 674, 654, 71, 465, 441, 675, 287, 749, 38, 501, 967, 292, 460, 763, 611, 105,
      27, 215, 658, 328, 37, 864, 581, 683, 499, 325, 884, 954, 601, 86, 981, 926, 273, 586, 139, 246, 293, 107, 157, 635, 738,
      693, 888, 598, 433, 860, 165, 718, 502, 31, 164, 689, 604, 213
    ];
    int expected = 772;

    // Act
    int result = XorAllNums(nums1, nums2);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public int XorAllNums(int[] nums1, int[] nums2)
  {
    bool num1Even = nums1.Length % 2 == 0;
    bool num2Even = nums2.Length % 2 == 0;
    if (num1Even && num2Even) return 0;

    int xorNums1 = 0, xorNums2 = 0;

    // XOR all elements in nums1
    xorNums1 = nums1.Aggregate(xorNums1, (current, i) => current ^ i);

    // XOR all elements in nums2
    xorNums2 = nums2.Aggregate(xorNums2, (current, i) => current ^ i);

    // Check the sizes of nums1 and nums2
    int result = 0; //Both even this is result.
    if (!num2Even) // If nums2.Length is odd
    {
      result = xorNums1; // 0 xor ## = ##
    }

    if (!num1Even) // If nums1.Length is odd
    {
      result ^= xorNums2; // result might be number, so need to xor it here
    }

    return result;
  }
}