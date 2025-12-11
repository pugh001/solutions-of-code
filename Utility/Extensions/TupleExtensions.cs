/**
 * Coordinate tuple extensions for basic arithmetic operations
 */

namespace Utility;

public static class TupleExtensions
{
  public static (int x, int y) Add(this (int x, int y) a, (int x, int y) b)
  {
    return (a.x + b.x, a.y + b.y);
  }

  public static (int x, int y, int z) Add(this (int x, int y, int z) a, (int x, int y, int z) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z);
  }

  public static (int x, int y, int z, int w) Add(this (int x, int y, int z, int w) a, (int x, int y, int z, int w) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
  }

  public static (long x, long y) Add(this (long x, long y) a, (long x, long y) b)
  {
    return (a.x + b.x, a.y + b.y);
  }

  public static (long x, long y, long z) Add(this (long x, long y, long z) a, (long x, long y, long z) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z);
  }

  public static (long x, long y, long z, long w) Add(this (long x, long y, long z, long w) a, (long x, long y, long z, long w) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
  }
}