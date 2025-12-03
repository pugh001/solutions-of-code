using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_2657;

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
    int[] A = [1, 3, 2, 4];
    int[] B = [3, 1, 2, 4];
    int[] expected = [0, 2, 3, 4];

    // Act
    int[] result = FindThePrefixCommonArray(A, B);

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
    int[] A = [2, 3, 1];
    int[] B = [3, 1, 2];
    int[] expected = [0, 1, 3];

    // Act
    int[] result = FindThePrefixCommonArray(A, B);

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
    int[] A = [4, 3, 2, 1];
    int[] B = [1, 2, 3, 4];
    int[] expected = [0, 0, 2, 4];

    // Act
    int[] result = FindThePrefixCommonArray(A, B);

    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public int[] FindThePrefixCommonArrayMine(int[] A, int[] B)
  {
    int n = A.Length;
    int[] result = new int[n];
    var seenInA = new HashSet<int>();
    var seenInB = new HashSet<int>();
    int matches = 0;

    for (int j = 0; j < n; j++)
    {
      if (seenInB.Contains(A[j])) matches++;
      seenInA.Add(A[j]);

      if (seenInA.Contains(B[j])) matches++;
      seenInB.Add(B[j]);

      result[j] = matches;
    }

    return result;
  }
  //This is the fastest solution on LeetCode,
  //so better as I use Contains
  // they directly check array for speed.
  public int[] FindThePrefixCommonArray(int[] A, int[] B)
  {
    int[] state = new int[A.Length];
    int[] res = new int[A.Length];

    int count = 0;

    for (int i = 0; i < A.Length; i++)
    {
      int ai = A[i] - 1; //Convert to 0 index
      int bi = B[i] - 1;

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