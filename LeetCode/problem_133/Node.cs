namespace LeetCode.problem_133;

public class Node
{
  public IList<Node> Neighbors;
  public int Val;

  public Node()
  {
    Val = 0;
    Neighbors = new List<Node>();
  }

  public Node(int val)
  {
    Val = val;
    Neighbors = new List<Node>();
  }

  public Node(int val, List<Node> neighbors)
  {
    Val = val;
    Neighbors = neighbors;
  }
}