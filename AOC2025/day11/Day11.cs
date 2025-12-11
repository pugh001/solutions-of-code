using System.Collections.Concurrent;
using Utility;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2025;

public class Day11
{
  public (string, string) Process(string input)
  {
    int part1 = 0;
    long part2 = 0;
    var data = SetupInputFile.OpenFile(input);
    var graph = new Dictionary<string, List<string>>();

    foreach (var line in data)
    {
      //data = start : dest dest (multiple)
      //Load into dictionary
      string[] split1 = line.Split(':');
      var key = split1[0];
      var destinations = split1[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
      graph[key] = destinations;


    }

    // Use the optimized counting method from Graph utility
    part1 = Graph.CountPaths(graph, "you", "out");
    
    // Use the optimized method for counting paths through nodes containing specific patterns
    // This handles multiple instances of dac/fft nodes efficiently
    part2 = Graph.CountPathsOptimized(graph, "svr", "out", "dac", "fft");
    return (part1.ToString(), part2.ToString());
  }
}
