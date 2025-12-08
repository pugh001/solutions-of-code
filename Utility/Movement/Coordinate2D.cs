namespace Utility;

public class Coordinate2D : IDistanceCalculable<Coordinate2D>
{
  public static readonly Coordinate2D Origin = new(0, 0);
  public static readonly Coordinate2D UnitX = new(1, 0);
  public static readonly Coordinate2D UnitY = new(0, 1);
  public readonly int X;
  public readonly int Y;

  public Coordinate2D(int x, int y)
  {
    X = x;
    Y = y;
  }

  public Coordinate2D((int x, int y) coord)
  {
    X = coord.x;
    Y = coord.y;
  }

  public Coordinate2D(string stringPair)
  {
    string[] t = stringPair.Split(',');
    X = int.Parse(t[0]);
    Y = int.Parse(t[1]);
  }

  public Coordinate2D RotateCw(int degrees, Coordinate2D center)
  {
    var offset = center - this;
    return center + offset.RotateCw(degrees);
  }
  public Coordinate2D RotateCw(int degrees)
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

  private Coordinate2D RotateCw()
  {
    return new Coordinate2D(Y, -X);
  }

  public Coordinate2D RotateCcw(int degrees, Coordinate2D center)
  {
    var offset = center - this;
    return center + offset.RotateCcw(degrees);
  }
  public Coordinate2D RotateCcw(int degrees)
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

  private Coordinate2D RotateCcw()
  {
    return new Coordinate2D(-Y, X);
  }

  public static Coordinate2D operator +(Coordinate2D a)
  {
    return a;
  }
  public static Coordinate2D operator +(Coordinate2D a, Coordinate2D b)
  {
    return new Coordinate2D(a.X + b.X, a.Y + b.Y);
  }
  public static Coordinate2D operator -(Coordinate2D a)
  {
    return new Coordinate2D(-a.X, -a.Y);
  }
  public static Coordinate2D operator -(Coordinate2D a, Coordinate2D b)
  {
    return a + -b;
  }
  public static Coordinate2D operator *(int scale, Coordinate2D a)
  {
    return new Coordinate2D(scale * a.X, scale * a.Y);
  }
  public static bool operator ==(Coordinate2D a, Coordinate2D b)
  {
    return a.X == b.X && a.Y == b.Y;
  }
  public static bool operator !=(Coordinate2D a, Coordinate2D b)
  {
    return a.X != b.X || a.Y != b.Y;
  }

  public static implicit operator Coordinate2D((int x, int y) a)
  {
    return new Coordinate2D(a.x, a.y);
  }

  public static implicit operator (int x, int y)(Coordinate2D a)
  {
    return (a.X, a.Y);
  }

  public int ManDistance(Coordinate2D other)
  {
    int x = Math.Abs(X - other.X);
    int y = Math.Abs(Y - other.Y);
    return x + y;
  }

  public double EuclideanDistance(Coordinate2D other)
  {
    return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
  }

  public double DistanceTo(Coordinate2D other)
  {
    return EuclideanDistance(other);
  }

  public override bool Equals(object obj)
  {
    if (obj == null) return false;
    if (obj.GetType() != typeof(Coordinate2D)) return false;

    return this == (Coordinate2D)obj;
  }

  public override int GetHashCode()
  {
    return (100 * X + Y).GetHashCode();
  }

  public override string ToString()
  {
    return string.Concat("(", X, ", ", Y, ")");
  }
  public void Deconstruct(out int xVal, out int yVal)
  {
    xVal = X;
    yVal = Y;
  }
}
