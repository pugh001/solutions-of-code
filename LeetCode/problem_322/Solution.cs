using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_322;

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
    int[] coins = [1, 2, 5];
    int amount = 11;
    int expected = 3;

    // Act
    int result = CoinChange(coins, amount);

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
    int[] coins = [2];
    int amount = 3;
    int expected = -1;

    // Act
    int result = CoinChange(coins, amount);

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
    int[] coins = [1];
    int amount = 0;
    int expected = 0;

    // Act
    int result = CoinChange(coins, amount);

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
    int[] coins = [1, 3, 4];
    int amount = 10;
    int expected = 3;

    // Act
    int result = CoinChange(coins, amount);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  //Updated based on Competitive Programmers Handbook version
  public int CoinChange(int[] coins, int amount)
  {
    const int inf = int.MaxValue; // Representing infinity
    int[] dp = new int[amount + 1]; // DP array to store the minimum coins for each amount

    dp[0] = 0; // Base case: 0 coins needed to make amount 0

    // Iterate over each amount from 1 to the target amount
    for (int x = 1; x <= amount; x++)
    {
      dp[x] = inf; // Initialize to "infinity"

      // Iterate over each coin
      foreach (int coin in coins)
      {
        if (x - coin >= 0 && dp[x - coin] != inf) // Check if the coin can contribute
        {
          dp[x] = Math.Min(dp[x], dp[x - coin] + 1);
        }
      }
    }

    // If the target amount is still unreachable, return -1
    return dp[amount] == inf ?
      -1 :
      dp[amount];
  }

  public int CoinChangeMySlowVersion(int[] coins, int amount)
  {

    /* can do recusive or dynamic, the coin problem exists in the competitive programmers handbook */

    // Dictionary for memoization to store results of subproblems
    var memo = new Dictionary<int, int>();

    return Solve(amount);

    // Recursive function to solve the problem
    int Solve(int remaining)
    {
      switch (remaining)
      {
        // Base cases
        case 0:
          return 0; // No coins needed for amount 0
        case < 0:
          return -1; // Invalid case, not possible to form the amount
      }

      // Check if result is already in the memo
      if (memo.ContainsKey(remaining)) return memo[remaining];

      int minCoins = int.MaxValue;
      foreach (int coin in coins)
      {
        int result = Solve(remaining - coin);
        if (result >= 0 && result < minCoins)
        {
          minCoins = result + 1; // Add 1 coin to the result
        }
      }

      // If no valid solution found, return -1
      memo[remaining] = minCoins == int.MaxValue ?
        -1 :
        minCoins;
      return memo[remaining];
    }
  }
}