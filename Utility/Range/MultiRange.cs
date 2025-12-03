namespace Utility;

public class MultiRange
{
  public List<Range> Ranges = new();

  public MultiRange() { }

  public MultiRange(IEnumerable<Range> Ranges)
  {
    this.Ranges = new List<Range>(Ranges);
  }

  public MultiRange(MultiRange other)
  {
    foreach (var r in other.Ranges)
    {
      Range n = new(r);
      Ranges.Add(n);
    }
  }

  public long len => Ranges.Aggregate(1L, (a, b) => a *= b.Len);
}