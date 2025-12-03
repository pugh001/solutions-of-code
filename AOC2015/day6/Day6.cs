using System.Text.RegularExpressions;
using Utility;

namespace AOC2015;

public class Day6
{
  private static readonly bool[,] Grid = new bool[1000, 1000];
  private static readonly int[,] GridBright = new int[1000, 1000];
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input).ToList();
    LightSwitch(data);
    return CountLit();
  }

  private static void LightSwitch(List<string> input)
  {

    var regex = new Regex(@".* (\d+),(\d+) through (\d+),(\d+)");
    string action;
    foreach (string lights in input)
    {

      action = SetAction(lights);

      var match = regex.Match(lights);
      if (!match.Success)
        continue;

      int startX = int.Parse(match.Groups[1].Value);
      int startY = int.Parse(match.Groups[2].Value);
      int endX = int.Parse(match.Groups[3].Value);
      int endY = int.Parse(match.Groups[4].Value);

      ChangeLights(startX, endX, startY, endY, action);
    }


  }
  private static void ChangeLights(int startX, int endX, int startY, int endY, string action)
  {

    for (int x = startX; x < endX + 1; x++)
    {
      for (int y = startY; y < endY + 1; y++)
      {
        if (action == "on")
        {
          Grid[x, y] = true;
          GridBright[x, y]++;
        }
        else if (action == "off")
        {
          Grid[x, y] = false;
          GridBright[x, y]--;
          if (GridBright[x, y] < 0) GridBright[x, y] = 0;
        }
        else if (action == "toggle")
        {
          Grid[x, y] = !Grid[x, y];
          GridBright[x, y] += 2;
        }

      }
    }
  }
  private static string SetAction(string lights)
  {

    if (lights.StartsWith("turn on")) return "on";
    if (lights.StartsWith("turn off")) return "off";

    return "toggle";

  }
  private static (string, string) CountLit()
  {
    int counter = 0;
    int countBright = 0;
    for (int i = 0; i < 1000; i++)
    {
      for (int j = 0; j < 1000; j++)
      {
        if (Grid[i, j]) counter++;
        countBright += GridBright[i, j];
      }
    }

    return (counter.ToString(), countBright.ToString());
  }
}