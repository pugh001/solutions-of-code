using Utility;

namespace AOC2024;

public class Day15
{
  private static (Dictionary<Coordinate2D, char> map, int maxX, int maxY) map;
  private static (Dictionary<Coordinate2D, char> map, int maxX, int maxY) map2;
  private static List<List<Coordinate2D>> boxesToMove = new();
  private static List<Coordinate2D> movedPositions = new();
  private static readonly Queue<char> Moves = new();
  private static string movestring;

  public (string, string) Process(string input)
  {
    long result1 = 0;
    long result2 = 0;

    var data = SetupInputFile.OpenFile(input);

    string data2 = File.ReadAllText(input);

    var split = data2.SplitByDoubleNewline();

    split[0] = split[0].Replace("#", "##");
    split[0] = split[0].Replace("O", "[]");
    split[0] = split[0].Replace(".", "..");
    split[0] = split[0].Replace("@", "@.");

    map2 = split[0].GenerateMap(false);
    movestring = split[1];


    char[,] grid = null;
    char[,] scaledGrid = null;
    var robotPosition = (0, 0);
    string instructions = "";

    List<string> gridLines = new();
    bool instructionsStarted = false;

    foreach (string line in data)
    {
      if (line.StartsWith("#"))
      {
        gridLines.Add(line); // Remove the # and add to grid lines
        continue;
      }

      if (string.IsNullOrWhiteSpace(line))
      {
        instructionsStarted = true;
        continue;
      }

      if (instructionsStarted)
      {
        instructions += line;
      }
    }

    // Convert gridLines to char[,] grid
    int rows = gridLines.Count;
    int cols = gridLines[0].Length;
    grid = new char[rows, cols];
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        grid[r, c] = gridLines[r][c];
        if (grid[r, c] == '@')
        {
          robotPosition = (r, c);
        }
      }
    }

    result1 = processPart1(grid, robotPosition, instructions);

    (scaledGrid, robotPosition) = ScaleUpGrid(gridLines);
    //printOutGrid(scaledGrid);

    result2 = ProcessInput2();

    //printOutGrid(scaledGrid);

    return (result1.ToString(), result2.ToString());

    ;
  }
  private static void printOutGrid(char[,] grid)
  {

    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        Console.Write(grid[r, c]);
      }

      Console.WriteLine();
    }

  }

  private static long processPart1(char[,] grid, (int, int) robotPosition, string instructions)
  {
    var moveTo = (0, 0);

    foreach (char move in instructions)
    {
      var direction = move switch
      {
        '>' => (0, 1),
        '<' => (0, -1),
        '^' => (-1, 0),
        'v' => (1, 0),
        _ => (0, 0)
      };

      moveTo = (robotPosition.Item1 + direction.Item1, robotPosition.Item2 + direction.Item2);

      if (grid[moveTo.Item1, moveTo.Item2] == '#')
      {
        //Do nothing
        continue;
      }

      if (grid[moveTo.Item1, moveTo.Item2] == '.')
      {
        //simple swap
        grid[moveTo.Item1, moveTo.Item2] = '@';
        grid[robotPosition.Item1, robotPosition.Item2] = '.';
        robotPosition = moveTo;
        continue;
      }

      (long, long) nextPosition = (moveTo.Item1 + direction.Item1, moveTo.Item2 + direction.Item2);

      while (true)
      {
        if (grid[nextPosition.Item1, nextPosition.Item2] == '.')
        {
          grid[moveTo.Item1, moveTo.Item2] = '@';
          grid[nextPosition.Item1, nextPosition.Item2] = 'O';
          grid[robotPosition.Item1, robotPosition.Item2] = '.';
          robotPosition = moveTo;
          break;
        }

        if (grid[nextPosition.Item1, nextPosition.Item2] == '#')
        {
          break;
        }

        nextPosition = (nextPosition.Item1 + direction.Item1, nextPosition.Item2 + direction.Item2);

      }


    }

    return CalculateScore(grid);

  }

  private static long ProcessPart2A(char[,] grid, (int, int) robotPosition, string instructions)
  {
    foreach (char move in instructions)
    {
      var direction = move switch
      {
        '>' => (0, 1),
        '<' => (0, -1),
        '^' => (-1, 0),
        'v' => (1, 0),
        _ => (0, 0)
      };

      var moveTo = (robotPosition.Item1 + direction.Item1, robotPosition.Item2 + direction.Item2);

      if (grid[moveTo.Item1, moveTo.Item2] == '#') continue;

      if (grid[moveTo.Item1, moveTo.Item2] == '.')
      {
        grid[moveTo.Item1, moveTo.Item2] = '@';
        grid[robotPosition.Item1, robotPosition.Item2] = '.';
        robotPosition = moveTo;
        continue;
      }

      if (grid[moveTo.Item1, moveTo.Item2] == '[' && grid[moveTo.Item1, moveTo.Item2 + 1] == ']')
      {
        var nextPosition = (moveTo.Item1 + direction.Item1, moveTo.Item2 + direction.Item2);
        var nextNextPosition = (moveTo.Item1 + direction.Item1, moveTo.Item2 + direction.Item2 + 1);

        if (grid[nextPosition.Item1, nextPosition.Item2] == '.' && grid[nextNextPosition.Item1, nextNextPosition.Item2] == '.')
        {
          grid[nextPosition.Item1, nextPosition.Item2] = '[';
          grid[nextNextPosition.Item1, nextNextPosition.Item2] = ']';
          grid[moveTo.Item1, moveTo.Item2] = '.';
          grid[moveTo.Item1, moveTo.Item2 + 1] = '.';
          grid[moveTo.Item1, moveTo.Item2] = '@';
          grid[robotPosition.Item1, robotPosition.Item2] = '.';
          robotPosition = moveTo;
        }
      }
    }

    return CalculateScore(grid);
  }
  private static long processPart2a(char[,] grid, (int, int) robotPosition, string instructions)
  {
    foreach (char move in instructions)
    {
      var direction = move switch
      {
        '>' => (0, 2), // Horizontal moves adjust for box width
        '<' => (0, -2),
        '^' => (-1, 0), // Vertical moves remain single-row
        'v' => (1, 0),
        _ => (0, 0)
      };

      var moveTo = (robotPosition.Item1 + direction.Item1, robotPosition.Item2 + direction.Item2);

      // If the robot encounters a wall, skip the move
      if (grid[moveTo.Item1, moveTo.Item2] == '#')
      {
        continue;
      }

      // If the robot encounters an empty space, simply move there
      if (grid[moveTo.Item1, moveTo.Item2] == '.')
      {
        grid[moveTo.Item1, moveTo.Item2] = '@';
        grid[robotPosition.Item1, robotPosition.Item2] = '.';
        robotPosition = moveTo;
        continue;
      }

      // If the robot encounters a box, check if the box can be pushed
      if (grid[moveTo.Item1, moveTo.Item2] == '[' && grid[moveTo.Item1, moveTo.Item2 + 1] == ']')
      {
        var nextPosition = (moveTo.Item1 + direction.Item1, moveTo.Item2 + direction.Item2);
        var nextNextPosition = (moveTo.Item1 + direction.Item1, moveTo.Item2 + direction.Item2 + 1);

        // Check if the destination for the box is valid (both parts must be empty)
        if (grid[nextPosition.Item1, nextPosition.Item2] == '.' && grid[nextNextPosition.Item1, nextNextPosition.Item2] == '.')
        {
          // Move the box
          grid[nextPosition.Item1, nextPosition.Item2] = '[';
          grid[nextNextPosition.Item1, nextNextPosition.Item2] = ']';

          // Clear the old box position
          grid[moveTo.Item1, moveTo.Item2] = '.';
          grid[moveTo.Item1, moveTo.Item2 + 1] = '.';

          // Move the robot
          grid[moveTo.Item1, moveTo.Item2] = '@';
          grid[robotPosition.Item1, robotPosition.Item2] = '.';
          robotPosition = moveTo;
        }
      }
    }

    return CalculateScore(grid);
  }

  private static (char[,], (int, int)) ScaleUpGrid(List<string> gridLines)
  {
    // Convert gridLines to char[,] grid
    var robotPosition = (0, 0);
    int rows = gridLines.Count;
    int cols = gridLines[0].Length * 2;
    char[,] grid = new char[rows, cols];
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c += 2)
      {

        grid[r, c] = gridLines[r][c / 2];
        if (grid[r, c] == '@')
        {
          robotPosition = (r, c);
          grid[r, c + 1] = '.';
        }
        else
        {
          if (grid[r, c] == '#')
          {
            grid[r, c + 1] = '#';
          }
          else if (grid[r, c] == '.')
          {
            grid[r, c + 1] = '.';
          }
          else
          {
            grid[r, c] = '[';
            grid[r, c + 1] = ']';
          }
        }
      }
    }

    return (grid, robotPosition);
  }

  private static long ProcessInput2()
  {
    long sum = 0;
    SetupMoves();
    var cur = map2.map.First(q => q.Value == '@').Key;
    CompassDirection movedir;
    while (Moves.TryDequeue(out char move))
    {
      movedir = move switch
      {
        '>' => CompassDirection.E,
        'v' => CompassDirection.N,
        '<' => CompassDirection.W,
        '^' => CompassDirection.S
      };

      var next = cur.MoveDirection(movedir);
      if (map2.map[next] == '[' || map2.map[next] == ']')
      {
        bool canmove = TryMoveBox(movedir, cur, next, true);
        if (canmove)
        {
          char edge1 = '[';
          char edge2 = ']';
          for (int x = boxesToMove.Count - 1; x >= 0; x--)
          {
            var curbox = boxesToMove[x];
            var nextpos1 = curbox[0].MoveDirection(movedir);
            var nextpos2 = curbox[1].MoveDirection(movedir);
            if (nextpos1.x < nextpos2.x)
            {
              map2.map[nextpos1] = edge1;
              map2.map[nextpos2] = edge2;
            }
            else
            {
              map2.map[nextpos1] = edge2;
              map2.map[nextpos2] = edge1;
            }

            movedPositions.Add(nextpos1);
            movedPositions.Add(nextpos2);
          }

          var newEmpty = boxesToMove.SelectMany(q => q.Where(e => !movedPositions.Contains(e))).ToList();

          foreach (var empty in newEmpty)
          {
            map2.map[empty] = '.';
          }

          map2.map[cur] = '.';
          map2.map[next] = '@';
          cur = next;
        }

        movedPositions = new List<Coordinate2D>();
        boxesToMove = new List<List<Coordinate2D>>();
      }
      else if (map2.map[next] == '.')
      {
        map2.map[cur] = '.';
        map2.map[next] = '@';
        cur = next;
      }

    }

    var boxes = map2.map.Where(q => q.Value == '[').ToList();

    foreach (var box in boxes)
    {
      sum += 100 * box.Key.y + box.Key.x;
    }

    return sum;
  }

  private static bool TryMoveBox(CompassDirection dir, Coordinate2D cur, Coordinate2D next, bool partTwo = false)
  {
    bool canmove = false;
    if (partTwo)
    {
      if (map2.map[next] == '.')
      {
        return true;
      }

      if (map2.map[next] == '[' || map2.map[next] == ']')
      {
        if (dir == CompassDirection.N || dir == CompassDirection.S)
        {
          Coordinate2D otherpart;
          if (map2.map[next] == '[')
          {
            otherpart = next.MoveDirection(CompassDirection.E);
          }
          else
          {
            otherpart = next.MoveDirection(CompassDirection.W);
          }

          boxesToMove.Add(new List<Coordinate2D> { next, otherpart });
          canmove = TryMoveBox(dir, cur, otherpart.MoveDirection(dir), partTwo) &&
                    TryMoveBox(dir, cur, next.MoveDirection(dir), partTwo);
        }
        else
        {
          var nextbox = next.MoveDirection(dir);
          boxesToMove.Add(new List<Coordinate2D> { next, nextbox });
          canmove = TryMoveBox(dir, cur, nextbox.MoveDirection(dir), partTwo);
        }
      }
      else if (map2.map[next] == '#')
      {
        return false;
      }
    }
    else
    {
      if (map.map[next] == '.')
      {
        map.map[cur] = '.';
        map.map[next] = 'O';
        return true;
      }

      if (map.map[next] == 'O')
      {
        var nextbox = next.MoveDirection(dir);
        canmove = TryMoveBox(dir, cur, nextbox);
      }
      else if (map.map[next] == '#')
      {
        return false;
      }

      return canmove;
    }

    return canmove;
  }


  private static long CalculateScore(char[,] grid)
  {
    long score = 0;
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);

    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        if (grid[r, c] == '[' || grid[r, c] == 'O')
        {
          score += r * 100 + c;
        }
      }
    }

    return score;
  }

  private static void SetupMoves()
  {
    foreach (char c in movestring)
    {
      if (c == '>' || c == '<' || c == '^' || c == 'v')
      {
        Moves.Enqueue(c);
      }
    }
  }
}