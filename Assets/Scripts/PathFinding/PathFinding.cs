using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    public Transform seeker, target;
    NodeGrid grid;

    private void Awake()
    {
       grid = GetComponent<NodeGrid>();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(targetPos);
        
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.removeFirst();
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
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
    }

    void RetracePath(Node startNode, Node endNode)
    {
        // This will give us the shortest path
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
        return;
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
