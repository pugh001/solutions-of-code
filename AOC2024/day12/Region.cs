namespace AOC2024;

public class Region
{

  public Region(char plantType)
  {
    PlantType = plantType;
    Area = 0;
    Perimeter = 0;
  }
  public char PlantType { get; }
  public int Area { get; set; }
  public int Perimeter { get; set; }
}