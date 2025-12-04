using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_102;

public class Solution
{
  private readonly ITestOutputHelper _testOutputHelper;
  public Solution(ITestOutputHelper testOutputHelper)
  {
    this._testOutputHelper = testOutputHelper;
  }

  [Fact]
  public void Solution_Test1()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int?[] root = [3, 9, 20, null, null, 15, 7];
    IList<IList<int>> expected = new List<IList<int>> { new List<int> { 3 }, new List<int> { 9, 20 }, new List<int> { 15, 7 } };

    // Act
    var tn = CreateTreeNode(root);
    var result = LevelOrder(tn);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void Solution_Test2()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int?[] root = [1];
    IList<IList<int>> expected = new List<IList<int>> { new List<int> { 1 } };

    // Act
    var tn = CreateTreeNode(root);
    var result = LevelOrder(tn);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  [Fact]
  public void Solution_Test3()
  {

    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int?[] root = [];
    IList<IList<int>> expected = [];

    // Act
    var tn = CreateTreeNode(root);
    var result = LevelOrder(tn);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }
  public IList<IList<int>> LevelOrder(TreeNode? root)
  {
    // I guess it is the reverse of my load tree node!!
    var result = new List<IList<int>>();
    if (root == null) return result;

    Queue<TreeNode?> queue = new();
    queue.Enqueue(root);
    //If anything in it queue will be 1 as added root.
    while (queue.Count > 0)
    {
      int startQueueSize = queue.Count;
      List<int> currentLevel = [];
      //Add the value from queue
      for (int i = 0; i < startQueueSize; i++)
      {
        //Read value (gets root node and it's left and right)
        var nodeOn = queue.Dequeue();
        int value = nodeOn.Val;
        currentLevel.Add(value);
        //If this node has a left/right those will be the next levels.
        if (nodeOn.Left != null) queue.Enqueue(nodeOn.Left);
        if (nodeOn.Right != null) queue.Enqueue(nodeOn.Right);
        //If more entires in queue with repeat, but once done this level it
        // will stop, why needed to get count first and not loop while entries.
      }

      //I can now add the level as done and if still levels those are in queue
      result.Add(currentLevel);
    }

    return result;
  }

  private static TreeNode? CreateTreeNode(int?[] nodes)
  {
    if (nodes == null || nodes.Length == 0 || nodes[0] == null)
      return null;

    // Create the root node
    var root = new TreeNode(nodes[0].Value);
    Queue<TreeNode?> queue = new();
    queue.Enqueue(root);

    int i = 1;
    while (i < nodes.Length)
    {
      var current = queue.Dequeue();

      // Add the left child
      if (i < nodes.Length && nodes[i].HasValue)
      {
        current.Left = new TreeNode(nodes[i].Value);
        queue.Enqueue(current.Left);
      }

      i++;

      // Add the right child
      if (i < nodes.Length && nodes[i].HasValue)
      {
        current.Right = new TreeNode(nodes[i].Value);
        queue.Enqueue(current.Right);
      }

      i++;
    }

    return root;
  }
}