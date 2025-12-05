namespace Utility;

public class MultiRange
{
  public List<Range> Ranges = new();

  public MultiRange() { }

  //public MultiRange(IEnumerable<Range> ranges)
  //{
  //  this.Ranges = new List<Range>(ranges);
  //}
  
  public void AddRange(Range range)
  {
    this.Ranges.Add(range);
  }
  public MultiRange(MultiRange other)
  {
    foreach (var r in other.Ranges)
    {
      Range n = new(r);
      Ranges.Add(n);
    }
  }
  public bool Contains(long value)
  {
    return Ranges.Any(range => value >= range.Start && value <= range.End);
  }
  public long Len => Ranges.Aggregate(1L, (a, b) => a += b.Len);
  public long SumOfRanges()
  {
    return Ranges.Sum(range => range.Len);
  }
  public long SumOfNoneOverlappingRanges()
  {
    if (Ranges.Count == 0) return 0;
    
    // Sort ranges by start position
    var sortedRanges = Ranges.OrderBy(r => r.Start).ToList();
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
    
    // Calculate sum of merged ranges
    return mergedRanges.Sum(range => range.Len);
  }
}
