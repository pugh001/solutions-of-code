using System.Text;
using Utility;

namespace AOC2015;

public class Day11
{

  private string _sumPart1 = "";
  private string _sumPart2 = "";
  public (string, string) Process(string input)
  {

    string? data = SetupInputFile.OpenFile(input).First();
    _sumPart1 = RunPassword(data);
    _sumPart2 = RunPassword(_sumPart1);
    return (_sumPart1, _sumPart2);
  }

  private static string RunPassword(string password)
  {
    var disallowed = new HashSet<char> { 'i', 'o', 'l' };
    if (Letters.ContainsDisallowed(password, disallowed, out int from) && from < password.Length)
    {
      char[] chars = password.ToCharArray();
      for (int i = from + 1; i < chars.Length; i++)
      {
        chars[i] = 'z';
      }

      password = new string(chars);
    }

    bool isValid = false;
    while (!isValid)
    {
      password = Letters.AddLetterToString(password, disallowed);
      if (!Letters.DoesItContainStraight(password))
        continue;

      if (Letters.DoesItContainNoneOverlappingDifferentPairs(password))
      {
        isValid = true;
      }
    }

    return password;
  }
}