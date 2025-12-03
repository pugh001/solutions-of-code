using System.Text.RegularExpressions;
using Utility;

namespace AOC2024;

public class Day14
{
  private static int _xMax = 101;
  private static int _yMax = 103;
  private static readonly int[,] Grid = new int[_xMax, _yMax];
  private static readonly List<((int, int), (int, int))> robots = new();
  private static Dictionary<(int, int), int> robotTree = new();
  public (string, string) Process(string input)
  {
    if (input.Contains("Example"))
    {
      _xMax = 11;
      _yMax = 7;
    }

    Array.Clear(Grid, 0, Grid.Length);
    long result1 = 0, result2 = 0;
    var data = SetupInputFile.OpenFile(input);
    var regex = new Regex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)");

    foreach (string line in data)
    {
      var match = regex.Match(line);
      if (match.Success)
      {
        int p1 = int.Parse(match.Groups[1].Value);
        int p2 = int.Parse(match.Groups[2].Value);
        int v1 = int.Parse(match.Groups[3].Value);
        int v2 = int.Parse(match.Groups[4].Value);
        robots.Add(new ValueTuple<(int, int), (int, int)>(new ValueTuple<int, int>(p1, p2), new ValueTuple<int, int>(v1, v2)));
      }
    }

    if (!input.Contains("Example"))
    {
      result2 = processPart2();
      return ("", result2.ToString());
    }

    result1 = processPart1(data);


    return (result1.ToString(), result2.ToString());
  }

  private static long processPart1(IEnumerable<string> data)
  {
    var regex = new Regex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)");

    foreach (string line in data)
    {
      var match = regex.Match(line);
      if (match.Success)
      {

        long x = (long.Parse(match.Groups[1].Value) + long.Parse(match.Groups[3].Value) * 100) % _xMax;
        long y = (long.Parse(match.Groups[2].Value) + long.Parse(match.Groups[4].Value) * 100) % _yMax;
        if (x < 0) x += _xMax;
        if (y < 0) y += _yMax;
        Grid[x, y]++;
      }
    }


    int Qx = _xMax / 2, Qy = _yMax / 2;
    int quad1 = 0, quad2 = 0, quad3 = 0, quad4 = 0;

    for (int i = 0; i < _xMax; i++)
    {
      if (i == Qx) continue;

      for (int j = 0; j < _yMax; j++)
      {
        if (j == Qy) continue;

        if (i < Qx && j < Qy) quad1 += Grid[i, j];
        else if (i > Qx && j < Qy) quad2 += Grid[i, j];
        else if (i < Qx && j > Qy) quad3 += Grid[i, j];
        else if (i > Qx && j > Qy) quad4 += Grid[i, j];
      }
    }


    return quad1 * quad2 * quad3 * quad4;

  }

  private static long processPart2()
  {
    long sum = 0;
    int totalRobots = robots.Count;
    int width = 101;
    int height = 103;
    bool overlap = true;
    while (overlap)
    {
      robotTree = new Dictionary<(int, int), int>();
      sum++;
      foreach (var robot in robots)
      {
        long x = robot.Item1.Item1 + robot.Item2.Item1 * sum;
        long y = robot.Item1.Item2 + robot.Item2.Item2 * sum;
        long newx = x % width < 0 ?
          width + x % width :
          x % width;
        long newy = y % height < 0 ?
          height + y % height :
          y % height;

        if (!robotTree.TryAdd((Convert.ToInt32(Math.Abs(newx)), Convert.ToInt32(Math.Abs(newy))), 1))
        {
          break;
        }
      }

      if (robotTree.Count == totalRobots)
      {
        overlap = false;
      }
    }

    return sum;
  }
}