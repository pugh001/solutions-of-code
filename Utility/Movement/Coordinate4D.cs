namespace Utility;

public class Coordinate4D
{


  private static Coordinate4D[] _neighbors;
  private readonly int _w;
  private readonly int _x;
  private readonly int _y;
  private readonly int _z;

  public Coordinate4D(int x, int y, int z, int w)
  {
    this._x = x;
    this._y = y;
    this._z = z;
    this._w = w;
  }

  public static implicit operator Coordinate4D((int x, int y, int z, int w) a)
  {
    return new Coordinate4D(a.x, a.y, a.z, a.w);
  }

  public static implicit operator (int x, int y, int z, int w)(Coordinate4D a)
  {
    return (a._x, a._y, a._z, a._w);
  }
  public static Coordinate4D operator +(Coordinate4D a)
  {
    return a;
  }
  public static Coordinate4D operator +(Coordinate4D a, Coordinate4D b)
  {
    return new Coordinate4D(a._x + b._x, a._y + b._y, a._z + b._z, a._w + b._w);
  }
  public static Coordinate4D operator -(Coordinate4D a)
  {
    return new Coordinate4D(-a._x, -a._y, -a._z, -a._w);
  }
  public static Coordinate4D operator -(Coordinate4D a, Coordinate4D b)
  {
    return a + -b;
  }
  public static bool operator ==(Coordinate4D a, Coordinate4D b)
  {
    return a._x == b._x && a._y == b._y && a._z == b._z && a._w == b._w;
  }
  public static bool operator !=(Coordinate4D a, Coordinate4D b)
  {
    return a._x != b._x || a._y != b._y || a._z != b._z || a._z != b._z;
  }

  public int ManhattanDistance(Coordinate4D other)
  {
    return Math.Abs(_x - other._x) + Math.Abs(_y - other._y) + Math.Abs(_z - other._z) + Math.Abs(_w - other._w);
  }

  public override bool Equals(object obj)
  {
    if (obj.GetType() != typeof(Coordinate4D)) return false;

    return this == (Coordinate4D)obj;
  }

  public override int GetHashCode()
  {
    return 137 * _x + 149 * _y + 163 * _z + 179 * _w;
  }


public static Coordinate4D[] GetNeighbors()
    {
      if (_neighbors != null) return _neighbors;
    
      _neighbors = Enumerable.Range(-1, 3)
        .SelectMany(x => Enumerable.Range(-1, 3)
          .SelectMany(y => Enumerable.Range(-1, 3)
            .SelectMany(z => Enumerable.Range(-1, 3)
              .Select(w => (x, y, z, w)))))
        .Where(coord => coord != (0, 0, 0, 0))
        .Select(coord => new Coordinate4D(coord.x, coord.y, coord.z, coord.w))
        .ToArray();
    
      return _neighbors;
    }
}
