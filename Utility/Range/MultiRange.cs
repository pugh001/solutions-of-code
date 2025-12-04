namespace Utility;

public class MultiRange
{
  public List<Range> Ranges = new();

  public MultiRange() { }

  public MultiRange(IEnumerable<Range> ranges)
  {
    this.Ranges = new List<Range>(ranges);
  }

  public MultiRange(MultiRange other)
  {
    foreach (var r in other.Ranges)
    {
      Range n = new(r);
      Ranges.Add(n);
    }
  }

  public long Len => Ranges.Aggregate(1L, (a, b) => a *= b.Len);
}