using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_3;

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
    string s = "abcabcbb";
    int expected = 3;

    // Act
    int result = LengthOfLongestSubstring(s);

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
    string s = "bbbbbbb";
    int expected = 1;

    // Act
    int result = LengthOfLongestSubstring(s);

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
    string s = "pwwkew";
    int expected = 3;

    // Act
    int result = LengthOfLongestSubstring(s);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }


  public int LengthOfLongestSubstring(string s)
  {
    if (string.IsNullOrEmpty(s)) return 0;

    var charIndexMap = new Dictionary<char, int>();
    int maxLength = 0;
    int left = 0;

    for (int right = 0; right < s.Length; right++)
    {
      // If the character is already in the map and is within the current window
      // Try Get fetches last time saw this letter, if seen in window update left
      if (charIndexMap.TryGetValue(s[right], out int value) && value >= left)
      {
        // Move the left pointer right past the previous occurrence
        left = value + 1;
      }

      // Update the last seen index of the current character or create if not there.
      charIndexMap[s[right]] = right;

      // Calculate the length of the current window
      maxLength = Math.Max(maxLength, right - left + 1);
    }

    return maxLength;
  }
}