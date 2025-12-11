using System.Text.RegularExpressions;
using Utility;

namespace AOC2015;

public class Day8
{
  public (string, string) Process(string input)
  {
    var lines = SetupInputFile.OpenFile(input);
    var data = new List<string>();
    foreach (string line in lines)
    {
      data.Add(line);
    }

    int literal, unescape, encoded, result2 = 0, result = 0;
    string unescapedStr;
    foreach (string code in data)
    {
      literal = code.Length;
      unescapedStr = Regex.Unescape(code);
      unescape = unescapedStr.Length - 2;

      int backslashCount = 0;
      int quoteCount = 0;
      foreach (char c in code)
      {
        if (c == '\\') backslashCount++;
        if (c == '"') quoteCount++;
      }

      encoded = literal + backslashCount + quoteCount + 2;
      result2 += encoded - literal;
      result += literal - unescape;

    }

    return (result.ToString(), result2.ToString());
  }
}