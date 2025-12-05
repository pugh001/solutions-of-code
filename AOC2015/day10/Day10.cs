using System.Text;
using Utility;

namespace AOC2015;

public class Day10
{
  private long _sumPart1;
  private long _sumPart2;
  public (string, string) Process(string input)
  {
    string? data = SetupInputFile.OpenFile(input).First();


    _sumPart1 = RunprocessWithOption(data, 40);
    _sumPart2 = RunprocessWithOption(data, 50);

    return (_sumPart1.ToString(), _sumPart2.ToString());
  }
  private static long RunprocessWithOption(string data, int loops)
  {

    for (int i = 0; i < loops; i++)
    {
      char current = data[0];
      int counter = 1;
      var next = new StringBuilder();

      for (int j = 1; j < data.Length; j++)
      {
        if (data[j] == current)
        {
          counter++;
          continue;
        }

        next.Append(counter);
        next.Append(current);
        current = data[j];
        counter = 1;

      }

      // Don't forget the last group
      next.Append(counter);
      next.Append(current);
      data = next.ToString();
    }

    return data.Length;
  }
}