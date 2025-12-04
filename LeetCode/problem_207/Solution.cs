using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace LeetCode.problem_207;

public class Solution
{

  private static Queue<int> _courseQueue = null!;
  private static Dictionary<int, List<int>> _courseGraph = null!;
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
    int numCourses = 2;
    int[][] prerequisites = [[1, 0]];
    bool expected = true;

    // Act
    bool result = CanFinish(numCourses, prerequisites);

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
    int numCourses = 2;
    int[][] prerequisites = [[1, 0], [0, 1]];
    bool expected = false;

    // Act
    bool result = CanFinish(numCourses, prerequisites);

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
    int numCourses = 1;
    int[][] prerequisites = [];
    bool expected = true;

    // Act
    bool result = CanFinish(numCourses, prerequisites);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }


  [Fact]
  public void Solution_Test4()
  {
    var stopWatch = Stopwatch.StartNew();
    // Arrange
    int numCourses = 5;
    int[][] prerequisites = [[1, 4], [2, 4], [3, 1], [3, 2]];
    bool expected = true;

    // Act
    bool result = CanFinish(numCourses, prerequisites);

    stopWatch.Stop();
    _testOutputHelper.WriteLine($"  Time:  {stopWatch.Elapsed}");
    // Assert

    Assert.Equal(expected, result);
  }

  public bool CanFinish(int numCourses, int[][] prerequisites)
  {

    if (prerequisites.Length == 0) return true;

    _courseQueue = new Queue<int>();
    _courseGraph = new Dictionary<int, List<int>>();

    // Step 1: Build the graph and calculate in-degrees
    int[] inDegree = new int[numCourses];
    inDegree = BuildGraphCalculateInDegree(prerequisites, inDegree);

    // Step 2: Find all nodes with in-degree 0

    BuildQueue(numCourses, inDegree);

    // Step 3: Process the nodes
    int coursesTaken = ProcessCoursesTaken(inDegree);

    // Step 4: Check if all courses can be taken
    return coursesTaken == numCourses;
  }
  private static int ProcessCoursesTaken(int[] inDegree)
  {

    int coursesTaken = 0;
    while (_courseQueue.Count > 0)
    {
      int current = _courseQueue.Dequeue();
      coursesTaken++;

      if (!_courseGraph.TryGetValue(current, out var value))
        continue;

      foreach (int neighbor in value)
      {
        inDegree[neighbor]--;
        if (inDegree[neighbor] == 0)
        {
          _courseQueue.Enqueue(neighbor);
        }
      }
    }

    return coursesTaken;
  }

  private static void BuildQueue(int numCourses, int[] inDegree)
  {

    for (int i = 0; i < numCourses; i++)
    {
      if (inDegree[i] == 0)
      {
        _courseQueue.Enqueue(i);
      }
    }

  }

  private static int[] BuildGraphCalculateInDegree(int[][] prerequisites, int[] inDegree)
  {
    foreach (int[] prerequisite in prerequisites)
    {
      int course = prerequisite[0];
      int pre = prerequisite[1];

      if (!_courseGraph.TryGetValue(pre, out var value))
      {
        value = [];
        _courseGraph[pre] = value;
      }

      value.Add(course);
      inDegree[course]++;
    }

    return inDegree;
  }
}