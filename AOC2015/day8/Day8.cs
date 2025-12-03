using System.Text.RegularExpressions;
using Utility;

namespace AOC2015;

public class Day8
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input).ToList();

    int literal, unescape, encoded, result2 = 0, result = 0;
    string unescapedStr;
    foreach (string code in data)
    {
      literal = code.Length;
      unescapedStr = Regex.Unescape(code);
      unescape = unescapedStr.Length - 2;
      encoded = literal + code.Count(bs => bs == '\\') + code.Count(bs => bs == '"') + 2;
      result2 += encoded - literal;
      result += literal - unescape;

    }

    return (result.ToString(), result2.ToString());
  }
}