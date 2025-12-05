using Utility;
using Utility.Algorithms;

namespace AOC2015;

public class Day9
{
  private long _sumPart1;
  private long _sumPart2;
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);

    // Parse the distances into a dictionary
    var distances = new Dictionary<(string, string), int>();
    var cities = new HashSet<string>();

    foreach (string line in data)
    {
      // Parse lines like "London to Dublin = 464"
      string city1 = SplitValues(line, out string city2, out int distance);

      // Add both directions since distance is symmetric
      distances[(city1, city2)] = distance;
      distances[(city2, city1)] = distance;

      cities.Add(city1);
      cities.Add(city2);
    }

    var cityList = cities.ToList();
    var allPermutations = Algorithms.GetPermutations(cityList);

    var allDistances = new List<int>();

    foreach (var permutation in allPermutations)
    {
      int totalDistance = 0;
      bool validPath = true;

      // Calculate total distance for this route
      for (int i = 0; i < permutation.Count - 1; i++)
      {
        var key = (permutation[i], permutation[i + 1]);
        if (distances.ContainsKey(key))
        {
          totalDistance += distances[key];
        }
        else
        {
          validPath = false;
          break;
        }
      }

      if (validPath)
      {
        allDistances.Add(totalDistance);
      }
    }

    _sumPart1 = allDistances.Min(); // Shortest distance
    _sumPart2 = allDistances.Max(); // Longest distance

    return (_sumPart1.ToString(), _sumPart2.ToString());
  }
  private static string SplitValues(string line, out string city2, out int distance)
  {

    string[] parts = line.Split(" to ");
    string city1 = parts[0];
    string[] remaining = parts[1].Split(" = ");
    city2 = remaining[0];
    distance = int.Parse(remaining[1]);
    return city1;
  }
}