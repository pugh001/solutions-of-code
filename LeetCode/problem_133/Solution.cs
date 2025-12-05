using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_133;

public class Solution
{
  private readonly ITestOutputHelper _testOutputHelper;


  private readonly Dictionary<Node, Node> _visited = new();
  public Solution(ITestOutputHelper testOutputHelper)
  {
    _testOutputHelper = testOutputHelper;
  }
  [Fact]
  public void Solution_Test1()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] nodeList = [[2, 4], [1, 3], [2, 4], [1, 3]];
    var adjList = CreateNodes(nodeList);
    var expected = CreateNodes(nodeList);

    var result = CloneGraph(adjList);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    AssertDeepClone(expected, result);
  }


  [Fact]
  public void Solution_Test2()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] nodeList = [[]];
    var adjList = CreateNodes(nodeList);
    var expected = CreateNodes(nodeList);

    var result = CloneGraph(adjList);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    AssertDeepClone(expected, result);
  }

  [Fact]
  public void Solution_Test3()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int[][] nodeList = [];
    var adjList = CreateNodes(nodeList);
    var expected = CreateNodes(nodeList);

    var result = CloneGraph(adjList);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    AssertDeepClone(expected, result);
  }

  public Node CloneGraph(Node node)
  {
    if (node == null) return node;

    //If there is nodes need to read first and keep looping until all visited.
    return CloneNode(node);
  }
  // Helper function to perform DFS and clone the graph but inside so can use 
  private Node CloneNode(Node n)
  {
    if (_visited.ContainsKey(n))
    {
      return _visited[n]; // Return the already cloned node
    }

    // Clone the node
    var clone = new Node(n.Val);
    _visited[n] = clone;

    // Clone all neighbors
    foreach (var neighbor in n.Neighbors)
    {
      clone.Neighbors.Add(CloneNode(neighbor));
    }

    return clone;
  }

  private Node CreateNodes(int[][] input)
  {
    if (input == null || input.Length == 0)
    {
      return null;
    }

    // Dictionary to map values to created nodes
    Dictionary<int, Node> nodeMap = new();

    // Create all nodes first
    for (int i = 0; i < input.Length; i++)
    {
      if (!nodeMap.ContainsKey(i + 1))
      {
        nodeMap[i + 1] = new Node(i + 1);
      }
    }

    // Add neighbors to each node
    for (int i = 0; i < input.Length; i++)
    {
      var currentNode = nodeMap[i + 1];
      foreach (int neighborVal in input[i])
      {
        if (!nodeMap.TryGetValue(neighborVal, out var value))
        {
          value = new Node(neighborVal);
          nodeMap[neighborVal] = value;
        }

        currentNode.Neighbors.Add(value);
      }
    }

    // Return the first node (Node with val = 1)
    return nodeMap[1];
  }

  private void AssertDeepClone(Node original, Node clone)
  {
    if (original == null && clone == null) return;

    Assert.NotNull(original);
    Assert.NotNull(clone);

    // Use sets to track visited nodes
    HashSet<Node> visitedOriginal = new();
    HashSet<Node> visitedClone = new();

    void AreGraphsEqual(Node originalNode, Node cloneNode)
    {
      if (visitedOriginal.Contains(originalNode)) return;

      // Add to visited sets
      visitedOriginal.Add(originalNode);
      visitedClone.Add(cloneNode);

      // Check value equality
      Assert.Equal(originalNode.Val, cloneNode.Val);

      // Check neighbors count
      Assert.Equal(originalNode.Neighbors.Count, cloneNode.Neighbors.Count);

      // Check neighbors recursively
      for (int i = 0; i < originalNode.Neighbors.Count; i++)
      {
        var origNeighbor = originalNode.Neighbors[i];
        var cloneNeighbor = cloneNode.Neighbors[i];

        // Ensure no shared references
        Assert.NotSame(origNeighbor, cloneNeighbor);

        AreGraphsEqual(origNeighbor, cloneNeighbor);
      }
    }

    AreGraphsEqual(original, clone);
  }
}