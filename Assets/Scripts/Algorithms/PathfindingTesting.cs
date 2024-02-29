using UnityEngine;
using FahimsUtils;
using System.Collections.Generic;
using System;


public class PathfindingTesting : MonoBehaviour
{

    [Tooltip("The pathfinding grid system")]
    public AStartGridPathfinding gridPathfinding;


    [Tooltip("The size of the grid cell")]
    public float cellSize = 5;

    [Tooltip("The number of columns in the grid")]
    public int columns = 5;

    [Tooltip("The number of rows in the grid")]
    public int rows = 5;


    [Header("Node Prefab")]

    public GameObject nodePrefab;

    [Header("Node List")]
    public List<Node> nodes;

    public Vector2 offset;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public virtual void Start()
    {
        nodes = new();
        gridPathfinding = new(rows, columns, cellSize);
        GenerateGrid();
        InvokeRepeating("DrawGridGizmos", 0.0f, 0.1f);
    }


    /// <summary>
    /// Generates the grid by instantiating the node prefab and setting the Node data.
    /// </summary>
    private void GenerateGrid()
    {
        // Log the length of the grid array to the console
        Debug.Log(gridPathfinding.grid.gridArray.Length);

        // Loop through each row in the grid
        for (int x = 0; x < rows; x++)
        {
            // Loop through each column in the current row
            for (int y = 0; y < columns; y++)
            {
                // Instantiate the node prefab at the current cell position
                var obj = Instantiate(nodePrefab, cellSize * new Vector2(x, y), Quaternion.identity, transform);

                // Set the name of the instantiated object to include the current row and column indices
                obj.name = $"Node {x} {y}";

                // Get the Node component from the instantiated object
                Node node = obj.GetComponent<Node>();

                // Add the node to the nodes list
                nodes.Add(node);

                // Set the model of the node to the corresponding element in the grid array
                node.model = gridPathfinding.grid.gridArray[x, y];

                // Initialize the node
                node.InitializeWithSprites();
            }
        }

        // Loop through each node in the nodes list
        foreach (var node in nodes)
        {
            // Get the 4 neighbours of the current node
            node.Get4Neighbours();
        }
    }


    // /// <summary>
    // /// Update is called every frame, if the MonoBehaviour is enabled.
    // /// </summary>
    // void Update()
    // {
    //     // Check if the left mouse button is clicked
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         // Get the x and y coordinates of the clicked cell
    //         int x, y;
    //         Vector3 location = UtilsClass.GetMouseWorldPosition();
    //         Debug.Log($"Location: {location}");

    //         // Get the x and y coordinates of the clicked cell in the grid
    //         gridPathfinding.grid.GetXY(location, out x, out y);

    //         // Find the path from the starting cell (0, 0) to the clicked cell (x, y)
    //         List<NodeDataModel> path = gridPathfinding.FindPath(0, 0, x, y);

    //         // If a valid path is found and its length is greater than 0
    //         if (path != null && path.Count > 0)
    //         {
    //             // Log the length of the path
    //             Debug.Log($"Path Len: {path.Count}");

    //             // Iterate through the path in reverse order
    //                     DrawPathLines(path);

    //         }
    //     }
    // }


    /// <summary>
    /// Draw Gizmo ever delay seconds
    /// </summary>
    public void DrawGridGizmos()
    {
        for (int x = 0; x < gridPathfinding.grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridPathfinding.grid.gridArray.GetLength(1); y++)
            {
                gridPathfinding.grid.DrawGridLines(gridPathfinding.grid.GetWorldCellPosition(x, y), 0.1f);
            }
        }
    }


    /// <summary>
    /// Retrieves the destination nodes along the path from the start position to the target position.
    /// </summary>
    /// <param name="start">The starting position.</param>
    /// <param name="target">The target position.</param>
    /// <returns>A list of NodeContainer objects representing the destination nodes along the path.</returns>
    public virtual List<NodeContainer> GetDestinationNodes(Vector3 start, Vector3 target)
    {
        // Find the path between the start and target positions using the gridPathfinding object.
        var pathList = gridPathfinding.FindPath(start - (Vector3)offset, target - (Vector3)offset);

        // Create a new list of NodeContainer objects to store the destination nodes.
        List<NodeContainer> nodeQueue = new();

        // Iterate through each node in the pathList.
        foreach (var nodeModel in pathList)
        {
            // Find the corresponding Node object in the nodes list.
            var node = nodes.Find((n) => n.model == nodeModel);

            // If the node is not found, skip to the next node in the pathList.
            if (node == null) continue;

            // Try to get the direction to the next node in the pathList.
            try
            {
                int index = pathList.IndexOf(nodeModel);
                var nextNode = nodes.Find((n) => n.model == pathList[index + 1]);
                NodeContainer container = new(node, node.GetDirectionToNeighbour(nextNode.transform.position));
                nodeQueue.Add(container);
            }
            // If an exception occurs, create a NodeContainer object with the current node and a default direction of up.
            catch (Exception)
            {
                NodeContainer container = new(node, NodeDirectionEnum.up);
                nodeQueue.Add(container);
            }
        }

        // Draw the path lines between the nodes.
        DrawPathLines(pathList);

        // Return the list of NodeContainer objects representing the destination nodes along the path.
        return nodeQueue;
    }




    public void DrawPathLines(List<NodeDataModel> path)
    {
        for (int i = path.Count - 1; i > 0; i--)
        {
            try
            {
                // Log the x and y coordinates of the current cell
                Debug.Log($"Path X: {path[i].XPosition}, Path Y: {path[i].YPosition}");

                // Log the x and y coordinates of the previous cell
                Debug.Log($"Path Prev X: {path[i].previousNode.XPosition}, Path Prev  Y: {path[i].previousNode.YPosition}");

                // Draw a line between the current cell and the previous cell
                Debug.DrawLine(gridPathfinding.grid.GetWorldCellPosition(path[i].XPosition, path[i].YPosition), gridPathfinding.grid.GetWorldCellPosition(path[i].previousNode.XPosition, path[i].previousNode.YPosition), Color.green, 1);
            }
            catch (Exception)
            {
                // If an exception occurs, ignore it
            }
        }
    }
}
