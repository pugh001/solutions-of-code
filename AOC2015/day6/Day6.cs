using Utility;
using Utility.Grid;

namespace AOC2015;

public class Day6
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input).ToList();
    var lightGrid = new LightGrid();
    
    lightGrid.ApplyInstructions(data);
    
    var lightsOn = lightGrid.CountLightsOn();
    var totalBrightness = lightGrid.CalculateTotalBrightness();

    return (lightsOn.ToString(), totalBrightness.ToString());
  }
}
