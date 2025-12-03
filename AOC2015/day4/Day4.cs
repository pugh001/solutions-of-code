using System.Security.Cryptography;
using System.Text;
using Utility;

namespace AOC2015;

public class Day4
{

  public (string, string) Process(string input)
  {
    string data = SetupInputFile.OpenFile(input).First();

    return (TryAHash(data, 5), TryAHash(data, 6));

  }
  private static string TryAHash(string secretKey, int leadingZeroes)
  {
    // Target prefix (e.g., "00000" for 5 leading zeroes)
    string targetPrefix = new('0', leadingZeroes);
    int number = 1;
    var memoization = new Dictionary<string, string>();

    using var md5 = MD5.Create();
    while (true)
    {
      string input = secretKey + number;

      // Use memoization to avoid redundant hash calculations
      if (!memoization.TryGetValue(input, out string hash))
      {
        // Compute hash
        byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        memoization[input] = hash;
      }

      // Check if hash starts with the target prefix
      if (hash.StartsWith(targetPrefix))
      {
        return number.ToString();
      }

      number++;
    }

  }
}