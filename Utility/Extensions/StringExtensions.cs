/**
 * String manipulation and processing utilities
 */

using System.Text;

namespace Utility;

public static class StringExtensions
{
  /// <summary>
  ///   Repeat text n times, returning a string with the repeated text.
  /// </summary>
  /// <param name="text">Text to Repeat.</param>
  /// <param name="n">Number of times to repeat.</param>
  /// <param name="seperator"></param>
  /// <returns>Repeated String.</returns>
  public static string Repeat(this string text, int n, string seperator = "")
  {
    return new StringBuilder((text.Length + seperator.Length) * n).Insert(0, $"{text}{seperator}", n).ToString();
  }

  /// <summary>
  ///   Joins enumerable to string with separator
  /// </summary>
  /// <typeparam name="T">Type of elements</typeparam>
  /// <param name="source">Source enumerable</param>
  /// <param name="separator">Separator string</param>
  /// <returns>Joined string</returns>
  public static string JoinToString<T>(this IEnumerable<T> source, string separator = "")
  {
    return string.Join(separator, source);
  }

  public static List<string> SplitByNewline(this string input, bool blankLines = false, bool shouldTrim = true)
  {
    return input.Split(["\r\n", "\r", "\n"], StringSplitOptions.None).Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
      .Select(s => shouldTrim ?
        s.Trim() :
        s).ToList();
  }

  public static List<string> SplitByDoubleNewline(this string input, bool blankLines = false, bool shouldTrim = true)
  {
    return input.Split(["\r\n\r\n", "\r\r", "\n\n"], StringSplitOptions.None)
      .Where(s => blankLines || !string.IsNullOrWhiteSpace(s)).Select(s => shouldTrim ?
        s.Trim() :
        s).ToList();
  }

  /// <summary>
  ///   Splits the input into columns, this is sometimes nice for maps drawing.
  ///   Automatically expands to a full rectangle if needed based on max length and number of rows.
  ///   Empty cells are denoted as ' ' (Space character)
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static string[] SplitIntoColumns(this string input)
  {
    var rows = input.SplitByNewline(false, false);
    int numColumns = rows.Max(x => x.Length);

    string[] res = new string[numColumns];
    for (int i = 0; i < numColumns; i++)
    {
      StringBuilder sb = new();
      foreach (string row in rows)
      {
        try
        {
          sb.Append(row[i]);
        }
        catch (IndexOutOfRangeException)
        {
          sb.Append(' ');
        }
      }

      res[i] = sb.ToString();
    }

    return res;
  }

  public static IEnumerable<int> AllIndexesOf(this string str, string value)
  {
    if (string.IsNullOrEmpty(value))
      throw new ArgumentException("the string to find may not be empty", nameof(value));

    for (int index = 0;; index += value.Length)
    {
      index = str.IndexOf(value, index);
      if (index == -1)
        break;

      yield return index;
    }
  }

  public static IEnumerable<string> Lines(this string str)
  {
    using var sr = new StringReader(str ?? string.Empty);

    while (true)
    {
      string? line = sr.ReadLine();
      if (line == null)
        yield break;

      yield return line;
    }
  }

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

  // Check if a number has repeated sequences (for part 2)
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