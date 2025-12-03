namespace Utility;

public class Coordinate4D
{


  private static Coordinate4D[] neighbors;
  private readonly int w;
  private readonly int x;
  private readonly int y;
  private readonly int z;

  public Coordinate4D(int x, int y, int z, int w)
  {
    this.x = x;
    this.y = y;
    this.z = z;
    this.w = w;
  }

  public static implicit operator Coordinate4D((int x, int y, int z, int w) a)
  {
    return new Coordinate4D(a.x, a.y, a.z, a.w);
  }

  public static implicit operator (int x, int y, int z, int w)(Coordinate4D a)
  {
    return (a.x, a.y, a.z, a.w);
  }
  public static Coordinate4D operator +(Coordinate4D a)
  {
    return a;
  }
  public static Coordinate4D operator +(Coordinate4D a, Coordinate4D b)
  {
    return new Coordinate4D(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
  }
  public static Coordinate4D operator -(Coordinate4D a)
  {
    return new Coordinate4D(-a.x, -a.y, -a.z, -a.w);
  }
  public static Coordinate4D operator -(Coordinate4D a, Coordinate4D b)
  {
    return a + -b;
  }
  public static bool operator ==(Coordinate4D a, Coordinate4D b)
  {
    return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
  }
  public static bool operator !=(Coordinate4D a, Coordinate4D b)
  {
    return a.x != b.x || a.y != b.y || a.z != b.z || a.z != b.z;
  }

  public int ManhattanDistance(Coordinate4D other)
  {
    return Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z) + Math.Abs(w - other.w);
  }

  public override bool Equals(object obj)
  {
    if (obj.GetType() != typeof(Coordinate4D)) return false;

    return this == (Coordinate4D)obj;
  }

  public override int GetHashCode()
  {
    return 137 * x + 149 * y + 163 * z + 179 * w;
  }


  public static Coordinate4D[] GetNeighbors()
  {
    if (neighbors != null) return neighbors;

    List<Coordinate4D> neighborList = new();

    for (int x = -1; x <= 1; x++)
    {
      for (int y = -1; y <= 1; y++)
      {
        for (int z = -1; z <= 1; z++)
        {
          for (int w = -1; w <= 1; w++)
          {
            if (!(0 == x && 0 == y && 0 == z && 0 == w))
            {
              neighborList.Add((x, y, z, w));
            }
          }
        }
      }
    }

    neighbors = neighborList.ToArray();
    return neighbors;
  }
}