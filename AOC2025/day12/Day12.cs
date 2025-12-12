using Utility;

namespace AOC2025;

/// <summary>
///   Day 12: Present Packing Problem
///   Solves a 2D bin packing problem where different shaped presents must fit into regions under Christmas trees.
///   Uses backtracking with constraint satisfaction to determine which regions can fit all required presents.
///   OPTIMIZATION TECHNIQUES USED:
///   1. Most Constraining Variable (MCV) heuristic: Choose largest shapes first to reduce search space
///   2. Early feasibility pruning: Skip impossible regions before expensive backtracking
///   3. Precomputation: Calculate shape orientations and cell counts once, reuse across regions
///   4. Efficient backtracking: Place/unplace shapes with minimal overhead
/// </summary>
public class Day12
{
  /// <summary>
  ///   Main processing method that parses input and solves the present packing problem
  /// </summary>
  /// <param name="input">Input file path containing shape definitions and region requirements</param>
  /// <returns>Tuple of (part1: number of regions that can fit all presents, part2: unused)</returns>
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    var groups = Parsing.SplitByEmptyLines(data.ToArray());

    var shapes = ParseShapes(groups);
    var regions = ParseRegions(groups);

    int regionsThatFitAllPresents = CountRegionsThatFitAllPresents(shapes, regions);

