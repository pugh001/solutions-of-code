namespace Utility;

/// <summary>
/// Core utilities that serve as the foundation for other utility classes.
/// Contains wrapper methods for backward compatibility and essential methods that don't belong in specialized folders.
/// </summary>
public static class Sequences
{
  /// <summary>
  /// Gets the highest subsequence of specified length from a string using greedy algorithm
  /// </summary>
  public static string GetHighestSubsequence(string input, int targetLength)
  {
    if (input.Length == targetLength) return input;

    var result = new List<char>();
    int remaining = targetLength;

    for (int i = 0; i < input.Length; i++)
    {
      // While we have digits in result and current digit is larger than the last one,
      // and we still have enough remaining digits to complete the sequence
      while (result.Count > 0 && input[i] > result[result.Count - 1] && result.Count - 1 + input.Length - i >= targetLength)
      {
        result.RemoveAt(result.Count - 1);
        remaining++;
      }

      // Add current digit if we still need more digits
      if (remaining > 0)
      {
        result.Add(input[i]);
        remaining--;
      }
    }

    return new string(result.ToArray());
  }

  /// <summary>
  /// Checks if a number string has repeated sequences (entire string formed by repeating pattern)
  /// </summary>
  public static bool HasRepeatedSequence(string numberStr)
  {
    int length = numberStr.Length;

    // Try all possible pattern lengths from 1 to length/2
    for (int patternLength = 1; patternLength <= length / 2; patternLength++)
    {
      // Check if the entire string can be formed by repeating a pattern of this length
      if (length % patternLength != 0)
        continue;

      string pattern = numberStr.Substring(0, patternLength);

      int repetitions = length / patternLength;

      // Check if this pattern repeats at least twice
      if (repetitions < 2)
        continue;

      if (CheckRepeats(numberStr, repetitions, patternLength, pattern))
        return true;
    }

    return false; // No repeated pattern found
  }
  
  private static bool CheckRepeats(string numberStr, int repetitions, int patternLength, string pattern)
  {
    bool isRepeated = true;
    for (int i = 1; i < repetitions; i++)
    {
      string currentSegment = numberStr.Substring(i * patternLength, patternLength);
      if (currentSegment == pattern)
        continue;

      isRepeated = false;
      break;
    }

    return isRepeated;
  }
}
