namespace Utility;

public class Range(long start,
  long end)
{
  public long End = end;
  public long Start = start;

  //Forced Deep Copy
  public Range(Range other) : this(other.Start, other.End)
  {
  }
  public long Len => End - Start + 1;

  public override string ToString()
  {
    return $"[{Start}, {End}] ({Len})";
  }
}