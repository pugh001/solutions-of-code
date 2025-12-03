namespace Utility.Algorithms;

/// <summary>
///   Common mathematical and algorithmic utilities for Advent of Code problems
/// </summary>
public static class Algorithms
{
  /// <summary>
  ///   Calculates the Greatest Common Divisor of two numbers
  /// </summary>
  public static long GCD(long a, long b)
  {
    while (b != 0)
    {
      long temp = b;
      b = a % b;
      a = temp;
    }

    return Math.Abs(a);
  }

  /// <summary>
  ///   Calculates the Least Common Multiple of two numbers
  /// </summary>
  public static long LCM(long a, long b)
  {
    return Math.Abs(a * b) / GCD(a, b);
  }

  /// <summary>
  ///   Calculates the LCM of multiple numbers
  /// </summary>
  public static long LCM(params long[] numbers)
  {
    return numbers.Aggregate(LCM);
  }

  /// <summary>
  ///   Calculates the LCM of multiple numbers
  /// </summary>
  public static long LCM(IEnumerable<long> numbers)
  {
    return numbers.Aggregate(LCM);
  }

  /// <summary>
  ///   Generates all permutations of a collection
  /// </summary>
  public static IEnumerable<IList<T>> GetPermutations<T>(IList<T> list)
  {
    if (list.Count == 0)
    {
      yield return new List<T>();

      yield break;
    }

    for (int i = 0; i < list.Count; i++)
    {
      var rest = list.Where((_, index) => index != i).ToList();
      foreach (var permutation in GetPermutations(rest))
      {
        yield return new[] { list[i] }.Concat(permutation).ToList();
      }
    }
  }

  /// <summary>
  ///   Generates all combinations of k elements from a collection
  /// </summary>
  public static IEnumerable<IList<T>> GetCombinations<T>(IList<T> list, int k)
  {
    if (k == 0)
    {
      yield return new List<T>();

      yield break;
    }

    if (k > list.Count)
      yield break;

    for (int i = 0; i < list.Count; i++)
    {
      var rest = list.Skip(i + 1).ToList();
      foreach (var combination in GetCombinations(rest, k - 1))
      {
        yield return new[] { list[i] }.Concat(combination).ToList();
      }
    }
  }

  /// <summary>
  ///   Calculates the Manhattan distance between two points
  /// </summary>
  public static int ManhattanDistance(int x1, int y1, int x2, int y2)
  {
    return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
  }

  /// <summary>
  ///   Calculates the Euclidean distance between two points
  /// </summary>
  public static double EuclideanDistance(double x1, double y1, double x2, double y2)
  {
    return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
  }

  /// <summary>
  ///   Checks if a number is prime
  /// </summary>
  public static bool IsPrime(long n)
  {
    if (n < 2) return false;
    if (n == 2) return true;
    if (n % 2 == 0) return false;

    long limit = (long)Math.Sqrt(n);
    for (long i = 3; i <= limit; i += 2)
    {
      if (n % i == 0) return false;
    }

    return true;
  }

  /// <summary>
  ///   Generates prime numbers up to a given limit using the Sieve of Eratosthenes
  /// </summary>
  public static List<int> SieveOfEratosthenes(int limit)
  {
    bool[] isPrime = new bool[limit + 1];
    Array.Fill(isPrime, true);
    isPrime[0] = isPrime[1] = false;

    for (int i = 2; i * i <= limit; i++)
    {
      if (isPrime[i])
      {
        for (int j = i * i; j <= limit; j += i)
        {
          isPrime[j] = false;
        }
      }
    }

    var primes = new List<int>();
    for (int i = 2; i <= limit; i++)
    {
      if (isPrime[i]) primes.Add(i);
    }

    return primes;
  }

  /// <summary>
  ///   Calculates modular exponentiation (base^exp % mod)
  /// </summary>
  public static long ModPow(long baseValue, long exponent, long modulus)
  {
    long result = 1;
    baseValue %= modulus;

    while (exponent > 0)
    {
      if (exponent % 2 == 1)
        result = result * baseValue % modulus;

      exponent >>= 1;
      baseValue = baseValue * baseValue % modulus;
    }

    return result;
  }

  /// <summary>
  ///   Finds the modular inverse using extended Euclidean algorithm
  /// </summary>
  public static long ModInverse(long a, long m)
  {
    if (GCD(a, m) != 1) throw new ArgumentException("Modular inverse doesn't exist");

    long m0 = m, x0 = 0, x1 = 1;

    while (a > 1)
    {
      long q = a / m;
      (m, a) = (a % m, m);
      (x0, x1) = (x1 - q * x0, x0);
    }

    return x1 < 0 ?
      x1 + m0 :
      x1;
  }

  /// <summary>
  ///   Chinese Remainder Theorem solver
  /// </summary>
  public static long ChineseRemainderTheorem(long[] remainders, long[] moduli)
  {
    if (remainders.Length != moduli.Length)
      throw new ArgumentException("Arrays must have the same length");

    long product = moduli.Aggregate(1L, (acc, m) => acc * m);
    long result = 0;

    for (int i = 0; i < remainders.Length; i++)
    {
      long partialProduct = product / moduli[i];
      long inverse = ModInverse(partialProduct, moduli[i]);
      result += remainders[i] * partialProduct * inverse;
    }

    return (result % product + product) % product;
  }

  /// <summary>
  ///   Calculates factorial
  /// </summary>
  public static long Factorial(int n)
  {
    if (n < 0) throw new ArgumentException("Factorial is not defined for negative numbers");

    if (n <= 1) return 1;

    long result = 1;
    for (int i = 2; i <= n; i++)
    {
      result *= i;
    }

    return result;
  }

  /// <summary>
  ///   Calculates binomial coefficient (n choose k)
  /// </summary>
  public static long BinomialCoefficient(int n, int k)
  {
    if (k > n || k < 0) return 0;
    if (k == 0 || k == n) return 1;

    // Use symmetry to reduce calculations
    if (k > n - k) k = n - k;

    long result = 1;
    for (int i = 0; i < k; i++)
    {
      result = result * (n - i) / (i + 1);
    }

    return result;
  }

  /// <summary>
  ///   Rotates an array to the left by k positions
  /// </summary>
  public static T[] RotateLeft<T>(T[] array, int k)
  {
    if (array.Length == 0) return array;

    k = k % array.Length;
    if (k == 0) return array;

    var result = new T[array.Length];
    for (int i = 0; i < array.Length; i++)
    {
      result[i] = array[(i + k) % array.Length];
    }

    return result;
  }

  /// <summary>
  ///   Rotates an array to the right by k positions
  /// </summary>
  public static T[] RotateRight<T>(T[] array, int k)
  {
    return RotateLeft(array, array.Length - k % array.Length);
  }

  /// <summary>
  ///   Finds all divisors of a number
  /// </summary>
  public static List<long> GetDivisors(long n)
  {
    var divisors = new List<long>();
    for (long i = 1; i * i <= n; i++)
    {
      if (n % i == 0)
      {
        divisors.Add(i);
        if (i != n / i)
          divisors.Add(n / i);
      }
    }

    return divisors.OrderBy(x => x).ToList();
  }
}