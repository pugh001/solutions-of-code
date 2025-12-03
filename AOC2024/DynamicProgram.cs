using System.Diagnostics;
using Utility;

namespace AOC2024;

static internal class DynamicProgram
{
  public static void Main(string[] args)
  {
    for (int day = 26; day > 0; day--)
    {
      try
      {
        (bool exists, string inputFilePath) = TestFiles.GetInputData(day, 2024, "part1Example.txt");
        if (!exists)
        {
          throw new InvalidOperationException($"Type AOC2024.Day{day} not found.");
        }

        object? dayInstance =
          Activator.CreateInstance(Type.GetType($"AOC2024.Day{day}") ?? throw new InvalidOperationException());
        Console.WriteLine("");
        Console.Write("Day " + day.ToString().PadLeft(2, ' ') + ":");
        var stopWatch = Stopwatch.StartNew();
        Console.Write(dayInstance.GetType().GetMethod("Process").Invoke(dayInstance, new object[] { inputFilePath }));
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
        break;
      }
    }
  }
}