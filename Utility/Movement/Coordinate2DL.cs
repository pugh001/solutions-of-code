namespace Utility;

public class Coordinate2Dl
{
  public static readonly Coordinate2Dl Origin = new(0, 0);
  public static readonly Coordinate2Dl UnitX = new(1, 0);
  public static readonly Coordinate2Dl UnitY = new(0, 1);
  public readonly long X;
  public readonly long Y;

  public Coordinate2Dl(long x, long y)
  {
    this.X = x;
    this.Y = y;
  }

  public Coordinate2Dl((long x, long y) coord)
  {
    X = coord.x;
    Y = coord.y;
  }

  public Coordinate2Dl RotateCw(int degrees, Coordinate2Dl center)
  {
    var offset = center - this;
    return center + offset.RotateCw(degrees);
  }
  public Coordinate2Dl RotateCw(int degrees)
  {
    return (degrees / 90 % 4) switch
    {
      0 => this,
      1 => RotateCw(),
      2 => -this,
      3 => RotateCcw(),
      _ => this
    };
  }

  private Coordinate2Dl RotateCw()
  {
    return new Coordinate2Dl(Y, -X);
  }

  public Coordinate2Dl RotateCcw(int degrees, Coordinate2Dl center)
  {
    var offset = center - this;
    return center + offset.RotateCcw(degrees);
  }
  public Coordinate2Dl RotateCcw(int degrees)
  {
    return (degrees / 90 % 4) switch
    {
      0 => this,
      1 => RotateCcw(),
      2 => -this,
      3 => RotateCw(),
      _ => this
    };
  }

  private Coordinate2Dl RotateCcw()
  {
    return new Coordinate2Dl(-Y, X);
  }

  public static Coordinate2Dl operator +(Coordinate2Dl a)
  {
    return a;
  }
  public static Coordinate2Dl operator +(Coordinate2Dl a, Coordinate2Dl b)
  {
    return new Coordinate2Dl(a.X + b.X, a.Y + b.Y);
  }
  public static Coordinate2Dl operator -(Coordinate2Dl a)
  {
    return new Coordinate2Dl(-a.X, -a.Y);
  }
  public static Coordinate2Dl operator -(Coordinate2Dl a, Coordinate2Dl b)
  {
    return a + -b;
  }
  public static Coordinate2Dl operator *(long scale, Coordinate2Dl a)
  {
    return new Coordinate2Dl(scale * a.X, scale * a.Y);
  }
  public static bool operator ==(Coordinate2Dl a, Coordinate2Dl b)
  {
    return a.X == b.X && a.Y == b.Y;
  }
  public static bool operator !=(Coordinate2Dl a, Coordinate2Dl b)
  {
    return a.X != b.X || a.Y != b.Y;
  }

  public static implicit operator Coordinate2Dl((long x, long y) a)
  {
    return new Coordinate2Dl(a.x, a.y);
  }

  public static implicit operator (long x, long y)(Coordinate2Dl a)
  {
    return (a.X, a.Y);
  }

  public long ManDistance(Coordinate2Dl other)
  {
    long x = Math.Abs(this.X - other.X);
    long y = Math.Abs(this.Y - other.Y);
    return x + y;
  }
  public override bool Equals(object obj)
  {
    if (obj == null) return false;
    if (obj.GetType() != typeof(Coordinate2Dl)) return false;

    return this == (Coordinate2Dl)obj;
  }

  public override int GetHashCode()
  {
    return (100 * X + Y).GetHashCode();
  }

  public override string ToString()
  {
    return string.Concat("(", X, ", ", Y, ")");
  }

  public void Deconstruct(out long xVal, out long yVal)
  {
    xVal = X;
    yVal = Y;
  }
}