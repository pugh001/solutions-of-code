using System;
using System.Collections.Generic;
using System.Linq;
using Utility;
using Xunit;

namespace Utility.Tests;

public class MorePublicApiTests
{
  [Fact]
  public void Parsing_ExtractAndConvert()
  {
    var ints = Parsing.ExtractIntegers("a -12 b 34 c 5");
    Assert.Equal(new List<int>{-12,34,5}, ints);

    var longs = Parsing.ExtractLongs("1 2 3");
    Assert.Equal(new List<long>{1,2,3}, longs);

    Assert.True(Parsing.TryExtractFirstLong("xxx 42 y", out long v));
    Assert.Equal(42, v);

    var nums = Parsing.ExtractNumbers("1.5 -2 3.25");
    Assert.Equal(3, nums.Count);

    Assert.Equal(5L, Parsing.BinaryToLong("00101"));
    Assert.Equal(255L, Parsing.HexToLong("FF"));
    Assert.Equal(10, Parsing.CharToDigit('a', 16));

    var freq = Parsing.CountFrequencies(new[] {"a","b","a"});
    Assert.Equal(2, freq["a"]);

    var rotated = Parsing.RotateClockwise(new[] {"ab","cd"});
    Assert.Equal(new[] {"ca","db"}, rotated);
  }

  [Fact]
  public void StringConversions_Basics()
  {
    var l = "123".ToIntList();
    Assert.Equal(new List<int>{1,2,3}, l);

    var arr = new[] {"1","2"}.ToIntArray();
    Assert.Equal(new[]{1,2}, arr);

    var pos = "a12b34".ExtractPosInts().ToList();
    Assert.Equal(new List<int>{12,34}, pos);

    var words = "abc 123 def".ExtractWords().ToList();
    Assert.Contains("abc", words);

    Assert.Equal("1010", "A".HexStringToBinary().Substring(0,4));
  }

  [Fact]
  public void Grid_Basics()
  {
    var lines = new[] {"ab","cd"};
    var g = new Grid(lines);
    Assert.Equal(2, g.Rows);
    Assert.Equal(2, g.Cols);
    Assert.Equal('a', g[0,0]);
    Assert.Equal('d', g[new Point2D<int>(1,1)]);
    Assert.True(g.IsInBounds(0,1));
    var all = g.FindAll('a');
    Assert.Single(all);
    Assert.Equal("ab", g.GetRow(0));
    Assert.Equal(lines, g.ToStringArray());
    var raw = g.GetRawGrid();
    Assert.Equal('c', raw[1,0]);
    var unique = g.GetUniqueCharacters();
    Assert.Contains('a', unique);
    Assert.Equal(1, g.Count('a'));
  }

  [Fact]
  public void GridSimulation_RunSimulation_Count()
  {
    var lines = new[] {"@@","@@"};
    var g = new Grid(lines);
    // remove all '@' unconditionally
    var res = GridSimulation.RunSimulation(g, p => true, '@', '.');
    Assert.Equal((4,4), res);

    // test neighbor count helper
    var grid = new Grid(new[] {"@.@","...","@.@"});
    int cnt = GridSimulation.CountNeighborsOfType(grid, (1,1), '@', true);
    Assert.Equal(4, cnt);
  }

  [Fact]
  public void LightGrid_Operations()
  {
    var lg = new LightGrid(10);
    lg.ApplyInstruction("turn on 0,0 through 1,1");
    Assert.True(lg.IsLightOn(0,0));
    Assert.Equal(4, lg.CountLightsOn());
    lg.ApplyInstruction("toggle 0,0 through 0,0");
    Assert.False(lg.IsLightOn(0,0));
    Assert.True(lg.GetBrightness(1,1) > 0);
  }

  [Fact]
  public void MathGrid_Apply()
  {
    var grid = new List<List<string>>
    {
      new List<string>{"1","2"},
      new List<string>{"+","+"}
    };

    var total = MathGrid.CalculateColumnResults(grid);
    Assert.Equal(3, total);

    var lines = new List<string>{" 1 2 ", "+ +" };
    var boundaries = MathGrid.FindProblemBoundaries(lines);
    Assert.True(boundaries.Count >= 2);
  }

  [Fact]
  public void GraphUtilities_Topological()
  {
    var nodes = new List<int>{1,2,3,4};
    var graph = GraphUtilities.BuildGraph(nodes, new List<(int,int)>{(1,2),(1,3),(2,4),(3,4)});
    var sorted = GraphUtilities.TopologicalSort(graph, nodes);
    Assert.Equal(4, sorted.Count);
  }

  [Fact]
  public void MoveDirections_Basics()
  {
    Assert.Equal(CompassDirection.N, MoveDirections.CompassDirectionFromArrow('^'));
    var a = (0,0).MoveDirection(CompassDirection.E);
    Assert.Equal((1,0), a);
    var c = new Coordinate2D(0,0).MoveDirection(CompassDirection.N);
    Assert.Equal(new Coordinate2D(0,1), c);
  }

  [Fact]
  public void Rectangle_And_Polygon()
  {
    var r = new Rectangle<int>(0,1,0,1);
    Assert.True(r.Contains(new Point2D<int>(0,0)));
    Assert.Equal(4, r.Area);

    var poly = new List<Point2D<int>>{(0,0),(2,0),(2,2),(0,2)};
    Assert.True(PolygonUtility.IsPointInOrOnRectilinearPolygon((1,1), poly));
    Assert.True(PolygonUtility.IsPointOnPolygonBoundary((0,1), poly));
    Assert.False(PolygonUtility.DoesRectilinearEdgeIntersectRectangleInterior((0,0),(0,3), new Rectangle<int>(1,2,1,2)));
  }
}
