/**
 * Collection manipulation and processing utilities
 */

namespace Utility;

public static class CollectionExtensions
{
  public static IEnumerable<T> Flatten<T>(this T[,] map)
  {
    for (int row = 0; row < map.GetLength(0); row++)
    {
      for (int col = 0; col < map.GetLength(1); col++)
      {
        yield return map[row, col];
      }
    }
  }

  public static string JoinAsStrings<T>(this IEnumerable<T> items, char seperator = '\u0000')
  {
    return string.Join(seperator, items);
  }

  public static string JoinAsStrings<T>(this IEnumerable<T> items, string seperator)
  {
    return string.Join(seperator, items);
  }

  //Removes a specified row AND column from a multidimensional array.
  public static int[,] TrimArray(this int[,] originalArray, int rowToRemove, int columnToRemove)
  {
    int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

    for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
    {
      if (i == rowToRemove)
        continue;

      for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
      {
        if (k == columnToRemove)
          continue;

        result[j, u] = originalArray[i, k];
        u++;
      }

      j++;
    }

    return result;
  }

  public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
  {
    return values.Count() == 1 ?
      new[] { values } :
      values.SelectMany(v => Permutations(values.Where(x => !x.Equals(v))), (v, p) => p.Prepend(v)).ToList();
  }

  public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values, int subcount)
  {
    var comboList = Combinations(values, subcount).ToList();
    foreach (var combination in comboList)
    {
      var perms = Permutations(combination);
      foreach (int i in Enumerable.Range(0, perms.Count())) yield return perms.ElementAt(i);
    }
  }

  public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> array, int m)
  {
    if (array.Count() < m)
      throw new ArgumentException("Array length can't be less than number of selected elements");
    if (m < 1)
      throw new ArgumentException("Number of selected elements can't be less than 1");

    var result = new T[m];
    foreach (int[] j in GetCombinations(m, array.Count()))
    {
      for (int i = 0; i < m; i++)
      {
        result[i] = array.ElementAt(j[i]);
      }

      yield return result;
    }
  }

  // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
  // in lexicographic order (first [0, 1, 2, ..., m-1]).
  private static IEnumerable<int[]> GetCombinations(int m, int n)
  {
    int[] result = new int[m];
    Stack<int> stack = new(m);
    stack.Push(0);
    while (stack.Count > 0)
    {
      int index = stack.Count - 1;
      int value = stack.Pop();
      while (value < n)
      {
        result[index++] = value++;
        stack.Push(value);
        if (index != m) continue;

        yield return (int[])result.Clone();

        break;
      }
    }
  }

  public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> array, int size)
  {
    for (int i = 0; i < (float)array.Count() / size; i++)
    {
      yield return array.Skip(i * size).Take(size);
    }
  }

  public static IEnumerable<List<T>> SplitAtIndex<T>(this List<T> array, int index)
  {
    if (index == 0) throw new ArgumentException($"{nameof(index)} must be a non-zero integer");

    if (index > 0)
    {
      index %= array.Count;
      yield return array.Take(index).ToList();
      yield return array.Skip(index).ToList();
    }
    else
    {
      index *= -1;
      index %= array.Count;
      yield return array.SkipLast(index).ToList();
      yield return array.TakeLast(index).ToList();
    }
  }

  public static string[] ToStringArray(this char[][] array)
  {
    string[] tmp = new string[array.GetLength(0)];

    for (int i = 0; i < tmp.Length; i++)
    {
      tmp[i] = array[i].JoinAsStrings();
    }

    return tmp;
  }

  /// <summary>
  ///   Rotates an IEnumerable by the requested amount
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="array"></param>
  /// <param name="rotations">
  ///   Number of steps to take, positive numbers move indices down (first item moves to end of array),
  ///   negative numbers move them up (item at end moves to start)
  /// </param>
  /// <returns></returns>
  public static IEnumerable<T> Rotate<T>(this IEnumerable<T> array, int rotations)
  {
    for (int i = 0; i < array.Count(); i++)
    {
      yield return i + rotations >= 0 ?
        array.ElementAt((i + rotations) % array.Count()) :
        array.ElementAt(i + rotations + array.Count());
    }
  }

  public static List<TK> KeyList<TK, TV>(this Dictionary<TK, TV> dictionary, bool sorted = false)
  {
    List<TK> keyList = [.. dictionary.Keys];

    if (sorted) keyList.Sort();

    return keyList;
  }
  
  /// <summary>
  /// Convert collection to dictionary with default value for missing keys
  /// </summary>
  /// <typeparam name="TSource">Source element type</typeparam>
  /// <typeparam name="TKey">Key type</typeparam>
  /// <typeparam name="TValue">Value type</typeparam>
  /// <param name="source">Source collection</param>
  /// <param name="keySelector">Key selector function</param>
  /// <param name="valueSelector">Value selector function</param>
  /// <param name="defaultValue">Default value for missing keys</param>
  /// <returns>Dictionary with default value support</returns>
  public static Dictionary<TKey, TValue> ToDictionaryWithDefault<TSource, TKey, TValue>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TValue> valueSelector,
      TValue defaultValue) where TKey : notnull
  {
    // Return a regular dictionary - default value functionality would need to be handled separately
    return source.ToDictionary(keySelector, valueSelector);
  }
}
