using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extensions;

/// <summary>
/// Utility methods for graph operations and algorithms
/// </summary>
public static class GraphUtilities
{
    /// <summary>
    /// Builds a graph from a list of nodes and dependency rules
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="nodes">Collection of nodes to include in graph</param>
    /// <param name="rules">Dependency rules as (parent, child) tuples</param>
    /// <returns>Dictionary representing adjacency list graph structure</returns>
    public static Dictionary<T, List<T>> BuildGraph<T>(IEnumerable<T> nodes, IEnumerable<(T X, T Y)> rules) where T : notnull
    {
        var graph = new Dictionary<T, List<T>>();
        
        // Initialize all nodes with empty adjacency lists
        foreach (var node in nodes)
        {
            graph[node] = new List<T>();
        }
        
        // Add edges based on rules
        foreach (var (x, y) in rules)
        {
            if (graph.ContainsKey(x) && graph.ContainsKey(y))
            {
                graph[x].Add(y);
            }
        }
        
        return graph;
    }

    /// <summary>
    /// Performs topological sort on a graph using Kahn's algorithm
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <param name="nodes">Collection of nodes to sort</param>
    /// <returns>Topologically sorted list of nodes</returns>
    public static List<T> TopologicalSort<T>(Dictionary<T, List<T>> graph, IEnumerable<T> nodes) where T : notnull
    {
        var result = new List<T>();
        var inDegree = new Dictionary<T, int>();
        var queue = new Queue<T>();
        
        // Calculate in-degrees
        foreach (var node in nodes)
        {
            inDegree[node] = 0;
        }
        
        foreach (var kvp in graph)
        {
            foreach (var neighbor in kvp.Value)
            {
                if (inDegree.ContainsKey(neighbor))
                {
                    inDegree[neighbor]++;
                }
            }
        }
        
        // Find all nodes with no incoming edges
        foreach (var kvp in inDegree)
        {
            if (kvp.Value == 0)
            {
                queue.Enqueue(kvp.Key);
            }
        }
        
        // Process nodes
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            
            if (graph.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (inDegree.ContainsKey(neighbor))
                    {
                        inDegree[neighbor]--;
                        if (inDegree[neighbor] == 0)
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// Performs depth-first search traversal
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <param name="start">Starting node for traversal</param>
    /// <param name="visited">Optional set to track visited nodes</param>
    /// <returns>List of nodes visited during DFS</returns>
    public static List<T> DepthFirstSearch<T>(Dictionary<T, List<T>> graph, T start, HashSet<T>? visited = null) where T : notnull
    {
        visited ??= new HashSet<T>();
        var result = new List<T>();
        var stack = new Stack<T>();
        
        stack.Push(start);
        
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            
            if (!visited.Add(current))
                continue;
                
            result.Add(current);
            
            if (graph.TryGetValue(current, out var neighbors))
            {
                // Push in reverse order to maintain left-to-right traversal
                for (int i = neighbors.Count - 1; i >= 0; i--)
                {
                    if (!visited.Contains(neighbors[i]))
                    {
                        stack.Push(neighbors[i]);
                    }
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// Performs breadth-first search traversal
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <param name="start">Starting node for traversal</param>
    /// <returns>List of nodes visited during BFS</returns>
    public static List<T> BreadthFirstSearch<T>(Dictionary<T, List<T>> graph, T start) where T : notnull
    {
        var visited = new HashSet<T>();
        var result = new List<T>();
        var queue = new Queue<T>();
        
        queue.Enqueue(start);
        visited.Add(start);
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            
            if (graph.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (visited.Add(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// Checks if the graph contains cycles using DFS
    /// </summary>
    /// <typeparam name="T">Type of graph nodes</typeparam>
    /// <param name="graph">Graph as adjacency list</param>
    /// <returns>True if graph contains cycles, false otherwise</returns>
    public static bool HasCycle<T>(Dictionary<T, List<T>> graph) where T : notnull
    {
        var visited = new HashSet<T>();
        var recursionStack = new HashSet<T>();
        
        foreach (var node in graph.Keys)
        {
            if (!visited.Contains(node))
            {
                if (HasCycleDFS(graph, node, visited, recursionStack))
                    return true;
            }
        }
        
        return false;
    }
    
    private static bool HasCycleDFS<T>(Dictionary<T, List<T>> graph, T node, HashSet<T> visited, HashSet<T> recursionStack) where T : notnull
    {
        visited.Add(node);
        recursionStack.Add(node);
        
        if (graph.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    if (HasCycleDFS(graph, neighbor, visited, recursionStack))
                        return true;
                }
                else if (recursionStack.Contains(neighbor))
                {
                    return true;
                }
            }
        }
        
        recursionStack.Remove(node);
        return false;
    }
}
