using System.Collections;

namespace Utility;

public class StringMap<T> : IEnumerable<(Point2D<int> Index, T Value)>
{
  private readonly T[,] fValues;

  public StringMap(string input, Func<char, T> selector)
  {
    string[] lines = input.Lines().ToArray();
    int lineLen = lines[0].Length;
    fValues = new T[lineLen, lines.Length];
    for (int y = 0; y < lines.Length; y++)
    {
      string line = lines[y];
      for (int x = 0; x < lineLen; x++)
        fValues[x, y] = selector(line[x]);
    }
  }

  public int Width => fValues.GetLength(0);
  public int Height => fValues.GetLength(1);
  public Point2D<int> Size => (Width, Height);

  public T this[Point2D<int> idx]
  {
    get => fValues[idx.X, idx.Y];
    set => fValues[idx.X, idx.Y] = value;
  }

  public IEnumerator<(Point2D<int> Index, T Value)> GetEnumerator()
  {
    return GetIndexedValues().GetEnumerator();
  }
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetIndexedValues().GetEnumerator();
  }

  public bool Contains(Point2D<int> idx)
  {
    return idx.X >= 0 && idx.Y >= 0 && idx.X < Width && idx.Y < Height;
  }

  public T GetValueOrDefault(Point2D<int> idx)
  {
    return GetValueOrDefault(idx, default);
  }
  public T GetValueOrDefault(Point2D<int> idx, T defaultValue)
  {
    if (Contains(idx))
      return this[idx];

    return defaultValue;
  }

  public bool TryGetValue(Point2D<int> idx, out T value)
  {
    if (Contains(idx))
    {
      value = this[idx];
      return true;
    }

    value = default;
    return false;
  }

  public IEnumerable<IEnumerable<T>> Rows()
  {
    return Enumerable.Range(0, Height).Select(y => Enumerable.Range(0, Width).Select(x => fValues[x, y]));
  }


  private IEnumerable<(Point2D<int> Index, T Value)> GetIndexedValues()
  {
    for (int x = 0; x < Width; x++)
      for (int y = 0; y < Height; y++)
        yield return ((x, y), fValues[x, y]);
  }
}