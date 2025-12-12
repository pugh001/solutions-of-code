using System.Collections.Generic;
using Utility;
using Xunit;

namespace Utility.Tests;

public class GraphTests
{
  [Fact]
  public void BuildGraph_FindAllPaths_CountPaths_Works()
  {
    var nodes = new[] {"A","B","C","D"};
    var rules = new[] { ("A","B"), ("A","C"), ("B","D"), ("C","D") };
    var graph = Graph.BuildGraph(nodes, rules);

    var allPaths = Graph.FindAllPaths(graph, "A", "D");
    Assert.Contains(allPaths, p => p.Count == 3 && p[0] == "A" && p[^1] == "D");
    Assert.Equal(2, allPaths.Count);

    int count = Graph.CountPaths(graph, "A", "D");
    Assert.Equal(2, count);
  }

  [Fact]
  public void TopologicalSort_And_Traversals()
  {
    var nodes = new[] {1,2,3,4};
    var graph = new Dictionary<int,List<int>>
    {
      [1] = new List<int>{2,3},
      [2] = new List<int>{4},
      [3] = new List<int>{4},
      [4] = new List<int>()
    };

    var topo = Graph.TopologicalSort(graph, nodes);
    Assert.Equal(4, topo.Count);

    var dfs = Graph.DepthFirstSearch(graph, 1);
    Assert.Contains(4, dfs);

    var bfs = Graph.BreadthFirstSearch(graph, 1);
    Assert.Equal(dfs.Count, bfs.Count);
  }

  [Fact]
  public void HasCycle_Detects_Cycle()
  {
    var graph = new Dictionary<string, List<string>>
    {
      ["a"] = new List<string>{"b"},
      ["b"] = new List<string>{"c"},
      ["c"] = new List<string>{"a"}
    };

    Assert.True(Graph.HasCycle(graph));
  }
}
