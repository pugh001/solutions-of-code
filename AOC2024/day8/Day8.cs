using Utility;
using Utility.Grid;

namespace AOC2024;

public class Day8
{
  public (string, string) Process(string input)
  {
    // Load and parse input data
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    return (ProcessPart1(data).ToString(), ProcessPart2(data).ToString());
  }


  private static long ProcessPart1(string[] lines)
  {
    var grid = new Grid(lines);
    var antinodes = new HashSet<Point2D<int>>();

    // Find all unique characters (antennas) excluding '.'
    var antennaTypes = new HashSet<char>();
    for (int r = 0; r < grid.Rows; r++)
    {
      for (int c = 0; c < grid.Cols; c++)
      {
        if (grid[r, c] != '.')
          antennaTypes.Add(grid[r, c]);
      }
    }

    // For each antenna type, find antinodes
    foreach (char antennaType in antennaTypes)
    {
      var antennas = grid.FindAll(antennaType);

      // Check all pairs of antennas
      for (int i = 0; i < antennas.Count; i++)
      {
        for (int j = i + 1; j < antennas.Count; j++)
        {
          var antenna1 = antennas[i];
          var antenna2 = antennas[j];

          var delta = antenna2 - antenna1;

          // Calculate the antinode positions
          var antinode1 = antenna2 + delta;
          var antinode2 = antenna1 - delta;

          // Check if antinodes are within bounds
          if (grid.IsInBounds(antinode1))
            antinodes.Add(antinode1);
          if (grid.IsInBounds(antinode2))
            antinodes.Add(antinode2);
        }
      }
    }

    return antinodes.Count;
  }

  private static long ProcessPart2(string[] lines)
  {
    var grid = new Grid(lines);
    var antinodes = new HashSet<Point2D<int>>();

    // Find all unique characters (antennas) excluding '.'
    var antennaTypes = new HashSet<char>();
    for (int r = 0; r < grid.Rows; r++)
    {
      for (int c = 0; c < grid.Cols; c++)
      {
        if (grid[r, c] != '.')
          antennaTypes.Add(grid[r, c]);
      }
    }

    // For each antenna type, find antinodes
    foreach (char antennaType in antennaTypes)
    {
      var antennas = grid.FindAll(antennaType);

      // If there are multiple antennas of this type, they're all antinodes
      if (antennas.Count > 1)
      {
        foreach (var antenna in antennas)
          antinodes.Add(antenna);
      }

      // Check all pairs of antennas
      for (int i = 0; i < antennas.Count; i++)
      {
        for (int j = i + 1; j < antennas.Count; j++)
        {
          var antenna1 = antennas[i];
          var antenna2 = antennas[j];

          var delta = antenna2 - antenna1;

          // Find all antinodes in both directions
          var pos = antenna2 + delta;
          while (grid.IsInBounds(pos))
          {
            antinodes.Add(pos);
            pos = pos + delta;
          }

          pos = antenna1 - delta;
          while (grid.IsInBounds(pos))
          {
            antinodes.Add(pos);
            pos = pos - delta;
          }
        }
      }
    }

    return antinodes.Count;
  }
}