namespace Utility;

public class SetupInputFile
{
  public static IEnumerable<string> OpenFile(string path)
  {
    return File.ReadLines(path);
  }
  public static string GetSolutionDirectory()
  {
    string currentDirectory = Directory.GetCurrentDirectory();
    var directoryInfo = new DirectoryInfo(currentDirectory);

    while (directoryInfo != null && !File.Exists(Path.Combine(directoryInfo.FullName, "SolutionOfCode.sln")))
    {
      directoryInfo = directoryInfo.Parent;
    }

    return directoryInfo?.FullName;
  }
}