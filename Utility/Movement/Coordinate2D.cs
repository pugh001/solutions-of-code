namespace Utility;

public class Coordinate2D
{
  public static readonly Coordinate2D origin = new(0, 0);
  public static readonly Coordinate2D unit_x = new(1, 0);
  public static readonly Coordinate2D unit_y = new(0, 1);
  public readonly int x;
  public readonly int y;

  public Coordinate2D(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public Coordinate2D((int x, int y) coord)
  {
    x = coord.x;
    y = coord.y;
  }

  public Coordinate2D(string StringPair)
  {
    string[] t = StringPair.Split(',');
    x = int.Parse(t[0]);
    y = int.Parse(t[1]);
  }

  public Coordinate2D RotateCW(int degrees, Coordinate2D center)
  {
    var offset = center - this;
    return center + offset.RotateCW(degrees);
  }
  public Coordinate2D RotateCW(int degrees)
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

  private Coordinate2D RotateCW()
  {
    return new Coordinate2D(y, -x);
  }

  public Coordinate2D RotateCCW(int degrees, Coordinate2D center)
  {
    var offset = center - this;
    return center + offset.RotateCCW(degrees);
  }
  public Coordinate2D RotateCCW(int degrees)
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

  private Coordinate2D RotateCCW()
  {
    return new Coordinate2D(-y, x);
  }

  public static Coordinate2D operator +(Coordinate2D a)
  {
    return a;
  }
  public static Coordinate2D operator +(Coordinate2D a, Coordinate2D b)
  {
    return new Coordinate2D(a.x + b.x, a.y + b.y);
  }
  public static Coordinate2D operator -(Coordinate2D a)
  {
    return new Coordinate2D(-a.x, -a.y);
  }
  public static Coordinate2D operator -(Coordinate2D a, Coordinate2D b)
  {
    return a + -b;
  }
  public static Coordinate2D operator *(int scale, Coordinate2D a)
  {
    return new Coordinate2D(scale * a.x, scale * a.y);
  }
  public static bool operator ==(Coordinate2D a, Coordinate2D b)
  {
    return a.x == b.x && a.y == b.y;
  }
  public static bool operator !=(Coordinate2D a, Coordinate2D b)
  {
    return a.x != b.x || a.y != b.y;
  }

  public static implicit operator Coordinate2D((int x, int y) a)
  {
    return new Coordinate2D(a.x, a.y);
  }

  public static implicit operator (int x, int y)(Coordinate2D a)
  {
    return (a.x, a.y);
  }

  public int ManDistance(Coordinate2D other)
  {
    int x = Math.Abs(this.x - other.x);
    int y = Math.Abs(this.y - other.y);
    return x + y;
  }

  public override bool Equals(object obj)
  {
    if (obj == null) return false;
    if (obj.GetType() != typeof(Coordinate2D)) return false;

    return this == (Coordinate2D)obj;
  }

  public override int GetHashCode()
  {
    return (100 * x + y).GetHashCode();
  }

  public override string ToString()
  {
    return string.Concat("(", x, ", ", y, ")");
  }
  public void Deconstruct(out int xVal, out int yVal)
  {
    xVal = x;
    yVal = y;
  }
}