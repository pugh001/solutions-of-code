/**
 * String conversion utilities for parsing and extracting data from strings
 */

using System.Text.RegularExpressions;

namespace Utility;

public static class StringConversions
{
  /// <summary>
  ///   Turns a string into a list of ints.
  /// </summary>
  /// <param name="str">The string to split up</param>
  /// <param name="delimiter">How to split. Default is each character becomes an int.</param>
  /// <returns>A list of integers</returns>
  public static List<int> ToIntList(this string str, string delimiter = "")
  {
    if (delimiter == "")
    {
      List<int> result = [];
      foreach (char c in str)
        if (int.TryParse(c.ToString(), out int n))
          result.Add(n);
      return result;
    }

    return str.Split(delimiter).Where(n => int.TryParse(n, out int _)).Select(n => Convert.ToInt32(n)).ToList();
  }

  /// <summary>
  ///   Turns a string array into an array of int.
  ///   Utilizes ToIntList
  /// </summary>
  /// <param name="array">array of strings to parse</param>
  /// <returns>An array of integers</returns>
  public static int[] ToIntArray(this string[] array)
  {
    return string.Join(",", array).ToIntList(",").ToArray();
  }

  /// <summary>
  ///   Extract all the positive integers from a string, automatically deliminates on all non numeric chars
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the integers found in the string.</returns>
  public static IEnumerable<int> ExtractPosInts(this string str)
  {
    return Regex.Matches(str, "\\d+").Select(m => int.Parse(m.Value));
  }

  /// <summary>
  ///   Extracts all ints from a string, treats `-` as a negative sign.
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the integers found in the string.</returns>
  public static IEnumerable<int> ExtractInts(this string str)
  {
    return Regex.Matches(str, "-?\\d+").Select(m => int.Parse(m.Value));
  }

  /// <summary>
  ///   Extracts all longs from a string, treats `-` as a negative sign.
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the longs found in the string.</returns>
  public static IEnumerable<long> ExtractLongs(this string str)
  {
    return Regex.Matches(str, "-?\\d+").Select(m => long.Parse(m.Value));
  }

  /// <summary>
  ///   Extract all the positive longs from a string, automatically deliminates on all non numeric chars
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the longs found in the string.</returns>
  public static IEnumerable<long> ExtractPosLongs(this string str)
  {
    return Regex.Matches(str, "\\d+").Select(m => long.Parse(m.Value));
  }

  /// <summary>
  ///   Extracts all "Words" (including xnoppyt) from a string
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static IEnumerable<string> ExtractWords(this string str)
  {
    return Regex.Matches(str, "[a-zA-z]+").Select(a => a.Value);
  }

  /// <summary>
  ///   Turns a string into a list of longs.
  /// </summary>
  /// <param name="str">The string to split up</param>
  /// <param name="delimiter">How to split. Default is each character becomes a long.</param>
  /// <returns>A list of long</returns>
  public static List<long> ToLongList(this string str, string delimiter = "")
  {
    if (delimiter == "")
    {
      List<long> result = [];
      foreach (char c in str)
        if (long.TryParse(c.ToString(), out long n))
          result.Add(n);
      return result.ToList();
    }

    return str.Split(delimiter).Where(n => long.TryParse(n, out long _)).Select(n => Convert.ToInt64(n)).ToList();
  }

  public static string HexStringToBinary(this string hexstring)
  {
    return string.Join(string.Empty,
      hexstring.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
  }
}
