/**
 * Mathematical utilities for calculations and algorithms
 */

using System.Numerics;

namespace Utility;

public static class MathUtilities
{
  public static int ManhattanDistance(this (int x, int y) a, (int x, int y) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
  }

  public static int ManhattanDistance(this (int x, int y, int z) a, (int x, int y, int z) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
  }

  public static long ManhattanDistance(this (long x, long y) a, (long x, long y) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
  }

  public static long ManhattanDistance(this (long x, long y, long z) a, (long x, long y, long z) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
  }

  public static long ManhattanMagnitude(this (long x, long y, long z) a)
  {
    return a.ManhattanDistance((0, 0, 0));
  }

  public static double FindGcd(double a, double b)
  {
    if (a == 0 || b == 0) return Math.Max(a, b);

    return a % b == 0 ?
      b :
      FindGcd(b, a % b);
  }

  public static double FindLcm(double a, double b)
  {
    return a * b / FindGcd(a, b);
  }

  public static long FindGcd(long a, long b)
  {
    if (a == 0 || b == 0) return Math.Max(a, b);

    return a % b == 0 ?
      b :
      FindGcd(b, a % b);
  }

  public static long FindLcm(long a, long b)
  {
    return a * b / FindGcd(a, b);
  }

  public static (long gcd, long x, long y) ExtendedGcd(long a, long b)
  {
    if (b == 0) return (a, 1, 0);

    (long gcd0, long x0, long y0) = ExtendedGcd(b, b % a);
    return (gcd0, y0, x0 - a / b * y0);
  }

  public static int Mod(int x, int m)
  {
    int r = x % m;
    return r < 0 ?
      r + m :
      r;
  }

  public static long Mod(long x, long m)
  {
    long r = x % m;
    return r < 0 ?
      r + m :
      r;
  }

  public static long ModInverse(long a, long n)
  {
    return ModPower(a, n - 2, n);
  }

  public static long ModPower(long x, long y, long p)
  {
    return (long)BigInteger.ModPow(x, y, p);
  }

  // Generate numbers where left half equals right half
  public static IEnumerable<long> GeneratePalindromicHalfNumbers(long start, long end)
  {
    // Determine the range of digit lengths to consider
    int startDigits = start.ToString().Length;
    int endDigits = end.ToString().Length;

    for (int digits = startDigits; digits <= endDigits; digits++)
    {
      // Only consider even-digit numbers
      if (digits % 2 != 0) continue;

      int halfDigits = digits / 2;
      long minHalf = (long)Math.Pow(10, halfDigits - 1);
      long maxHalf = (long)Math.Pow(10, halfDigits) - 1;

      // Adjust bounds based on the actual range
      minHalf = MinMaxHalf(start, digits, startDigits, halfDigits, minHalf, "max");
      maxHalf = MinMaxHalf(end, digits, endDigits, halfDigits, maxHalf, "min");

      foreach (long l in GetNumber(start, end, minHalf, maxHalf))
        yield return l;
    }
  }

  private static IEnumerable<long> GetNumber(long start, long end, long minHalf, long maxHalf)
  {
    for (long half = minHalf; half <= maxHalf; half++)
    {
      // Create the full number by duplicating the half
      string halfStr = half.ToString();
      long fullNumber = long.Parse(halfStr + halfStr);

      if (fullNumber >= start && fullNumber <= end)
      {
        yield return fullNumber;
      }
    }
  }

  private static long MinMaxHalf(long endOrstart, int digits, int startDigits, int halfDigits, long minHalf, string option)
  {
    if (digits != startDigits)
      return minHalf;

    string startStr = endOrstart.ToString();
    if (startStr.Length != digits)
      return minHalf;

    long startHalf = long.Parse(startStr[..halfDigits]);
    return option == "max" ?
      Math.Max(minHalf, startHalf) :
      Math.Min(minHalf, startHalf);
  }

  /// <summary>
  ///   Finds the highest (numerically largest) subsequence in a collection that meets specified criteria
  ///   If multiple subsequences of the same length meet criteria, returns the one with highest numeric value
  /// </summary>
  /// <typeparam name="T">Type of elements in the collection</typeparam>
  /// <param name="collection">The collection to analyze</param>
  /// <param name="predicate">Function to determine if elements form valid subsequence</param>
  /// <returns>The highest/longest valid subsequence</returns>
  public static IEnumerable<T> GetHighestSubsequence<T>(IEnumerable<T> collection, Func<IEnumerable<T>, bool> predicate)
  {
    var items = collection.ToList();
    var bestSubsequence = Enumerable.Empty<T>();
    long bestNumericValue = long.MinValue;

    // Try all possible subsequences, starting with longest
    for (int length = items.Count; length >= 1; length--)
    {
      bool foundValidSequenceThisLength = false;
      var bestForThisLength = Enumerable.Empty<T>();
      long bestNumericForThisLength = long.MinValue;

      for (int start = 0; start <= items.Count - length; start++)
      {
        var subsequence = items.Skip(start).Take(length);
        if (predicate(subsequence))
        {
          foundValidSequenceThisLength = true;

          // Compare all subsequences of the same length to find the numerically highest
          if (typeof(T) == typeof(char))
          {
            string numericString = string.Concat(subsequence.Cast<char>());
            if (long.TryParse(numericString, out long numericValue))
            {
              if (!bestForThisLength.Any() || numericValue > bestNumericForThisLength)
              {
                bestForThisLength = subsequence;
                bestNumericForThisLength = numericValue;
              }
            }
          }
          else
          {
            // For non-char types, just take the first valid one of this length
            if (!bestForThisLength.Any())
            {
              bestForThisLength = subsequence;
            }
          }
        }
      }

      // If we found valid subsequences of this length, use the best one and stop (prioritize longest first)
      if (foundValidSequenceThisLength)
      {
        bestSubsequence = bestForThisLength;
        break;
      }
    }

    return bestSubsequence;
  }

  /// <summary>
  ///   Checks if a sequence contains any repeated contiguous subsequences
  /// </summary>
  /// <typeparam name="T">Type of elements in the sequence</typeparam>
  /// <param name="sequence">The sequence to check</param>
  /// <param name="minLength">Minimum length of subsequence to consider</param>
  /// <returns>True if sequence contains repeated subsequences, false otherwise</returns>
  public static bool HasRepeatedSequence<T>(IEnumerable<T> sequence, int minLength = 2) where T : IEquatable<T>
  {
    var items = sequence.ToList();

    // Check for repeated subsequences of various lengths
    for (int length = minLength; length <= items.Count / 2; length++)
    {
      for (int start = 0; start <= items.Count - length * 2; start++)
      {
        var firstSubsequence = items.Skip(start).Take(length);

        // Check if this subsequence appears again later in the sequence
        for (int nextStart = start + length; nextStart <= items.Count - length; nextStart++)
        {
          var secondSubsequence = items.Skip(nextStart).Take(length);

          if (firstSubsequence.SequenceEqual(secondSubsequence))
          {
            return true;
          }
        }
      }
    }

    return false;
  }
}