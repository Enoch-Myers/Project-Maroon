using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    // Costs for A*
    public int gCost;          // Cost from the start node
    public int hCost;          // Heuristic cost to the end node
    public int fCost => gCost + hCost; // Sum

    // For path reconstruction
    public Node parent;

    public Node(bool walkable, Vector2 worldPos, int gridX, int gridY){
        this.walkable = walkable;
        this.worldPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}

public class AStarGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [Tooltip("Mark real obstacles (e.g., walls) as unwalkable, NOT the floor. The floor should remain walkable.")]
    public LayerMask unwalkableMask;   
    public Vector2 gridWorldSize = new Vector2(10, 10);
    public float nodeRadius = 0.5f;    // Half of the distance between grid nodes

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector2 bottomLeft = (Vector2)transform.position 
                             - Vector2.right * (gridWorldSize.x / 2f)
                             - Vector2.up    * (gridWorldSize.y / 2f);
        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeY; y++){
                Vector2 worldPoint = bottomLeft 
                                     + Vector2.right * (x * nodeDiameter + nodeRadius)
                                     + Vector2.up    * (y * nodeDiameter + nodeRadius);

                // If OverlapCircle detects a collider in unwalkableMask, mark node as not walkable
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x - (transform.position.x - gridWorldSize.x / 2)) / gridWorldSize.x;
        float percentY = (worldPosition.y - (transform.position.y - gridWorldSize.y / 2)) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int x = node.gridX;
        int y = node.gridY;

        if (x - 1 >= 0) neighbors.Add(grid[x - 1, y]);
        if (x + 1 < gridSizeX) neighbors.Add(grid[x + 1, y]);
        if (y - 1 >= 0) neighbors.Add(grid[x, y - 1]);
        if (y + 1 < gridSizeY) neighbors.Add(grid[x, y + 1]);

        return neighbors;
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        if (!startNode.walkable || !targetNode.walkable)
        {
            // If start or end is blocked, no path
            return new List<Vector2>();
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                   (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // If we reach targetNode, build path
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) 
                    continue;

                // Basic cost from current to neighbor
                int newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        // No path found
        return new List<Vector2>();
    }

    List<Vector2> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }
        path.Add(startNode.worldPosition);
        path.Reverse();
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);
        return dstX + dstY; // No diagonal => each step is basically cost 1
    }

    /// <summary>
    /// Debug visualization in the editor.
    /// White = walkable, Red = unwalkable.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.05f));
            }
        }
    }
}
