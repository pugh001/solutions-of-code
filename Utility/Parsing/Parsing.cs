using System.Text.RegularExpressions;

namespace Utility;

/// <summary>
///   Common parsing utilities for Advent of Code problems
/// </summary>
public static class Parsing
{
  /// <summary>
  ///   Extracts all integers from a string
  /// </summary>
  public static List<int> ExtractIntegers(string text)
  {
    var matches = Regex.Matches(text, @"-?\d+");
    var result = new List<int>(matches.Count);
    foreach (Match match in matches)
    {
      result.Add(int.Parse(match.Value));
    }
    return result;
  }

  /// <summary>
  ///   Extracts all long integers from a string
  /// </summary>
  public static List<long> ExtractLongs(string text)
  {
    var matches = Regex.Matches(text, @"-?\d+");
    var result = new List<long>(matches.Count);
    foreach (Match match in matches)
    {
      result.Add(long.Parse(match.Value));
    }
    return result;
  }

  /// <summary>
  ///   Extracts all numbers (including decimals) from a string
  /// </summary>
  public static List<double> ExtractNumbers(string text)
  {
    var matches = Regex.Matches(text, @"-?\d+\.?\d*");
    var result = new List<double>(matches.Count);
    foreach (Match match in matches)
    {
      result.Add(double.Parse(match.Value));
    }
    return result;
  }

  /// <summary>
  ///   Splits text into groups separated by empty lines
  /// </summary>
  public static List<List<string>> SplitByEmptyLines(string[] lines)
  {
    var groups = new List<List<string>>();
    var currentGroup = new List<string>();

    foreach (string line in lines)
    {
      if (string.IsNullOrWhiteSpace(line))
      {
        if (currentGroup.Count > 0)
        {
          groups.Add(currentGroup);
          currentGroup = new List<string>();
        }
      }
      else
      {
        currentGroup.Add(line);
      }
    }

    if (currentGroup.Count > 0)
      groups.Add(currentGroup);

    return groups;
  }

  /// <summary>
  ///   Splits text into groups separated by empty lines
  /// </summary>
  public static List<List<string>> SplitByEmptyLines(IEnumerable<string> lines)
  {
    return SplitByEmptyLines(lines.ToArray());
  }

  /// <summary>
  ///   Converts a binary string to a long integer
  /// </summary>
  public static long BinaryToLong(string binary)
  {
    return Convert.ToInt64(binary, 2);
  }

  /// <summary>
  ///   Converts a hexadecimal string to a long integer
  /// </summary>
  public static long HexToLong(string hex)
  {
    return Convert.ToInt64(hex, 16);
  }

  /// <summary>
  ///   Converts characters to their numeric values (A=10, B=11, etc.)
  /// </summary>
  public static int CharToDigit(char c, int baseValue = 10)
  {
    if (char.IsDigit(c))
      return c - '0';

    if (char.IsLetter(c))
    {
      int value = char.ToUpperInvariant(c) - 'A' + 10;
      return value < baseValue ?
        value :
        throw new ArgumentException($"Character '{c}' is not valid for base {baseValue}");
    }

    throw new ArgumentException($"Character '{c}' is not a digit or letter");
  }

  /// <summary>
  ///   Parses a Point2D from string format like "x,y" or "(x,y)"
  /// </summary>
  public static Point2D<int> ParsePoint(string pointStr)
  {
    string clean = pointStr.Trim('(', ')', ' ');
    string[] parts = clean.Split(',');
    if (parts.Length != 2)
      throw new ArgumentException($"Invalid point format: {pointStr}");

    return new Point2D<int>(int.Parse(parts[0].Trim()), int.Parse(parts[1].Trim()));
  }

  /// <summary>
  ///   Creates a frequency/count dictionary from an enumerable
  /// </summary>
  public static Dictionary<T, int> CountFrequencies<T>(IEnumerable<T> items) where T : notnull
  {
    var frequencies = new Dictionary<T, int>();
    foreach (var item in items)
    {
      frequencies[item] = frequencies.GetValueOrDefault(item, 0) + 1;
    }

    return frequencies;
  }

  /// <summary>
  ///   Transposes a 2D array of strings (useful for rotating grids)
  /// </summary>
  public static string[] Transpose(string[] lines)
  {
    if (lines.Length == 0) return lines;

    int width = lines[0].Length;
    string[] result = new string[width];

    for (int col = 0; col < width; col++)
    {
      char[] chars = new char[lines.Length];
      for (int row = 0; row < lines.Length; row++)
      {
        chars[row] = lines[row][col];
      }

      result[col] = new string(chars);
    }

    return result;
  }

  /// <summary>
  ///   Rotates a grid 90 degrees clockwise
  /// </summary>
  public static string[] RotateClockwise(string[] lines)
  {
    if (lines.Length == 0) return lines;

    int height = lines.Length;
    int width = lines[0].Length;
    string[] result = new string[width];

    for (int col = 0; col < width; col++)
    {
      char[] chars = new char[height];
      for (int row = 0; row < height; row++)
      {
        chars[row] = lines[height - 1 - row][col];
      }

      result[col] = new string(chars);
    }

    return result;
  }
}
