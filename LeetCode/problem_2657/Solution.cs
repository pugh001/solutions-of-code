using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_2657;

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
    int[] a = [1, 3, 2, 4];
    int[] b = [3, 1, 2, 4];
    int[] expected = [0, 2, 3, 4];

    // Act
    int[] result = FindThePrefixCommonArray(a, b);

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
    int[] a = [2, 3, 1];
    int[] b = [3, 1, 2];
    int[] expected = [0, 1, 3];

    // Act
    int[] result = FindThePrefixCommonArray(a, b);

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
    int[] a = [4, 3, 2, 1];
    int[] b = [1, 2, 3, 4];
    int[] expected = [0, 0, 2, 4];

    // Act
    int[] result = FindThePrefixCommonArray(a, b);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public int[] FindThePrefixCommonArrayMine(int[] a, int[] b)
  {
    int n = a.Length;
    int[] result = new int[n];
    var seenInA = new HashSet<int>();
    var seenInB = new HashSet<int>();
    int matches = 0;

    for (int j = 0; j < n; j++)
    {
      if (seenInB.Contains(a[j])) matches++;
      seenInA.Add(a[j]);

      if (seenInA.Contains(b[j])) matches++;
      seenInB.Add(b[j]);

      result[j] = matches;
    }

    return result;
  }
  //This is the fastest solution on LeetCode,
  //so better as I use Contains
  // they directly check array for speed.
  public int[] FindThePrefixCommonArray(int[] a, int[] b)
  {
    int[] state = new int[a.Length];
    int[] res = new int[a.Length];

    int count = 0;

    for (int i = 0; i < a.Length; i++)
    {
      int ai = a[i] - 1; //Convert to 0 index
      int bi = b[i] - 1;

      state[ai]++; //Sets value seen count (Once 2 seen in both)
      state[bi]++;

      if (ai == bi) //Same so only check the one
      {
        if (state[ai] == 2)
          count++;
      }
      else //Different so check both to ignore sequence
      {
        if (state[ai] == 2)
          count++;
        if (state[bi] == 2)
          count++;
      }

      res[i] = count; //the count always going up.
      //So once seen a number it'll be included
      //so starts 0's then sequential.


    }

    return res;
  }
}