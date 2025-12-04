namespace Utility;

public class Directions
{
  public static readonly int[][] AllDirections =
  [
    [0, 1], // Right
    [1, 0], // Down
    [0, -1], // Left
    [-1, 0], // Up
    [1, 1], // Down-right (Diagonal)
    [1, -1], // Down-left (Diagonal)
    [-1, 1], // Up-right (Diagonal)
    [-1, -1] // Up-left (Diagonal)
  ];

  public static readonly int[][] Diagonals =
  [
    [1, 1], // Down-right (Diagonal)
    [1, -1], // Down-left (Diagonal)
    [-1, 1], // Up-right (Diagonal)
    [-1, -1] // Up-left (Diagonal)
  ];

  private (int, int)[] _deltas = { (-1, 0), (0, 1), (1, 0), (0, -1) };
}