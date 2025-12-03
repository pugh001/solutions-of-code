namespace Utility;

public class TestFiles
{
  public static (bool, string) GetInputData(int day, int year, string myFile)
  {
    string path = SetupInputFile.GetSolutionDirectory();
    string fileOne = $"{path}/AOC{year}/day{day}/{myFile}";
    bool exists = File.Exists(fileOne);
    return (exists, fileOne);
  }
}