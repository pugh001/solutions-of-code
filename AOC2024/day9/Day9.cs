using Utility;

namespace AOC2024;

public class Day9
{
  public (string, string) Process(string input)
  {
    // Load and parse input data
    var data = SetupInputFile.OpenFile(input);

    // Extract data from the first line
    string inputLine = data.First();

    return (ChecksumWithGapHandling(inputLine).ToString(), ChecksumWithGroupedMovement(inputLine).ToString());

  }

  private static long ChecksumWithGapHandling(string inputLine)
  {
    long resultPart1 = 0;
    var store = LoadStore(inputLine);
    int last = store.Count;
    // Compact the disk while checking for overlap and handling gaps
    for (int i = 0; i < store.Count; i++)
    {
      if (store[i].Item1 >= 0) continue; // Skip non-free space blocks

      if (i >= last)
      {
        break;
      }

      last = FindLast(last, store);
      if (store[last].Item1 <= 0)
        continue;

      store[i] = store[last];
      long fileIdValue, sizeOfValue;
      (_, sizeOfValue) = store[last];
      fileIdValue = -1;
      sizeOfValue++;
      store[last] = (fileIdValue, sizeOfValue);

    }

    resultPart1 = CalculateChecksum(store);
    return resultPart1;
  }
  private static int FindLast(int last, List<(long, long)> store)
  {
    last--;
    while (store[last].Item1 <= 0)
    {
      last--;
    }

    return last;
  }

  private static long CalculateChecksum(List<(long, long)> store)
  {
    long checksum = 0;

    for (int i = 0; i < store.Count; i++)
    {
      if (store[i].Item1 > 0) // Only include valid file blocks (non-zero values)
      {
        checksum += i * store[i].Item1;
      }
    }

    return checksum;
  }

  private static long ChecksumWithGroupedMovement(string inputLine)
  {
    long resultPart2 = 0;
    var store = LoadStore(inputLine);

    int last = store.Count - 1;

    while (last >= 0)
    {
      // Skip free space blocks
      if (store[last].Item1 == -1)
      {
        last--;
        continue;
      }

      // Read the size of the last valid block
      (long fileId, long size) = store[last];
      int startIndex = last - (int)size + 1;

      // Try to find a suitable gap earlier in the store
      for (int i = 0; i <= last - size; i++)
      {
        // Check if there is a continuous gap of the required size
        bool isGap = true;
        for (int j = i; j < i + size; j++)
        {
          if (store[j].Item1 == -1)
            continue;

          isGap = false;
          break;
        }

        if (!isGap)
          continue;

        {
          // Move the block to the gap
          for (int j = 0; j < size; j++)
          {
            store[i + j] = (fileId, size);
          }

          // Mark the original block as free space
          for (int j = startIndex; j <= last; j++)
          {
            store[j] = (-1, size);
          }

          break; // Exit the gap search loop
        }
      }

      // Move to the next block
      last = startIndex - 1;
    }

    // Calculate the checksum based on the final state
    resultPart2 = CalculateChecksum(store);

    return resultPart2;
  }

  private static List<(long, long)> LoadStore(string inputLine)
  {
    var store = new List<(long, long)>();
    bool isBlock = true;
    long fileId = 0;

    // Parse the input into blocks
    foreach (long size in inputLine.Select(xxx => (long)char.GetNumericValue(xxx)))
    {
      if (isBlock)
      {

        // Add file blocks
        for (int i = 0; i < size; i++)
        {
          store.Add((fileId, size));
        }

        fileId++;
        isBlock = false;
        continue;
      }

      // Add free space blocks
      for (int i = 0; i < size; i++)
      {
        store.Add((-1, size)); // -1 indicates free space
      }

      isBlock = true;
    }

    return store;
  }
}