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
    var result = new List<char>(positions.Count);
    foreach (var pos in positions)
    {
      result.Add(GetValueAtPosition(pos));
    }
    return result;
  }

  // Get positions in specified directions from a given position
  public List<int[]> GetDirectionPositions(int[] position, string? indicators)
  {
    var directions = new Dictionary<string, int[]>
    {
      { "+u", [position[0] - 1, position[1]] },
      { "+d", [position[0] + 1, position[1]] },
      { "+l", [position[0], position[1] - 1] },
      { "+r", [position[0], position[1] + 1] },
      { "-ul", [position[0] - 1, position[1] - 1] },
      { "-ur", [position[0] - 1, position[1] + 1] },
      { "-dl", [position[0] + 1, position[1] - 1] },
      { "-dr", [position[0] + 1, position[1] + 1] }
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

    var filteredResult = new List<int[]>();
    foreach (var pos in result)
    {
      if (OnBoard(pos))
        filteredResult.Add(pos);
    }
    return filteredResult;
  }

  // Get all values in a specified column
  public List<char> GetColumnValues(int col)
  {
    var result = new List<char>(Lines.Count);
    foreach (var line in Lines)
    {
      result.Add(line[col]);
    }
    return result;
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
    var result = new List<int[]>();
    for (int row = 0; row < Rows; row++)
    {
      for (int col = 0; col < Columns; col++)
      {
        if (Lines[row][col] == target)
        {
          result.Add(new[] { row, col });
        }
      }
    }
    return result;
  }

  // Print the map to console
  public void PrintMap()
  {
    Lines.ForEach(Console.WriteLine);
  }
}
