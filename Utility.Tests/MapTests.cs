using System;
using System.Collections.Generic;
using Utility;
using Xunit;

namespace Utility.Tests;

public class MapTests
{
  [Fact]
  public void GetDirectionPositions_And_Values_Works()
  {
    var lines = new List<string>
    {
      "abc",
      "dSe",
      "fEg"
    };

    var map = new Map(lines);

    // center at S (row 1, col 1)
    var pos = new[] {1,1};
    var neighbors4 = map.GetDirectionPositions(pos, "4");
    Assert.Contains(neighbors4, p => p[0] == 0 && p[1] == 1); // up
    Assert.Contains(neighbors4, p => p[0] == 2 && p[1] == 1); // down
    Assert.Contains(neighbors4, p => p[0] == 1 && p[1] == 0); // left
    Assert.Contains(neighbors4, p => p[0] == 1 && p[1] == 2); // right

    var values = map.GetDirectionValues(pos, "4");
    Assert.Contains('b', values);
    Assert.Contains('E', values);
    Assert.Contains('d', values);
    Assert.Contains('e', values);
  }

  [Fact]
  public void FindAll_OnBoard_GetRow_Column_Values()
  {
    var lines = new List<string>
    {
      "12",
      "34"
    };

    var map = new Map(lines);
    Assert.Equal(2, map.Rows);
    Assert.Equal(2, map.Columns);

    var col0 = map.GetColumnValues(0);
    Assert.Equal(new List<char>{'1','3'}, col0);

    var row1 = map.GetRowValues(1);
    Assert.Equal(new List<char>{'3','4'}, row1);

    var found = map.FindAll('3');
    Assert.Single(found);
    Assert.Equal(1, found[0][0]);
    Assert.Equal(0, found[0][1]);
  }
}
