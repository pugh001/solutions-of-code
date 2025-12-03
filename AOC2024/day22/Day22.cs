using Utility;

namespace AOC2024;

public class Day22
{


  public (string, string) Process(string input)
  {
    int[] data = SetupInputFile.OpenFile(input).Select(int.Parse).ToArray();


    var bigNumbers = new Dictionary<int, List<int>>();
    var lastDigits = new Dictionary<int, List<int>>();
    var diff = new Dictionary<int, List<int>>();
    var sequences = new Dictionary<int, Dictionary<int, int>>();


    int result1 = 0;
    foreach (int line in data)
    {
      int number = line;
      int previousLastDigit = number % 10;
      bigNumbers[number] = [];
      lastDigits[number] = [];
      diff[number] = [];
      sequences[number] = new Dictionary<int, int>();

      for (int i = 0; i < 2000; i++)
      {
        number = (int)CalculateSecretKey(number);
        bigNumbers[line].Add(number);
        int lastDigit = number % 10;
        lastDigits[line].Add(lastDigit);
        diff[line].Add(lastDigit - previousLastDigit);
        previousLastDigit = lastDigit;

      }

      result1 += number;
      var firstOccurences = new HashSet<int>();
      for (int i = 4; i <= 2000; i++)
      {

        int value = lastDigits[line][i - 1];
        int hash = GetSequenceHash(diff[line].GetRange(i - 4, 4));

        if (firstOccurences.Contains(hash)) { continue; }

        firstOccurences.Add(hash);
        if (sequences[line].ContainsKey(hash)) { continue; }

        sequences[line][hash] = value;
      }


    }

    if (sequences.Select(item => item.Value.Select(x => x.Key).ToList()).Any(qq => qq.Count != qq.Distinct().Count()))
    {
      throw new Exception();
    }

    // **Precompute banana counts**
    var bananaCounts = AggregateBananaCounts(sequences);

    var countOfBananaSequence = bananaCounts.OrderByDescending(x => x.Value).ToList();

    long result = countOfBananaSequence.Max(x => x.Value);


    return ($"{result1}", $"{result}");

  }

  private static Dictionary<int, int> AggregateBananaCounts(Dictionary<int, Dictionary<int, int>> sequences)
  {
    var bananaCounts = new Dictionary<int, int>();
    foreach (var seqDict in sequences.Values)
    {
      foreach ((int key, int value) in seqDict)
      {
        if (bananaCounts.ContainsKey(key))
          bananaCounts[key] += value;
        else
          bananaCounts[key] = value;
      }
    }

    return bananaCounts;
  }


  private static long CalculateSecretKey(long secretKey)
  {
    long step1 = Prune(Mix(secretKey, Multiply(secretKey, 64)));
    long step2 = Prune(Mix(step1, Divide32(step1)));
    secretKey = Prune(Mix(step2, Multiply(step2, 2048)));
    return secretKey;
  }

  private static long Divide32(long value)
  {
    return value / 32;
  }

  private static long Prune(long value)
  {
    return value % 16777216;
  }

  private static long Mix(long startNumber, long value)
  {
    return startNumber ^ value;
  }

  private static long Multiply(long initial, long value)
  {
    return initial * value;
  }
  private static int GetSequenceHash(List<int> sequence)
  {
    unchecked
    {
      int hash = 17;
      foreach (int num in sequence)
      {
        hash = hash * 31 + num;
      }

      return hash;
    }
  }
}