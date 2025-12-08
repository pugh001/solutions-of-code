using Utility;

namespace AOC2024;

public class Day15
{
  private Dictionary<Coordinate2D, char> _map = new();
  private Dictionary<Coordinate2D, char> _map2 = new();
  private List<List<Coordinate2D>> _boxesToMove = [];
  private List<Coordinate2D> _movedPositions = [];
  private Queue<char> _moves = new();
  private string _moveString = string.Empty;

  public (string, string) Process(string input)
  {
    var data = File.ReadAllText(input);
    var sections = data.SplitByDoubleNewline();
    
    // Part 1 setup
    var (map1, _, _) = sections[0].GenerateMap(false);
    _map = map1;
    
    // Part 2 setup - scale up the grid
    string scaledGrid = sections[0]
      .Replace("#", "##")
      .Replace("O", "[]")
      .Replace(".", "..")
      .Replace("@", "@.");
    
    var (map2, _, _) = scaledGrid.GenerateMap(false);
    _map2 = map2;
    _moveString = sections[1];


    var robotPos1 = _map.First(kvp => kvp.Value == '@').Key;
    var instructions = _moveString.Where(c => "^v<>".Contains(c)).ToArray();
    
    long result1 = ProcessPart1(robotPos1, instructions);
    long result2 = ProcessPart2();

    return (result1.ToString(), result2.ToString());

    ;
  }

  private long ProcessPart1(Coordinate2D robotPosition, char[] instructions)
  {
    var currentPos = robotPosition;

    foreach (char move in instructions)
    {
      var direction = MoveDirections.CompassDirectionFromArrow(move);
      var nextPos = currentPos.MoveDirection(direction, true);

      // Check if hitting a wall
      if (_map.GetValueOrDefault(nextPos, '#') == '#')
        continue;

      if (CheckIfEmpty(nextPos, ref currentPos))
        continue;

      // Check for box pushing
      if (_map.GetValueOrDefault(nextPos, '.') != 'O')
        continue;

      var pushPos = nextPos;
        
      // Find the end of the box chain
      while (_map.GetValueOrDefault(pushPos, '#') == 'O')
      {
        pushPos = pushPos.MoveDirection(direction, true);
      }

      // If we can push (empty space found)
      if (_map.GetValueOrDefault(pushPos, '#') != '.')
        continue;

      _map[currentPos] = '.';
      _map[nextPos] = '@';
      _map[pushPos] = 'O';
      currentPos = nextPos;
    }

    return CalculateScore(_map);
  }
  private bool CheckIfEmpty(Coordinate2D nextPos, ref Coordinate2D currentPos)
  {

    // Check if moving to empty space
    if (_map.GetValueOrDefault(nextPos, '.') != '.')
      return false;

    _map[currentPos] = '.';
    _map[nextPos] = '@';
    currentPos = nextPos;
    return true;

  }


  private long ProcessPart2()
  {
    SetupMoves();
    var currentPos = _map2.First(kvp => kvp.Value == '@').Key;
    
    while (_moves.TryDequeue(out char move))
    {
      var direction = MoveDirections.CompassDirectionFromArrow(move);
      var nextPos = currentPos.MoveDirection(direction, true);
      
      var nextCell = _map2.GetValueOrDefault(nextPos, '#');
      
      switch (nextCell)
      {
        case '#':
          continue;
        case '.':
          _map2[currentPos] = '.';
          _map2[nextPos] = '@';
          currentPos = nextPos;
          continue;
        case '[' or ']':
        {
          if (TryMoveBox(direction, currentPos, nextPos, true))
          {
            ExecuteBoxMoves(direction);
            _map2[currentPos] = '.';
            _map2[nextPos] = '@';
            currentPos = nextPos;
          }
        
          _movedPositions.Clear();
          _boxesToMove.Clear();
          break;
        }
      }

    }

    return _map2.Where(kvp => kvp.Value == '[')
                .Sum(kvp => 100 * kvp.Key.Y + kvp.Key.X);
  }

  private void ExecuteBoxMoves(CompassDirection direction)
  {
    // Move boxes from last to first to avoid overwriting
    for (int i = _boxesToMove.Count - 1; i >= 0; i--)
    {
      var box = _boxesToMove[i];
      var newPos1 = box[0].MoveDirection(direction, true);
      var newPos2 = box[1].MoveDirection(direction, true);
      
      _map2[newPos1] = newPos1.X < newPos2.X ? '[' : ']';
      _map2[newPos2] = newPos1.X < newPos2.X ? ']' : '[';
      
      _movedPositions.Add(newPos1);
      _movedPositions.Add(newPos2);
    }

    // Clear old positions that weren't overwritten
    var emptyPositions = _boxesToMove.SelectMany(box => box)
                                     .Where(pos => !_movedPositions.Contains(pos));
    
    foreach (var pos in emptyPositions)
    {
      _map2[pos] = '.';
    }
  }

  private bool TryMoveBox(CompassDirection direction, Coordinate2D current, Coordinate2D next, bool partTwo = false)
  {
    if (!partTwo) return false; // Only implement part 2 logic
    
    var nextCell = _map2.GetValueOrDefault(next, '#');
    
    if (nextCell == '.') return true;
    if (nextCell == '#') return false;
    
    if (nextCell is '[' or ']')
    {
      // For vertical movement, need to consider both parts of the box
      if (direction is CompassDirection.N or CompassDirection.S)
      {
        var otherPart = nextCell == '[' 
          ? next.MoveDirection(CompassDirection.E, true)
          : next.MoveDirection(CompassDirection.W, true);
        
        _boxesToMove.Add([next, otherPart]);
        
        return TryMoveBox(direction, current, next.MoveDirection(direction, true), true) &&
               TryMoveBox(direction, current, otherPart.MoveDirection(direction, true), true);
      }
      else // Horizontal movement
      {
        var nextBox = next.MoveDirection(direction, true);
        _boxesToMove.Add([next, nextBox]);
        return TryMoveBox(direction, current, nextBox.MoveDirection(direction, true), true);
      }
    }
    
    return false;
  }


  private static long CalculateScore(Dictionary<Coordinate2D, char> map)
  {
    return map.Where(kvp => kvp.Value is 'O' or '[')
              .Sum(kvp => kvp.Key.Y * 100 + kvp.Key.X);
  }

  private void SetupMoves()
  {
    _moves.Clear();
    foreach (char c in _moveString.Where(ch => "^v<>".Contains(ch)))
    {
      _moves.Enqueue(c);
    }
  }
}
