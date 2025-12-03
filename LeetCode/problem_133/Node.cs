namespace LeetCode.problem_133;

public class Node
{
  public IList<Node> neighbors;
  public int val;

  public Node()
  {
    val = 0;
    neighbors = new List<Node>();
  }

  public Node(int _val)
  {
    val = _val;
    neighbors = new List<Node>();
  }

  public Node(int _val, List<Node> _neighbors)
  {
    val = _val;
    neighbors = _neighbors;
  }
}