namespace Utility;

public static class ShapeUtils
{
  // Generate all unique orientations (rotations and flips) of a 2D bool array
  public static List<bool[,]> GetAllOrientations(bool[,] shape)
  {
    var result = new List<bool[,]>();
    var seen = new HashSet<string>();
    for (int flip = 0; flip < 2; flip++)
    {
      bool[,] current = flip == 0 ?
        shape :
        FlipHorizontal(shape);
      for (int rot = 0; rot < 4; rot++)
      {
        bool[,] rotated = Rotate(current, rot);
        string key = ShapeToString(rotated);
        if (seen.Add(key))
          result.Add(rotated);
      }
    }

    return result;
  }

  public static bool[,] Rotate(bool[,] arr, int times)
  {
    bool[,] result = arr;
    for (int i = 0; i < times; i++)
      result = Rotate90(result);
    return result;
  }

  public static bool[,] Rotate90(bool[,] arr)
  {
    int h = arr.GetLength(0), w = arr.GetLength(1);
    bool[,] res = new bool[w, h];
    for (int y = 0; y < h; y++)
      for (int x = 0; x < w; x++)
        res[x, h - 1 - y] = arr[y, x];
    return res;
  }

  public static bool[,] FlipHorizontal(bool[,] arr)
  {
    int h = arr.GetLength(0), w = arr.GetLength(1);
    bool[,] res = new bool[h, w];
    for (int y = 0; y < h; y++)
      for (int x = 0; x < w; x++)
        res[y, w - 1 - x] = arr[y, x];
    return res;
  }

  public static string ShapeToString(bool[,] arr)
  {
    int h = arr.GetLength(0), w = arr.GetLength(1);
    char[] chars = new char[h * (w + 1)];
    int idx = 0;
    for (int y = 0; y < h; y++)
    {
      for (int x = 0; x < w; x++)
        chars[idx++] = arr[y, x] ?
          '#' :
          '.';
      chars[idx++] = '\n';
    }

    return new string(chars);
  }
  // Checks if a shape can be placed at (oy, ox) in the grid
  public static bool CanPlace(bool[,] grid, bool[,] shape, int oy, int ox)
  {
    int h = grid.GetLength(0), w = grid.GetLength(1);
    int sh = shape.GetLength(0), sw = shape.GetLength(1);
    if (oy + sh > h || ox + sw > w) return false;

    for (int y = 0; y < sh; y++)
      for (int x = 0; x < sw; x++)
        if (shape[y, x] && grid[oy + y, ox + x])
          return false;

    return true;
  }

  // Place or remove a shape at (oy, ox) in the grid
  public static void Place(bool[,] grid, bool[,] shape, int oy, int ox, bool val)
  {
    int sh = shape.GetLength(0), sw = shape.GetLength(1);
    for (int y = 0; y < sh; y++)
      for (int x = 0; x < sw; x++)
        if (shape[y, x])
          grid[oy + y, ox + x] = val;
  }

  /// <summary>
  ///   Counts the number of filled cells in a shape
  /// </summary>
  /// <param name="shape">The 2D boolean array representing the shape</param>
  /// <returns>Number of true cells in the shape</returns>
  public static int CountShapeCells(bool[,] shape)
  {
    int count = 0;
    int h = shape.GetLength(0);
    int w = shape.GetLength(1);
    for (int y = 0; y < h; y++)
    {
      for (int x = 0; x < w; x++)
      {
        if (shape[y, x]) count++;
      }
    }

    return count;
  }

  /// <summary>
  ///   Precomputes all orientations (rotations + flips) for multiple shapes
  /// </summary>
  /// <param name="shapes">List of shapes to process</param>
  /// <returns>Array of orientation lists for each shape</returns>
  public static List<bool[,]>[] PrecomputeAllOrientations(List<bool[,]> shapes)
  {
    return shapes.Select(GetAllOrientations).ToArray();
  }

  /// <summary>
  ///   Precomputes cell counts for multiple shapes
  /// </summary>
  /// <param name="shapes">List of shapes to process</param>
  /// <returns>Array of cell counts for each shape</returns>
  public static int[] PrecomputeShapeCellCounts(List<bool[,]> shapes)
  {
    return shapes.Select(CountShapeCells).ToArray();
  }
}