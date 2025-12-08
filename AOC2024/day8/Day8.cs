using Utility;

namespace AOC2024;

public class Day8
{
  public (string, string) Process(string input)
  {
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    var grid = new Grid(data);
    
    var part1 = AntennaGrid.FindAllAntinodes(grid, includeResonant: false, '.');
    var part2 = AntennaGrid.FindAllAntinodes(grid, includeResonant: true, '.');

    return (part1.Count.ToString(), part2.Count.ToString());
  }
}
