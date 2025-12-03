namespace Utility;

public class DictMultiRange<T>
{
  public Dictionary<T, Range> Ranges = new();

  public DictMultiRange() { }

  public DictMultiRange(DictMultiRange<T> other)
  {
    foreach (var r in other.Ranges)
    {
      Range n = new(r.Value);
      Ranges[r.Key] = n;
    }
  }

  public long len => Ranges.Aggregate(1L, (a, b) => a *= b.Value.Len);
}