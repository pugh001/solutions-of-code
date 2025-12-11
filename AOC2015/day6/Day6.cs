using Utility;

namespace AOC2015;

public class Day6
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input).ToList();
    var lightGrid = new LightGrid();

    lightGrid.ApplyInstructions(data);

    int lightsOn = lightGrid.CountLightsOn();
    long totalBrightness = lightGrid.CalculateTotalBrightness();

    return (lightsOn.ToString(), totalBrightness.ToString());
  }
}