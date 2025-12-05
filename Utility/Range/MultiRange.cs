namespace Utility;

public class MultiRange
{
  private List<Range>? _merged;
  private List<Range> _ranges = new();
  private List<Range>? _sorted;

  public MultiRange() { }

  public MultiRange(MultiRange other)
  {
    foreach (var r in other.Ranges)
    {
      Range n = new(r);
      _ranges.Add(n);
    }
  }

  public List<Range> Ranges
  {
    get => _ranges;
    set
    {
      _ranges = value;
      InvalidateCache();
    }
  }

  public List<Range> Sorted
  {
    get
    {
      if (_sorted == null)
      {
        _sorted = _ranges.OrderBy(r => r.Start).ToList();
      }

      return _sorted;
    }
  }

  public List<Range> Merged
  {
    get
    {
      if (_merged == null)
      {
        _merged = CalculateMergedRanges();
      }

      return _merged;
    }
  }

  public long Len => Ranges.Aggregate(1L, (a, b) => a += b.Len);

  public long Sum => Ranges.Sum(range => range.Len);

  public long UniqueSum => Merged.Sum(range => range.Len);

  public void AddRange(Range range)
  {
    _ranges.Add(range);
    InvalidateCache();
  }

  private void InvalidateCache()
  {
    _sorted = null;
    _merged = null;
  }

  private List<Range> CalculateMergedRanges()
  {
    if (_ranges.Count == 0) return new List<Range>();

    var sortedRanges = Sorted;
    var mergedRanges = new List<Range>();

    var currentRange = new Range(sortedRanges[0]);

    for (int i = 1; i < sortedRanges.Count; i++)
    {
      var nextRange = sortedRanges[i];

      // Check if ranges overlap or are adjacent
      if (nextRange.Start <= currentRange.End + 1)
      {
        // Merge ranges by extending the end if necessary
        currentRange.End = Math.Max(currentRange.End, nextRange.End);
      }
      else
      {
        // No overlap, add current range and start a new one
        mergedRanges.Add(currentRange);
        currentRange = new Range(nextRange);
      }
    }

    // Don't forget to add the last range
    mergedRanges.Add(currentRange);

    return mergedRanges;
  }

  public bool Contains(long value)
  {
    return Ranges.Any(range => value >= range.Start && value <= range.End);
  }
}