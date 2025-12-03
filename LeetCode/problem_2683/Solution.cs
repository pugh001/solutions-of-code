using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_2683;

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
    int[] derived = [1, 1];
    const bool expected = true;

    // Act
    bool result = DoesValidArrayExist(derived);

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
    int[] derived = [1, 1, 0];
    const bool expected = true;

    // Act
    bool result = DoesValidArrayExist(derived);

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
    int[] derived = [1, 0];
    const bool expected = false;

    // Act
    bool result = DoesValidArrayExist(derived);


    stopWatch.Stop();
    testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public bool DoesValidArrayExist(int[] derived)
  {
    /*
     If you take the examples and xor each value in array with next it seems that when result
     is 0 there is a solution but when 1 not.
     so the LINQ is same as a loop for each and then do this
         xorDerived = xorDerived ^ array element value
     Start at 0 so first value is same as initial.
     Bitwise XOR
     1 ^ 1 = 0
     0 ^ 0 = 0
     1 ^ 0 = 1
     0 ^ 1 =1
    */
    int xorDerived = 0;
    xorDerived = derived.Aggregate(xorDerived, (current, i) => current ^ i);
    return xorDerived == 0;
  }
}