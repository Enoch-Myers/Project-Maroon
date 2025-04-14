using System.Collections.Generic;
using UnityEngine;

// A single square in the grid that we use for pathfinding
public class Node
{
    public bool walkable;                  // Can we walk on this node or is it blocked?
    public Vector2 worldPosition;          // The position in the world
    public int gridX, gridY;               // Grid position (column and row)

    // A* cost values
    public int gCost;                      // Cost from the start node
    public int hCost;                      // Estimated cost to the goal
    public int fCost => gCost + hCost;     // Total cost (used for picking best node)

    public Node parent;                    // Used to retrace the final path

    // Constructor for setting up a new node
    public Node(bool walkable, Vector2 worldPos, int gridX, int gridY){
        this.walkable = walkable;
        this.worldPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}

// The grid manager and A* pathfinding logic
public class AStarGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public LayerMask unwalkableMask;          // What counts as an obstacle
    public Vector2 gridWorldSize = new Vector2(10, 10); // Size of the grid in world units
    public float nodeRadius = 0.5f;           // Half the size of a node

    private Node[,] grid;                     // 2D array to hold all the nodes
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;                                // Full size of one node
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); // How many columns
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); // How many rows

        CreateGrid(); // Build the grid at the start
    }

    // Build the actual grid by filling in each node
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        // Find bottom-left corner of the grid
        Vector2 bottomLeft = (Vector2)transform.position 
                             - Vector2.right * (gridWorldSize.x / 2f)
                             - Vector2.up    * (gridWorldSize.y / 2f);

        // Loop through all grid positions and place a node
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                Vector2 worldPoint = bottomLeft 
                                     + Vector2.right * (x * nodeDiameter + nodeRadius)
                                     + Vector2.up    * (y * nodeDiameter + nodeRadius);

                // Check if this spot is walkable
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // Get the node in the grid that matches a given world position
    Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        // Convert world position to percentage across grid
        float percentX = (worldPosition.x - (transform.position.x - gridWorldSize.x / 2)) / gridWorldSize.x;
        float percentY = (worldPosition.y - (transform.position.y - gridWorldSize.y / 2)) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX); // Ensure it's between 0 and 1
        percentY = Mathf.Clamp01(percentY);

        // Convert percentage into grid index
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    // Get the 4-direction neighbors (up, down, left, right)
    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int x = node.gridX;
        int y = node.gridY;

        // Check if neighbor exists (inside bounds)
        if(x - 1 >= 0) neighbors.Add(grid[x - 1, y]);
        if(x + 1 < gridSizeX) neighbors.Add(grid[x + 1, y]);
        if(y - 1 >= 0) neighbors.Add(grid[x, y - 1]);
        if(y + 1 < gridSizeY) neighbors.Add(grid[x, y + 1]);

        return neighbors;
    }

    // The main A* pathfinding function
    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        if(!startNode.walkable || !targetNode.walkable){
            // Can't find a path if either start or end is blocked
            return new List<Vector2>();
        }

        List<Node> openSet = new List<Node>();     // Nodes we need to check
        HashSet<Node> closedSet = new HashSet<Node>(); // Nodes we've already checked
        openSet.Add(startNode);

        while(openSet.Count > 0){
            // Find node in openSet with the lowest fCost
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++){
                if(openSet[i].fCost < currentNode.fCost ||
                  (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)){
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode){
                // Found the path!
                return RetracePath(startNode, targetNode);
            }

            foreach(Node neighbor in GetNeighbors(currentNode)){
                // Skip if the node is blocked or already checked
                if(!neighbor.walkable || closedSet.Contains(neighbor))continue;

                int newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if(newGCost < neighbor.gCost || !openSet.Contains(neighbor)){
                    // Update neighbor if this path is better
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if(!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
        // No path found
        return new List<Vector2>();
    }

    // Go backward from end node to start to get the full path
    List<Vector2> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = endNode;

        while(currentNode != startNode){
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }

        path.Add(startNode.worldPosition);
        path.Reverse(); // Put path in correct order
        return path;
    }

    // Manhattan Distance (only vertical and horizontal) since we only move in 4 directions
    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);
        return dstX + dstY;
    }

    // Visualize the grid in the Unity editor DOESN'T WORK
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