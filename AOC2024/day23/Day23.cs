using Utility;

namespace AOC2024;

public class Day23
{
  public (string, string) Process(string input)
  {
    string[] data = SetupInputFile.OpenFile(input).ToArray();


    // Parse the connections into a graph representation
    var graph = new Dictionary<string, HashSet<string>>();

    foreach (string? connection in data)
    {
      string[] nodes = connection.Split('-');
      if (!graph.ContainsKey(nodes[0])) graph[nodes[0]] = [];
      if (!graph.ContainsKey(nodes[1])) graph[nodes[1]] = [];

      graph[nodes[0]].Add(nodes[1]);
      graph[nodes[1]].Add(nodes[0]);
    }

    // Find all valid sets of three interconnected computers
    var setsOfThree = FindSetsOfThree(graph);

    // Filter sets where at least one computer's name starts with 't'
    int filteredSets = setsOfThree.Where(set => set.Any(name => name.StartsWith('t'))).ToList().Count;
    var largestClique = FindLargestClique(graph);

    // Sort the clique alphabetically and generate the password
    string password = string.Join(",", largestClique.OrderBy(name => name));

    return ($"{filteredSets}", $"{password}");
  }

  private static List<HashSet<string>> FindSetsOfThree(Dictionary<string, HashSet<string>> graph)
  {
    var result = new HashSet<string>();

    foreach (string node in graph.Keys)
    {
      var neighbors = graph[node].ToList();
      for (int i = 0; i < neighbors.Count; i++)
      {
        for (int j = i + 1; j < neighbors.Count; j++)
        {
          if (!graph[neighbors[i]].Contains(neighbors[j]))
            continue;

          var set = new List<string> { node, neighbors[i], neighbors[j] };
          set.Sort();
          result.Add(string.Join(",", set));
        }
      }
    }

    return result.Select(s => new HashSet<string>(s.Split(','))).ToList();
  }
  private static HashSet<string> FindLargestClique(Dictionary<string, HashSet<string>> graph)
  {
    var largestClique = new HashSet<string>();

    foreach (string? node in graph.Keys)
    {
      var clique = new HashSet<string> { node };
      FindClique(graph, node, clique);

      if (clique.Count > largestClique.Count)
      {
        largestClique = [..clique];
      }
    }

    return largestClique;
  }

  private static void FindClique(Dictionary<string, HashSet<string>> graph, string node, HashSet<string> clique)
  {
    foreach (string neighbor in graph[node].Where(neighbor => clique.All(member => graph[neighbor].Contains(member))))
    {
      clique.Add(neighbor);
      FindClique(graph, neighbor, clique);
    }
  }
}