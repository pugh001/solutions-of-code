# 207. Course Schedule

There are a total of numCourses courses you have to take, labeled from 0 to numCourses - 1. You are given an array
prerequisites where prerequisites[i] = [ai, bi] indicates that you must take course bi first if you want to take course
ai.

For example, the pair [0, 1], indicates that to take course 0 you have to first take course 1.
Return true if you can finish all courses. Otherwise, return false.

Example 1:

Input: numCourses = 2, prerequisites = [[1,0]]
Output: true
Explanation: There are a total of 2 courses to take.
To take course 1 you should have finished course 0. So it is possible.
Example 2:

Input: numCourses = 2, prerequisites = [[1,0],[0,1]]
Output: false
Explanation: There are a total of 2 courses to take.
To take course 1 you should have finished course 0, and to take course 0 you should also have finished course 1. So it
is impossible.

Constraints:

1 <= numCourses <= 2000
0 <= prerequisites.length <= 5000
prerequisites[i].length == 2
0 <= ai, bi < numCourses
All the pairs prerequisites[i] are unique.

Solution is it's called Khan's Algorithm (for Topological
Sorting) :  https://www.geeksforgeeks.org/topological-sorting-indegree-based-solution/

Algorithm:

Add all nodes with in-degree 0 to a queue.
While the queue is not empty:
Remove a node from the queue.
For each outgoing edge from the removed node, decrement the in-degree of the destination node by 1.
If the in-degree of a destination node becomes 0, add it to the queue.
If the queue is empty and there are still nodes in the graph, the graph contains a cycle and cannot be topologically
sorted.
The nodes in the queue represent the topological ordering of the graph.