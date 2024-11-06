using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public int unwalkablePenalty;
    Node[,] grid;
    public TerrainType[] walkableRegions;
    public LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    [SerializeField] private bool dDebug;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    // DEBUG
    private int minPenalty = int.MaxValue;
    private int maxPenalty = int.MinValue;
    private Vector3 cube;

    // Start is called before the first frame update
    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        
        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value += region.mask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.mask.value, 2), region.penalty);
        }
        CreateGrid();
        cube = GameObject.Find("Finder").transform.position;
    }

    private void Update()
    {
        Debug.Log(cube);
        Ray ray = new Ray(cube + Vector3.up * 50, Vector3.down);
        Debug.DrawRay(cube + Vector3.up*50, Vector3.down*50);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, walkableMask))
        {
            int penalty;
            Debug.Log(walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out penalty));
            //Debug.Log(penalty);
        }
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    private void BlurPenaltyMap(int blurSize)
    {
        int boxSize = blurSize * 2 + 1;
        int boxExtents = (boxSize - 1) / 2;

        int[ , ] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[ , ] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -boxExtents; x <= boxExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, boxExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - boxExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + boxExtents, 0, gridSizeX - 1);
                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -boxExtents; y <= boxExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, boxExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (boxSize * boxSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - boxExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + boxExtents, 0, gridSizeY - 1);
                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (boxSize * boxSize));
                grid[x, y].movementPenalty = blurredPenalty;

                // DEBUG
                if (blurredPenalty > maxPenalty) maxPenalty = blurredPenalty;
                if (blurredPenalty < minPenalty) minPenalty = blurredPenalty;
            }
        }
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
                int penalty = 0;

                if (!walkable) penalty += unwalkablePenalty;


                if (walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, walkableMask))
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out penalty);
                    }
                }
                grid[x, y] = new Node(walkable, worldPoint, x, y, penalty);
            }
        }
        BlurPenaltyMap(3);
    }

    private void OnDrawGizmos()
    {
        if (!dDebug) return;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(minPenalty, maxPenalty, node.movementPenalty));
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter-.1f));
            }
        }
    }
}

[System.Serializable]
public class TerrainType
{
    public LayerMask mask;
    public int penalty;
}
