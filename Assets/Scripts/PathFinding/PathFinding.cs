using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    NodeGrid grid;
    public bool pathSuccess;

    private void Awake()
    {
       grid = GetComponent<NodeGrid>();
    }

    public static void StartFindPath(Vector3 startPos,Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(targetPos);
        Vector3[] waypoints = new Vector3[0];

        if (startNode.walkable && endNode.walkable) StopCoroutine("FindPath");

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.removeFirst();
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                pathSuccess = true;
                RetracePath(startNode, endNode);
                break;
            }

            foreach(Node neighbor in grid.GetAllNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
        yield return null;
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        // This will give us the shortest path
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector3(path[i - 1].gridX - path[i].gridX,
                                               path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld) waypoints.Add(path[i].worldPos);
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        // This will get the distance from nodeA and nodeB
        // Imagine a rectangle of node with A and B in opposite corners.
        // to get to B, you will get inline with B from A, with the side shorter side
        // Then you will have a straight path to B.

        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return 14*dstY + 10*(dstX - dstY); //Note this is some formula we used to calculate the cost
        return 14*dstX + 10*(dstY - dstX);
    }
}
