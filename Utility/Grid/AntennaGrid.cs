namespace Utility;

/// <summary>
/// Utility for antenna and antinode calculations
/// </summary>
public static class AntennaGrid
{
  /// <summary>
  /// Calculates antinode positions for a pair of antennas
  /// </summary>
  public static (Point2D<int> antinode1, Point2D<int> antinode2) CalculateAntinodes(
    Point2D<int> antenna1, 
    Point2D<int> antenna2)
  {
    var delta = antenna2 - antenna1;
    return (antenna2 + delta, antenna1 - delta);
  }

  /// <summary>
  /// Calculates all antinodes in a line between two antennas (both directions)
  /// </summary>
  public static IEnumerable<Point2D<int>> CalculateResonantAntinodes(
    Point2D<int> antenna1,
    Point2D<int> antenna2,
    Func<Point2D<int>, bool> isInBounds)
  {
    var delta = antenna2 - antenna1;
    
    // Forward direction from antenna2
    var pos = antenna2 + delta;
    while (isInBounds(pos))
    {
      yield return pos;
      pos = pos + delta;
    }
    
    // Backward direction from antenna1
    pos = antenna1 - delta;
    while (isInBounds(pos))
    {
      yield return pos;
      pos = pos - delta;
    }
  }

  /// <summary>
  /// Finds all antinodes for antennas of the same type
  /// </summary>
  public static HashSet<Point2D<int>> FindAntinodes(
    List<Point2D<int>> antennas,
    Func<Point2D<int>, bool> isInBounds,
    bool includeResonant = false)
  {
    var antinodes = new HashSet<Point2D<int>>();
    
    // If resonant and multiple antennas, all antenna positions are antinodes
    if (includeResonant && antennas.Count > 1)
    {
      foreach (var antenna in antennas)
        antinodes.Add(antenna);
    }
    
    // Check all pairs of antennas
    for (int i = 0; i < antennas.Count; i++)
    {
      for (int j = i + 1; j < antennas.Count; j++)
      {
        if (includeResonant)
        {
          // Add all antinodes in both directions
          foreach (var antinode in CalculateResonantAntinodes(antennas[i], antennas[j], isInBounds))
            antinodes.Add(antinode);
        }
        else
        {
          // Add simple antinodes
          var (antinode1, antinode2) = CalculateAntinodes(antennas[i], antennas[j]);
          if (isInBounds(antinode1)) antinodes.Add(antinode1);
          if (isInBounds(antinode2)) antinodes.Add(antinode2);
        }
      }
    }
    
    return antinodes;
  }

  /// <summary>
  /// Finds all antinodes for all antenna types in a grid
  /// </summary>
  public static HashSet<Point2D<int>> FindAllAntinodes(
    Grid grid,
    bool includeResonant = false,
    params char[] excludeChars)
  {
    var allAntinodes = new HashSet<Point2D<int>>();
    var antennaTypes = grid.GetUniqueCharacters(excludeChars);
    
    foreach (char antennaType in antennaTypes)
    {
      var antennas = grid.FindAll(antennaType);
      var antinodes = FindAntinodes(antennas, grid.IsInBounds, includeResonant);
      
      foreach (var antinode in antinodes)
        allAntinodes.Add(antinode);
    }
    
    return allAntinodes;
  }
}
