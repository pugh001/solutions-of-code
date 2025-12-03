namespace Utility;

public class Map
{
  // Constructor to initialize map from a list of strings
  public Map(List<string> lines)
  {
    Lines = lines;
    Rows = lines.Count;
    Columns = lines[0].Length;
  }

  public List<string> Lines { get; }
  public int Rows { get; }
  public int Columns { get; }

  // Get character values at specified directions from a given position
  public List<char> GetDirectionValues(int[] position, string? indicators)
  {
    var positions = GetDirectionPositions(position, indicators);
    return positions.Select(GetValueAtPosition).ToList();
  }

  // Get positions in specified directions from a given position
  public List<int[]> GetDirectionPositions(int[] position, string? indicators)
  {
    var directions = new Dictionary<string, int[]>
    {
      { "+u", new[] { position[0] - 1, position[1] } },
      { "+d", new[] { position[0] + 1, position[1] } },
      { "+l", new[] { position[0], position[1] - 1 } },
      { "+r", new[] { position[0], position[1] + 1 } },
      { "-ul", new[] { position[0] - 1, position[1] - 1 } },
      { "-ur", new[] { position[0] - 1, position[1] + 1 } },
      { "-dl", new[] { position[0] + 1, position[1] - 1 } },
      { "-dr", new[] { position[0] + 1, position[1] + 1 } }
    };

    var result = new List<int[]>();
    indicators = indicators?.ToLower() ?? "8";
    indicators = indicators switch
    {
      "4" => "+u+d+l+r",
      "diagonals" => "-ul-ur-dl-dr",
      _ => "+u+d+l+r-ul-ur-dl-dr"
    };

    foreach (string? key in directions.Keys)
    {
      if (indicators.Contains(key)) result.Add(directions[key]);
    }

    return result.Where(OnBoard).ToList();
  }

  // Get all values in a specified column
  public List<char> GetColumnValues(int col)
  {
    return Lines.Select(line => line[col]).ToList();
  }

  // Get all values in a specified row
  public List<char> GetRowValues(int row)
  {
    return Lines[row].ToCharArray().ToList();
  }

  // Get value at a specific position
  public char GetValueAtPosition(int[] position)
  {
    return Lines[position[0]][position[1]];
  }

  // Check if a position is on the map
  public bool OnBoard(int[] position)
  {
    return position[0] >= 0 && position[0] < Rows && position[1] >= 0 && position[1] < Columns;
  }

  // Find all positions with a specific character
  public List<int[]> FindAll(char target)
  {
    return Enumerable.Range(0, Rows).SelectMany(row
      => Enumerable.Range(0, Columns).Where(col => Lines[row][col] == target).Select(col => new[] { row, col })).ToList();
  }

  // Print the map to console
  public void PrintMap()
  {
    Lines.ForEach(Console.WriteLine);
  }
}