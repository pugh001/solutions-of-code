namespace Utility;

public class Coordinate3D
{

  private static readonly Coordinate3D[] neighbors3D =
  {
    (-1, -1, -1), (-1, -1, 0), (-1, -1, 1), (-1, 0, -1), (-1, 0, 0), (-1, 0, 1), (-1, 1, -1), (-1, 1, 0), (-1, 1, 1),
    (0, -1, -1), (0, -1, 0), (0, -1, 1), (0, 0, -1), (0, 0, 1), (0, 1, -1), (0, 1, 0), (0, 1, 1),
    (1, -1, -1), (1, -1, 0), (1, -1, 1), (1, 0, -1), (1, 0, 0), (1, 0, 1), (1, 1, -1), (1, 1, 0), (1, 1, 1)
  };

  public readonly int x;
  public readonly int y;
  public readonly int z;

  public Coordinate3D(int x, int y, int z)
  {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  public Coordinate3D(string stringformat)
  {
    int[] n = stringformat.ExtractInts().ToArray();
    x = n[0];
    y = n[1];
    z = n[2];
  }

  public List<Coordinate3D> Rotations => new()
  {
    (x, y, z),
    (x, z, -y),
    (x, -y, -z),
    (x, -z, y),
    (y, x, -z),
    (y, z, x),
    (y, -x, z),
    (y, -z, -x),
    (z, x, y),
    (z, y, -x),
    (z, -x, -y),
    (z, -y, x),
    (-x, y, -z),
    (-x, z, y),
    (-x, -y, z),
    (-x, -z, -y),
    (-y, x, z),
    (-y, z, -x),
    (-y, -x, -z),
    (-y, -z, x),
    (-z, x, -y),
    (-z, y, x),
    (-z, -x, y),
    (-z, -y, -x)
  };

  public static implicit operator Coordinate3D((int x, int y, int z) a)
  {
    return new Coordinate3D(a.x, a.y, a.z);
  }

  public static implicit operator (int x, int y, int z)(Coordinate3D a)
  {
    return (a.x, a.y, a.z);
  }
  public static Coordinate3D operator +(Coordinate3D a)
  {
    return a;
  }
  public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b)
  {
    return new Coordinate3D(a.x + b.x, a.y + b.y, a.z + b.z);
  }
  public static Coordinate3D operator -(Coordinate3D a)
  {
    return new Coordinate3D(-a.x, -a.y, -a.z);
  }
  public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b)
  {
    return a + -b;
  }
  public static bool operator ==(Coordinate3D a, Coordinate3D b)
  {
    return a.x == b.x && a.y == b.y && a.z == b.z;
  }
  public static bool operator !=(Coordinate3D a, Coordinate3D b)
  {
    return a.x != b.x || a.y != b.y || a.z != b.z;
  }

  public int ManhattanDistance(Coordinate3D other)
  {
    return Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z);
  }
  public int ManhattanMagnitude()
  {
    return Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
  }

  public override bool Equals(object obj)
  {
    if (obj.GetType() != typeof(Coordinate3D)) return false;

    return this == (Coordinate3D)obj;
  }

  public override int GetHashCode()
  {
    //Primes times coordinates for fewer collisions
    return 137 * x + 149 * y + 163 * z;
  }
  public override string ToString()
  {
    return $"{x}, {y}, {z}";
  }

  public static Coordinate3D[] GetNeighbors()
  {
    return neighbors3D;
  }

  internal void Deconstruct(out int x, out int y, out int z)
  {
    x = this.x;
    y = this.y;
    z = this.z;
  }
}