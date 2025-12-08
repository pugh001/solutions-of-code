using System.Diagnostics;
using Utility;

namespace AOC2025;

static internal class Program
{
  public static void Main(string[] args)
  {
    for (int day = 26; day > 0; day--)
    {
      try
      {
        (bool exists, string inputFilePath) = TestFiles.GetInputData(day, 2025, "puzzleInput.txt");
        object? dayInstance =
          Activator.CreateInstance(Type.GetType($"AOC2025.Day{day}") ?? throw new InvalidOperationException());
        if (!exists) //Class exists, so check if input exists
        {
          Console.WriteLine("Day " + day.ToString().PadLeft(2, ' ') + ": NO PUZZLE INPUT");
          continue;
        }

        Console.WriteLine("");
        Console.Write("Day " + day.ToString().PadLeft(2, ' ') + ":");
        var stopWatch = Stopwatch.StartNew();
        Console.Write(dayInstance.GetType().GetMethod("Process").Invoke(dayInstance, [inputFilePath]));
        stopWatch.Stop();
        Console.Write($"  Time:  {stopWatch.Elapsed}");
      }
      catch (InvalidOperationException)
      {
        //No Day# code yet
        Console.Write("");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");

      }
    }
  }
}
