using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    // Start is called before the first frame update
    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }


    public List<Node> GetAllNeighbors(Node startNode)
    {
        List<Node> neighbors = new List<Node>();
        for (int x=-1; x<=1; x++)
        {
            for (int y=-1; y<=1; y++)
            {
                if (x == 0 && y == 0) continue; // Just the startNode Pos
                int checkX = startNode.gridX;
                int checkY = startNode.gridY;
                if (checkX+x>=0 && checkX+x<gridSizeX && checkY+y>=0 && checkY+y<gridSizeY) 
                    neighbors.Add(this.grid[checkX+x, checkY+y]);
            }
        }
        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        // Basically a percentage where you at 0.5 is X=0 if gridWorldSize=100 (X=-50,X=50)
        // Similarly for the Z if it was 0.5, then Z=0 if gridWorldSize=100 (Z=-50, Z=50)
        float percentX = (worldPos.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentZ = (worldPos.z + gridWorldSize.y/2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentZ);
        return grid[x, y];
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter-.1f));
            }
        }
    }
}
