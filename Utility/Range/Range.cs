namespace Utility;

public class Range
{
  public long End;
  public long Start;

  public Range(long Start, long End)
  {
    this.Start = Start;
    this.End = End;
  }

  //Forced Deep Copy
  public Range(Range other)
  {
    Start = other.Start;
    End = other.End;
  }
  public long Len => End - Start + 1;

  public override string ToString()
  {
    return $"[{Start}, {End}] ({Len})";
  }
}