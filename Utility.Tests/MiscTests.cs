using System.Collections.Generic;
using System.Linq;
using Utility;
using Xunit;

namespace Utility.Tests;

public class MiscTests
{
  [Fact]
  public void Range_BasicOperations()
  {
    var r = new Range(2, 5);
    Assert.Equal(2, r.Start);
    Assert.Equal(5, r.End);
    Assert.Equal(4, r.Len);
  }

  [Fact]
  public void MultiRange_CoversAndAdd()
  {
    var mr = new MultiRange();
    mr.AddRange(new Range(1,3));
    mr.AddRange(new Range(5,7));
    Assert.True(mr.Contains(2));
    Assert.False(mr.Contains(4));
  }

  [Fact]
  public void DictMultiRange_Basic()
  {
    var dm = new DictMultiRange<string>();
    dm.Ranges["a"] = new Range(1,2);
    dm.Ranges["b"] = new Range(4,5);
    Assert.Equal(2, dm.Ranges.Count);
  }

  [Fact]
  public void ExtendedDictionary_Defaults()
  {
    var ed = new ExtendedDictionary<string,List<int>>();
    ed["x"].Add(3);
    Assert.Single(ed["x"]);
    // accessing missing key creates a new list
    Assert.Empty(ed["missing"]);
  }

  [Fact]
  public void Coordinate2D_Distance_Compare()
  {
    var a = new Coordinate2D(0,0);
    var b = new Coordinate2D(3,4);
    Assert.Equal(5.0, a.EuclideanDistance(b), 6);
  }

  [Fact]
  public void StringMap_BasicIteration()
  {
    var input = "12\n34";
    var sm = new StringMap<int>(input, c => c - '0');
    Assert.Equal(2, sm.Width);
    Assert.Equal(2, sm.Height);
    Assert.Equal(1, sm[new Point2D<int>(0,0)]);
    Assert.True(sm.TryGetValue((1,1), out int v));
    Assert.Equal(4, v);
  }

  [Fact]
  public void ConnectionChain_BuildAndDistance()
  {
    var p1 = new Coordinate2D(0,0);
    var p2 = new Coordinate2D(3,4);
    var conn = new Connection<Coordinate2D>(p1, p2);
    Assert.Equal(5.0, conn.EuclideanDistance, 6);
  }
}
