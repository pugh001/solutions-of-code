using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_916;

public class Solution
{
  private readonly ITestOutputHelper _testOutputHelper;
  public Solution(ITestOutputHelper testOutputHelper)
  {
    this._testOutputHelper = testOutputHelper;
  }


  [Fact]
  public void WordSubsets_Test1()
  {
    var stopWatch = Stopwatch.StartNew();
    // Arrange
    string[] words1 = ["amazon", "apple", "facebook", "google", "leetcode"];
    string[] words2 = ["e", "o"];
    IList<string> expected = new List<string> { "facebook", "google", "leetcode" };

    // Act
    var result = WordSubsets(words1, words2);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void WordSubsets_Test3()
  {
    var stopWatch = Stopwatch.StartNew();
    // Arrange
    string[] words1 =
    [
      "dcbddbbbeb", "edeabaedbc", "beecbdbabe", "bacadddbda", "ecbdebddbb",
      "abeabbcaaa", "eabbdbadbb", "aacabeacde", "bcceeaccae", "ebbdebbcad"
    ];
    string[] words2 = ["add", "b", "ba", "ada", "dcd"];
    IList<string> expected = new List<string> { "edeabaedbc", "bacadddbda" };

    // Act
    var result = WordSubsets(words1, words2);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public IList<string> WordSubsets(string[] words1, string[] words2)
  {
    /*
     * Count all the same letters in each word2 and store the max of that,
     * Then in each word 1 see if every letter less or equal to that letters max frequency
     * if pass then all word2's are valid.
     * If you have "oo" and "ao" in word2, then o=2 as needs to pass max,
     * the "ao" o=1 not matter if you can't pass o=2
     */
    // Compute the maximum frequency requirements from words2
    int[] maxFreq = MaxFreqWord2(words2);

    // Validate each word in words1 against maxFreq
    var result = new List<string>();
    foreach (string word in words1)
    {
      int[] freq = GetCharFrequency(word);
      bool isUniversal = true;

      for (int i = 0; i < 26; i++)
      {
        if (freq[i] >= maxFreq[i])
          continue;

        isUniversal = false;
        break;
      }

      if (isUniversal)
      {
        result.Add(word);
      }
    }

    return result;

  }
  private static int[] MaxFreqWord2(string[] words2)
  {

    int[] maxFreq = new int[26];
    foreach (string word in words2)
    {
      int[] freq = GetCharFrequency(word);
      for (int i = 0; i < 26; i++)
      {
        maxFreq[i] = Math.Max(maxFreq[i], freq[i]);
      }
    }

    return maxFreq;
  }
  private static int[] GetCharFrequency(string word)
  {
    int[] freq = new int[26];
    foreach (char c in word)
    {
      freq[c - 'a']++;
    }

    return freq;

  }
}