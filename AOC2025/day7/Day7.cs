using Utility;

namespace AOC2025;

public class Day7
{
  public (string, string) Process(string input)
  {
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    var grid = new Grid(data);

    var startPos = grid.FindFirst('S') ?? Point2D<int>.Origin;

    int part1 = CountSplits(grid, startPos);
    long part2 = CountUniquePaths(grid, startPos);

    return (part1.ToString(), part2.ToString());
  }

  private int CountSplits(Grid grid, Point2D<int> startPos)
  {
    // Ultra-optimized approach for part 1: count unique split positions
    bool[,] visited = new bool[grid.Rows, grid.Cols];
    var queue = new Queue<Point2D<int>>();
    int splitCount = 0;

    queue.Enqueue(startPos);
    visited[startPos.Y, startPos.X] = true;

    while (queue.Count > 0)
    {
      var current = queue.Dequeue();
      int downY = current.Y + 1;
      int downX = current.X;

      // Quick bounds check using raw coordinates
      if (downY >= grid.Rows)
        continue;

      char downChar = grid[downY, downX];

      if (downChar == '^')
      {
        // This is a split point - count it only once
        if (!visited[downY, downX])
        {
          splitCount++;
          visited[downY, downX] = true;

          // Add both left and right paths
          int leftY = downY;
          int leftX = downX - 1;
          int rightY = downY;
          int rightX = downX + 1;

          if (leftX >= 0 && !visited[leftY, leftX])
          {
            queue.Enqueue(new Point2D<int>(leftX, leftY));
            visited[leftY, leftX] = true;
          }

          if (rightX < grid.Cols && !visited[rightY, rightX])
          {
            queue.Enqueue(new Point2D<int>(rightX, rightY));
            visited[rightY, rightX] = true;
          }
        }
      }
      else if (!visited[downY, downX])
      {
        // Continue moving down
        queue.Enqueue(new Point2D<int>(downX, downY));
        visited[downY, downX] = true;
      }
    }

    return splitCount;
  }

  private long CountUniquePaths(Grid grid, Point2D<int> startPos)
  {
    // For part 2: use dynamic programming to count all possible paths through the grid
    // Each position records how many different ways we can reach it
    long[,] dp = new long[grid.Rows, grid.Cols];

    // Start with 1 path at the starting position
    dp[startPos.Y, startPos.X] = 1;

    // Process positions row by row (since we only move down, left, or right from splits)
    for (int y = startPos.Y; y < grid.Rows - 1; y++)
    {
      for (int x = 0; x < grid.Cols; x++)
      {
        if (dp[y, x] == 0) continue; // No paths reach this position

        long currentPaths = dp[y, x];
        int downY = y + 1;

        // Check what's below this position
        if (downY < grid.Rows)
        {
          char belowChar = grid[downY, x];

          if (belowChar == '^')
          {
            // Split: paths go left and right from the split position
            int leftX = x - 1;
            int rightX = x + 1;

            if (leftX >= 0)
            {
              dp[downY, leftX] += currentPaths;
            }

            if (rightX < grid.Cols)
            {
              dp[downY, rightX] += currentPaths;
            }
          }
          else if (belowChar == '.' || belowChar == 'S')
          {
            // Continue straight down
            dp[downY, x] += currentPaths;
          }
          // If belowChar is anything else, path ends here
        }
      }
    }

    // Sum all paths that reached the last row
    long totalPaths = 0;
    for (int x = 0; x < grid.Cols; x++)
    {
      totalPaths += dp[grid.Rows - 1, x];
    }

    return totalPaths;
  }
}