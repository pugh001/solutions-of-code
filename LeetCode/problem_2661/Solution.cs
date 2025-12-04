using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_2661;

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
    int[] arr = [1, 3, 4, 2];
    int[][] mat = [[1, 4], [2, 3]];
    int expected = 2;

    // Act
    int result = FirstCompleteIndex(arr, mat);

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
    int[] arr = [2, 8, 7, 4, 1, 3, 5, 6, 9];
    int[][] mat = [[3, 2, 5], [1, 4, 6], [8, 7, 9]];

    int expected = 3;

    // Act
    int result = FirstCompleteIndex(arr, mat);

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
    int[] arr = [1, 4, 5, 2, 6, 3];
    int[][] mat = [[4, 3, 5], [1, 2, 6]];

    int expected = 1;

    // Act
    int result = FirstCompleteIndex(arr, mat);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void Solution_Test4()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[] arr = [6, 2, 3, 1, 4, 5];
    int[][] mat = [[5, 1], [2, 4], [6, 3]];

    int expected = 2;

    // Act
    int result = FirstCompleteIndex(arr, mat);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }


  public int FirstCompleteIndex(int[] arr, int[][] mat)
  {
    int n = mat.Length;
    int m = mat[0].Length;
    int[] rows = new int[m];
    int[] cols = new int[n];

    var matrixMap = new Dictionary<int, (int row, int col)>();

    for (int i = 0; i < n; i++)
    {
      for (int j = 0; j < m; j++)
      {
        int key = mat[i][j];
        matrixMap[key] = (j, i);
      }
    }

    for (int result = 0; result < arr.Length; result++)
    {
      int value = arr[result];
      (int row, int col) = matrixMap.GetValueOrDefault(value);
      rows[row]++;
      cols[col]++;
      if (rows[row] == n || cols[col] == m)
      {
        return result;
      }
    }

    return 0;
  }
}