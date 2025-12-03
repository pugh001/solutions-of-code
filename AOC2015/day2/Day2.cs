using System.Text.RegularExpressions;
using Utility;

namespace AOC2015;

public class Day2
{
  public (string, string) Process(string input)
  {
    int result1 = 0;
    int result2 = 0;
    var data = SetupInputFile.OpenFile(input);
    var regex = new Regex(@"(\d+)x(\d+)x(\d+)");
    foreach (string present in data)
    {
      var match = regex.Match(present);
      if (match.Success)
      {
        int l = int.Parse(match.Groups[1].Value);
        int w = int.Parse(match.Groups[2].Value);
        int h = int.Parse(match.Groups[3].Value);
        int side1 = l * w;
        int side2 = w * h;
        int side3 = h * l;
        int minSide = Math.Min(side1, Math.Min(side2, side3));
        result1 += side1 * 2 + side2 * 2 + side3 * 2 + minSide;
        int smallestPar = Math.Min(2 * (h + l), Math.Min(2 * (h + w), 2 * (w + l)));
        int cubic = h * l * w;
        result2 += smallestPar + cubic;
      }
    }

    return (result1.ToString(), result2.ToString());
  }
}