    return (regionsThatFitAllPresents.ToString(), "0");
  }

  /// <summary>
  ///   Parses all shape definitions from the input groups
  /// </summary>
  /// <param name="groups">Input groups separated by empty lines</param>
  /// <returns>List of 2D boolean arrays representing present shapes</returns>
  private static List<bool[,]> ParseShapes(List<List<string>> groups)
  {
    var shapes = new List<bool[,]>();

    foreach (var group in groups)
    {
      if (group.Count == 0) continue;

      string header = group[0].Trim();
      if (!IsShapeDefinition(header)) continue;

      bool[,]? shape = ParseShapeFromGroup(group);
      if (shape != null)
      {
        shapes.Add(shape);
      }
    }

    return shapes;
  }

  /// <summary>
  ///   Parses all region requirements from the input groups
  /// </summary>
  /// <param name="groups">Input groups separated by empty lines</param>
  /// <returns>List of region dimensions and required present counts</returns>
  private static List<(int w, int h, int[] counts)> ParseRegions(List<List<string>> groups)
  {
    var regions = new List<(int w, int h, int[] counts)>();

    foreach (var group in groups)
    {
      if (group.Count == 0) continue;

      string header = group[0].Trim();
      if (!IsRegionDefinition(header)) continue;

      foreach (string line in group)
      {
        var region = ParseRegionFromLine(line);
        regions.Add(region);
      }
    }

    return regions;
  }

  /// <summary>
  ///   Determines if a header represents a shape definition
  /// </summary>
  /// <param name="header">Header line to check</param>
  /// <returns>True if this is a shape definition (format: "N:")</returns>
  private static bool IsShapeDefinition(string header)
  {
    return header.Length >= 2 && char.IsDigit(header[0]) && header[1] == ':' && !header.Contains('x');
  }

  /// <summary>
  ///   Determines if a header represents a region definition
  /// </summary>
  /// <param name="header">Header line to check</param>
  /// <returns>True if this is a region definition (format: "WxH: ...")</returns>
  private static bool IsRegionDefinition(string header)
  {
    return header.Contains('x') && header.Contains(':');
  }

  /// <summary>
  ///   Parses a single shape from its group definition
  /// </summary>
  /// <param name="group">Group containing shape header and pattern lines</param>
  /// <returns>2D boolean array representing the shape, or null if malformed</returns>
  private static bool[,]? ParseShapeFromGroup(List<string> group)
  {
    // Extract shape pattern lines (skip header, ignore empty lines)
    string[] shapeLines = group.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
    if (shapeLines.Length == 0) return null; // Skip malformed shape definitions

    // Convert text pattern to 2D boolean array
    int height = shapeLines.Length;
    int width = shapeLines[0].Length;
    bool[,] shape = new bool[height, width];

    for (int y = 0; y < height; y++)
    {
      if (shapeLines[y].Length != width)
        throw new ArgumentException("Shape lines must have consistent width");

      for (int x = 0; x < width; x++)
      {
        shape[y, x] = shapeLines[y][x] == '#'; // '#' = part of shape, '.' = empty space
      }
    }

    return shape;
  }

  /// <summary>
  ///   Parses a single region definition from a line
  /// </summary>
  /// <param name="line">Line containing region definition (format: "WxH: count0 count1 ...")</param>
  /// <returns>Tuple of (width, height, required present counts)</returns>
  private static (int w, int h, int[] counts) ParseRegionFromLine(string line)
  {
    string[] parts = line.Split(':');
    string[] size = parts[0].Split('x');
    int width = int.Parse(size[0]);
    int height = int.Parse(size[1]);

    // Parse required count of each shape type (indexed by shape order)
    int[] counts = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

    return (width, height, counts);
  }

  /// <summary>
  ///   Counts how many regions can fit all their required presents
  /// </summary>
  /// <param name="shapes">All available present shapes</param>
  /// <param name="regions">All regions with their requirements</param>
  /// <returns>Number of regions that can accommodate all their presents</returns>
  private int CountRegionsThatFitAllPresents(List<bool[,]> shapes, List<(int w, int h, int[] counts)> regions)
  {
    // OPTIMIZATION: Precompute shape data once instead of per-region
    var allOrientations = ShapeUtils.PrecomputeAllOrientations(shapes);
    int[] shapeCells = ShapeUtils.PrecomputeShapeCellCounts(shapes);

    int successfulRegions = 0;

    foreach ((int width, int height, int[] counts) in regions)
    {
      if (CanRegionFitAllPresents(width, height, counts, allOrientations, shapeCells))
      {
        successfulRegions++;
      }
    }

    return successfulRegions;
  }

  /// <summary>
  ///   Checks if a region has sufficient area to fit all required presents
  /// </summary>
  /// <param name="width">Region width</param>
  /// <param name="height">Region height</param>
  /// <param name="requiredCounts">Required count of each present type</param>
  /// <param name="shapeCells">Precomputed cell counts for each shape</param>
  /// <returns>True if there's enough space for all presents</returns>
  private static bool HasSufficientArea(int width, int height, int[] requiredCounts, int[] shapeCells)
  {
    int totalRequiredCells = 0;

    for (int i = 0; i < requiredCounts.Length && i < shapeCells.Length; i++)
    {
      totalRequiredCells += requiredCounts[i] * shapeCells[i];
    }

    return totalRequiredCells <= width * height;
  }

  /// <summary>
  ///   Determines if a single region can fit all its required presents
  /// </summary>
  /// <param name="width">Region width</param>
  /// <param name="height">Region height</param>
  /// <param name="requiredCounts">Required count of each present type</param>
  /// <param name="allOrientations">Precomputed orientations for all shapes</param>
  /// <param name="shapeCells">Precomputed cell counts for all shapes</param>
  /// <returns>True if all presents can fit in this region</returns>
  private bool CanRegionFitAllPresents(int width,
    int height,
    int[] requiredCounts,
    List<bool[,]>[] allOrientations,
    int[] shapeCells)
  {
    // OPTIMIZATION: Early feasibility check - if total area needed exceeds region area, skip
    if (!HasSufficientArea(width, height, requiredCounts, shapeCells))
    {
      return false;
    }

    // Create empty grid and working copy of counts
    bool[,] grid = new bool[height, width];
    int[] workingCounts = (int[])requiredCounts.Clone();

    // Use backtracking solver to attempt placement
    return Solve(grid, allOrientations, workingCounts, width, height, shapeCells);
  }

  /// <summary>
  ///   Recursive backtracking solver for the 2D bin packing problem
  ///   Uses constraint satisfaction with pruning optimizations
  /// </summary>
  /// <param name="grid">Current state of the region grid</param>
  /// <param name="allOrientations">All possible rotations/flips for each shape</param>
  /// <param name="counts">Remaining count of each shape type to place</param>
  /// <param name="w">Region width</param>
  /// <param name="h">Region height</param>
  /// <param name="shapeCells">Precomputed cell count for each shape (for optimization)</param>
  /// <returns>True if all remaining presents can be placed, false otherwise</returns>
  private bool Solve(bool[,] grid, List<bool[,]>[] allOrientations, int[] counts, int w, int h, int[] shapeCells)
  {
    int nextShapeIdx = GetNextShapeIdx(allOrientations, counts, shapeCells);

    // Base case: if no more shapes to place, we've successfully placed everything
    if (nextShapeIdx == -1) return true;

    // Try all orientations (rotations + flips) of the selected shape
    var orientations = allOrientations[nextShapeIdx];
    foreach (bool[,] orientation in orientations)
    {
      int shapeHeight = orientation.GetLength(0);
      int shapeWidth = orientation.GetLength(1);

      // Try all valid positions in the grid for this orientation
      for (int gridY = 0; gridY <= h - shapeHeight; gridY++)
      {
        for (int gridX = 0; gridX <= w - shapeWidth; gridX++)
        {
          // Check if this position conflicts with already placed shapes
          if (!ShapeUtils.CanPlace(grid, orientation, gridY, gridX))
            continue;

          // PLACEMENT: temporarily place the shape
          ShapeUtils.Place(grid, orientation, gridY, gridX, true);
          counts[nextShapeIdx]--; // Reduce count of this shape type

          // RECURSION: try to place remaining shapes
          bool canPlaceRemainder = Solve(grid, allOrientations, counts, w, h, shapeCells);

          // BACKTRACK: remove the shape and restore count
          counts[nextShapeIdx]++;
          ShapeUtils.Place(grid, orientation, gridY, gridX, false);

          // If we found a solution, propagate success up the call stack
          if (canPlaceRemainder) return true;
        }
      }
    }

    // No valid placement found for this shape - backtrack
    return false;
  }
  private static int GetNextShapeIdx(List<bool[,]>[] allOrientations, int[] counts, int[] shapeCells)
  {

    // OPTIMIZATION: Most Constraining Variable (MCV) heuristic
    // Select the shape type with the largest area first to reduce branching factor
    // This helps fail faster when solutions are impossible
    int nextShapeIdx = -1;
    int largestArea = -1;

    for (int i = 0; i < counts.Length && i < allOrientations.Length; i++)
    {
      if (counts[i] <= 0) continue; // Skip shapes we don't need more of

      int shapeArea = shapeCells[i];
      if (shapeArea <= largestArea)
        continue;

      largestArea = shapeArea;
      nextShapeIdx = i;
    }

    return nextShapeIdx;
  }
}