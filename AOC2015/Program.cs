using System.Diagnostics;
using Utility;

namespace AOC2015;

static internal class Program
{
  public static void Main(string[] args)
  {
    for (int day = 25; day > 0; day--)
    {
      try
      {
        (bool exists, string inputFilePath) = TestFiles.GetInputData(day, 2015, "puzzleInput.txt");


        // Create an instance of the Day## class dynamically
        object? dayInstance =
          Activator.CreateInstance(Type.GetType($"AOC2015.Day{day}") ?? throw new InvalidOperationException());
        if (!exists) //Class exists, so check if input exists
        {
          Console.WriteLine("Day " + day.ToString().PadLeft(2, ' ') + ": NO PUZZLE INPUT");
          continue;
        }

        if (dayInstance == null)
        {
          Console.WriteLine($"Day{day} class could not be instantiated.");
          continue;
        }

        Console.WriteLine("");
        Console.Write("Day " + day.ToString().PadLeft(2, ' ') + ":");

        // Retrieve the Process method dynamically
        var processMethod = dayInstance.GetType().GetMethod("Process");
        if (processMethod == null)
        {
          Console.WriteLine($"Process method not found in Day{day}.");
          continue;
        }

        var stopWatch = Stopwatch.StartNew();

        // Invoke the Process method on the instance
        object? result = processMethod.Invoke(dayInstance, [inputFilePath]);

        stopWatch.Stop();

        // Display the result of the Process method
        Console.Write(result);
        Console.Write($"  Time:  {stopWatch.Elapsed}");
      }
      catch (InvalidOperationException)
      {
        // No Day# code yet
        Console.Write(".");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
        break;
      }
    }
  }
}