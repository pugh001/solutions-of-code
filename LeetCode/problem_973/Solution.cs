using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_973;

public class Solution
{
  private readonly ITestOutputHelper _testOutputHelper;
  public Solution(ITestOutputHelper testOutputHelper)
  {
    _testOutputHelper = testOutputHelper;
  }

  [Fact]
  public void KClosest_Test1()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] points = [[1, 3], [-2, 2]];
    const int k = 1;
    int[][] expected = [[-2, 2]];
    // Act
    int[][] result = KClosest(points, k);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  [Fact]
  public void KClosest_Test2()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] points = [[3, 3], [5, -1], [-2, 4]];
    const int k = 2;
    int[][] expected = [[3, 3], [-2, 4]];
    // Act
    int[][] result = KClosest(points, k);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  public int[][] KClosest(int[][] points, int k)
  {
    return points.OrderBy(point => Math.Pow(point[0], 2) + Math.Pow(point[1], 2)) // Calculate squared distance
      .Take(k) // Take the first k points
      .ToArray();

    /*
     The longer route is to create an array of distance, and index.
     so (double distance, int indexInPoints) = new (double,int)[n] where n is length
     then loop through and add the sum of the powers
     Then sort Array.Sort (distances, (a, b) => a.distance.CompareTo(b.distance)) by distance
     Then jut create a new array of results and loop through sorted array k times,
     adding the points array to it that is the distance index pointer But the LINQ above works...
     */

  }
}