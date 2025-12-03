namespace Utility;

public class Coordinate2DL
{
  public static readonly Coordinate2DL origin = new(0, 0);
  public static readonly Coordinate2DL unit_x = new(1, 0);
  public static readonly Coordinate2DL unit_y = new(0, 1);
  public readonly long x;
  public readonly long y;

  public Coordinate2DL(long x, long y)
  {
    this.x = x;
    this.y = y;
  }

  public Coordinate2DL((long x, long y) coord)
  {
    x = coord.x;
    y = coord.y;
  }

  public Coordinate2DL RotateCW(int degrees, Coordinate2DL center)
  {
    var offset = center - this;
    return center + offset.RotateCW(degrees);
  }
  public Coordinate2DL RotateCW(int degrees)
  {
    return (degrees / 90 % 4) switch
    {
      0 => this,
      1 => RotateCW(),
      2 => -this,
      3 => RotateCCW(),
      _ => this
    };
  }

  private Coordinate2DL RotateCW()
  {
    return new Coordinate2DL(y, -x);
  }

  public Coordinate2DL RotateCCW(int degrees, Coordinate2DL center)
  {
    var offset = center - this;
    return center + offset.RotateCCW(degrees);
  }
  public Coordinate2DL RotateCCW(int degrees)
  {
    return (degrees / 90 % 4) switch
    {
      0 => this,
      1 => RotateCCW(),
      2 => -this,
      3 => RotateCW(),
      _ => this
    };
  }

  private Coordinate2DL RotateCCW()
  {
    return new Coordinate2DL(-y, x);
  }

  public static Coordinate2DL operator +(Coordinate2DL a)
  {
    return a;
  }
  public static Coordinate2DL operator +(Coordinate2DL a, Coordinate2DL b)
  {
    return new Coordinate2DL(a.x + b.x, a.y + b.y);
  }
  public static Coordinate2DL operator -(Coordinate2DL a)
  {
    return new Coordinate2DL(-a.x, -a.y);
  }
  public static Coordinate2DL operator -(Coordinate2DL a, Coordinate2DL b)
  {
    return a + -b;
  }
  public static Coordinate2DL operator *(long scale, Coordinate2DL a)
  {
    return new Coordinate2DL(scale * a.x, scale * a.y);
  }
  public static bool operator ==(Coordinate2DL a, Coordinate2DL b)
  {
    return a.x == b.x && a.y == b.y;
  }
  public static bool operator !=(Coordinate2DL a, Coordinate2DL b)
  {
    return a.x != b.x || a.y != b.y;
  }

  public static implicit operator Coordinate2DL((long x, long y) a)
  {
    return new Coordinate2DL(a.x, a.y);
  }

  public static implicit operator (long x, long y)(Coordinate2DL a)
  {
    return (a.x, a.y);
  }

  public long ManDistance(Coordinate2DL other)
  {
    long x = Math.Abs(this.x - other.x);
    long y = Math.Abs(this.y - other.y);
    return x + y;
  }
  public override bool Equals(object obj)
  {
    if (obj == null) return false;
    if (obj.GetType() != typeof(Coordinate2DL)) return false;

    return this == (Coordinate2DL)obj;
  }

  public override int GetHashCode()
  {
    return (100 * x + y).GetHashCode();
  }

  public override string ToString()
  {
    return string.Concat("(", x, ", ", y, ")");
  }

  public void Deconstruct(out long xVal, out long yVal)
  {
    xVal = x;
    yVal = y;
  }
}