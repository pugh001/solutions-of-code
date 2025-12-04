namespace Utility;

public class Coordinate3D
{

  private static readonly Coordinate3D[] neighbors3D =
  {
    (-1, -1, -1), (-1, -1, 0), (-1, -1, 1), (-1, 0, -1), (-1, 0, 0), (-1, 0, 1), (-1, 1, -1), (-1, 1, 0), (-1, 1, 1),
    (0, -1, -1), (0, -1, 0), (0, -1, 1), (0, 0, -1), (0, 0, 1), (0, 1, -1), (0, 1, 0), (0, 1, 1),
    (1, -1, -1), (1, -1, 0), (1, -1, 1), (1, 0, -1), (1, 0, 0), (1, 0, 1), (1, 1, -1), (1, 1, 0), (1, 1, 1)
  };

  public readonly int X;
  public readonly int Y;
  public readonly int Z;

  public Coordinate3D(int x, int y, int z)
  {
    this.X = x;
    this.Y = y;
    this.Z = z;
  }

  public Coordinate3D(string stringformat)
  {
    int[] n = stringformat.ExtractInts().ToArray();
    X = n[0];
    Y = n[1];
    Z = n[2];
  }

  public List<Coordinate3D> Rotations => new()
  {
    (X, Y, Z),
    (X, Z, -Y),
    (X, -Y, -Z),
    (X, -Z, Y),
    (Y, X, -Z),
    (Y, Z, X),
    (Y, -X, Z),
    (Y, -Z, -X),
    (Z, X, Y),
    (Z, Y, -X),
    (Z, -X, -Y),
    (Z, -Y, X),
    (-X, Y, -Z),
    (-X, Z, Y),
    (-X, -Y, Z),
    (-X, -Z, -Y),
    (-Y, X, Z),
    (-Y, Z, -X),
    (-Y, -X, -Z),
    (-Y, -Z, X),
    (-Z, X, -Y),
    (-Z, Y, X),
    (-Z, -X, Y),
    (-Z, -Y, -X)
  };

  public static implicit operator Coordinate3D((int x, int y, int z) a)
  {
    return new Coordinate3D(a.x, a.y, a.z);
  }

  public static implicit operator (int x, int y, int z)(Coordinate3D a)
  {
    return (a.X, a.Y, a.Z);
  }
  public static Coordinate3D operator +(Coordinate3D a)
  {
    return a;
  }
  public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b)
  {
    return new Coordinate3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
  }
  public static Coordinate3D operator -(Coordinate3D a)
  {
    return new Coordinate3D(-a.X, -a.Y, -a.Z);
  }
  public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b)
  {
    return a + -b;
  }
  public static bool operator ==(Coordinate3D a, Coordinate3D b)
  {
    return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
  }
  public static bool operator !=(Coordinate3D a, Coordinate3D b)
  {
    return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
  }

  public int ManhattanDistance(Coordinate3D other)
  {
    return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
  }
  public int ManhattanMagnitude()
  {
    return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
  }

  public override bool Equals(object obj)
  {
    if (obj.GetType() != typeof(Coordinate3D)) return false;

    return this == (Coordinate3D)obj;
  }

  public override int GetHashCode()
  {
    //Primes times coordinates for fewer collisions
    return 137 * X + 149 * Y + 163 * Z;
  }
  public override string ToString()
  {
    return $"{X}, {Y}, {Z}";
  }

  public static Coordinate3D[] GetNeighbors()
  {
    return neighbors3D;
  }

  internal void Deconstruct(out int x, out int y, out int z)
  {
    x = this.X;
    y = this.Y;
    z = this.Z;
  }
}