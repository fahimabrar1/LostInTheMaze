using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStartGridPathfinding
{
    /// <href ="https://www.geeksforgeeks.org/a-search-algorithm/">
    public const int MOVE_STRAIGHT_COST = 10;

    public const int MOVE_DIAGONAL_COST = 20;


    [Tooltip("Nodes queued up for searching")]
    public List<NodeDataModel> openList;

    [Tooltip("Nodes that have alreadty been searched")]
    public List<NodeDataModel> closedList;

    /// <summary>
    /// The Grid system
    /// </summary>
    public GridGeneric<NodeDataModel> grid;

    public AStartGridPathfinding(int rows, int column, float cellSize, bool autoInitialize = false)
    {
        if (autoInitialize)
            grid = new(rows, column, cellSize, (x, y) => InitializeNodeData(x, y, cellSize));
        else
            grid = new(rows, column, cellSize);
    }



    /// <summary>
    /// Initialzies the row/col number with the walkable balue
    /// </summary>
    /// <param name="x">The x row on the grid</param>
    /// <param name="y">The y col on the grid</param>
    /// <returns>The node Data</returns>
    private NodeDataModel InitializeNodeData(int x, int y, float cellSize)
    {
        return new NodeDataModel(x, y, cellSize, (x == 0 && y == 0) || RandomWalkProbability());
    }


    /// <summary>
    /// Sets the probaility of the random ness, keeping mostly walkable
    /// </summary>
    /// <returns>isWalkable or not</returns>
    private bool RandomWalkProbability()
    {

        // Adjust the probability as needed
        const double probabilityOfWalking = 0.7; // 70% probability of walking (true)

        // Generate a random number between 0 and 1
        float randomNumber = UnityEngine.Random.Range(0f, 1f);

        // Return true if the random number is less than the probability of walking
        return randomNumber < probabilityOfWalking;
    }

    /// <summary>
    /// Finds the path between two points in a grid-based environment.
    /// </summary>
    /// <param name="start">The starting point for the path.</param>
    /// <param name="end">The destination point for the path.</param>
    /// <returns>A list of NodeDataModel containing the path between start and end.</returns>
    public List<NodeDataModel> FindPath(Vector3 start, Vector3 end)
    {
        int x, x1, y, y1; // Variables to store the grid coordinates

        // Get the grid coordinates for the start and end points
        grid.GetXY(start, out x, out y);
        grid.GetXY(end, out x1, out y1);

        // Call the overload that directly uses the grid coordinates
        return FindPath(x, y, x1, y1);
    }



    /// <summary>
    /// The Algorithm that finds the shortest path from start to end
    /// </summary>
    /// <param name="startX">The starting x point of the node</param>
    /// <param name="startY">The starting y point of the node</param>
    /// <param name="endX">The end x point of the node</param>
    /// <param name="endY">The end y point of the node</param>
    /// <returns></returns>
    public List<NodeDataModel> FindPath(int startX, int startY, int endX, int endY)
    {
        // Initialize closed list
        closedList = new();

        Debug.Log($"Start Coordinate:  X: {startX}, Y: {startY}");
        Debug.Log($"End Coordinate:  X: {endX}, Y: {endY}");
        Debug.Log($"End Coordinate:  X: {grid.gridArray[endX, endY]}");


        NodeDataModel startNode = grid.GetValue(startX, startY);
        NodeDataModel endNode = grid.GetValue(endX, endY);


        Debug.Log($"Start Node: {startNode}, Column: {startNode.Column}, Row: {startNode.Row}");
        Debug.Log($"End Node: {endNode}, Column: {endNode.Column}, Row: {endNode.Row}");
        // Initialize open list with start node
        openList = new() { startNode };

        // Set default GCost, HCost, and previousNode for all nodes
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                NodeDataModel node = grid.GetValue(x, y);
                // Debug.Log($"THE NODE: {node}, X: {x}, Y: {y}");
                node.GCost = int.MaxValue;
                node.CalculateFCost();
                node.previousNode = null;
            }
        }

        // Initialize start node's GCost and HCost
        startNode.GCost = 0;
        startNode.HCost = CalculateDiagonalDistanceHeuristicCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            // Get the node with the lowest FCost
            NodeDataModel currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                // Finish Search
                return CalculatePath(endNode);
            }

            // Remove the current node from the open list and add it to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Get the neighbors of the current node
            List<NodeDataModel> neighbours = Get4DirectionalNeighbours(currentNode);

            foreach (NodeDataModel neighbour in neighbours)
            {
                if (neighbour == null) continue;

                if (!neighbour.isWalkable)
                {
                    // Ignore neighbor if it's in the closed list
                    if (!closedList.Contains(neighbour))
                    {
                        closedList.Add(neighbour);
                    }
                    continue;
                }
                else if (closedList.Contains(neighbour))
                {
                    continue;
                }

                // Calculate new GCost for the neighbor
                int gCost = currentNode.GCost + CalculateEuclideanlDistanceHeuristicCost(currentNode, neighbour);

                if (gCost < neighbour.GCost)
                {
                    // Update neighbor's previous node, GCost, HCost, and FCost
                    neighbour.previousNode = currentNode;
                    neighbour.GCost = gCost;
                    neighbour.HCost = Calculate1ManhattanDistanceHeuristicCost(currentNode, neighbour);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                    {
                        // Add the neighbor to the open list
                        openList.Add(neighbour);
                    }
                }
            }
        }

        Debug.LogWarning("No Path Found");

        return null;
    }


    /// <summary>
    /// Gets the 8 direcitonal neighbour of a node in the grid
    /// </summary>
    /// <param name="currentNode">The inital from of thich the surrounding  nodes will be gotten</param>
    /// <returns>The 8 neighbour or successor</returns>
    private List<NodeDataModel> Get8DirectionalNeighbours(NodeDataModel currentNode)
    {
        List<NodeDataModel> neighboursList = new();
        if (currentNode.Column - 1 >= 0)
        {
            // Left Node
            neighboursList.Add(GetNode(currentNode.Column - 1, currentNode.Row));
            // Left Down
            if (currentNode.Row - 1 >= 0) neighboursList.Add(GetNode(currentNode.Column - 1, currentNode.Row - 1));
            // Left Up
            if (currentNode.Row + 1 >= 0) neighboursList.Add(GetNode(currentNode.Column - 1, currentNode.Row + 1));
        }

        if (currentNode.Column + 1 < grid.GetWidth())
        {
            // Right Node
            neighboursList.Add(GetNode(currentNode.Column + 1, currentNode.Row));
            // Right Down
            if (currentNode.Row - 1 >= 0) neighboursList.Add(GetNode(currentNode.Column + 1, currentNode.Row - 1));
            // Right Up
            if (currentNode.Row + 1 >= 0) neighboursList.Add(GetNode(currentNode.Column + 1, currentNode.Row + 1));
        }

        // Up
        if (currentNode.Row + 1 < grid.Getheight()) neighboursList.Add(GetNode(currentNode.Column, currentNode.Row + 1));

        // Down
        if (currentNode.Row - 1 >= 0) neighboursList.Add(GetNode(currentNode.Column, currentNode.Row - 1));


        Debug.Log($"Found: {neighboursList.Count} Nodes");
        return neighboursList;
    }


    /// <summary>
    /// Gets the 8 direcitonal neighbour of a node in the grid
    /// </summary>
    /// <param name="currentNode">The inital from of thich the surrounding  nodes will be gotten</param>
    /// <returns>The 8 neighbour or successor</returns>
    private List<NodeDataModel> Get4DirectionalNeighbours(NodeDataModel currentNode)
    {
        List<NodeDataModel> neighboursList = new();
        // Left Node
        if (currentNode.Column - 1 >= 0)
            neighboursList.Add(GetNode(currentNode.Column - 1, currentNode.Row));


        // Right Node
        if (currentNode.Column + 1 < grid.GetWidth())
            neighboursList.Add(GetNode(currentNode.Column + 1, currentNode.Row));

        // Up Node
        if (currentNode.Row + 1 < grid.Getheight()) neighboursList.Add(GetNode(currentNode.Column, currentNode.Row + 1));

        // Down Node
        if (currentNode.Row - 1 >= 0)
            neighboursList.Add(GetNode(currentNode.Column, currentNode.Row - 1));


        // Debug.Log($"Found: {neighboursList.Count} Nodes");
        return neighboursList;
    }


    /// <summary>
    /// Calculates the final path tracing back to starting node
    /// </summary>
    /// <param name="endNode">the final destination node</param>
    /// <returns>the path node list</returns>
    private List<NodeDataModel> CalculatePath(NodeDataModel endNode)
    {
        // Initialize a new list of NodeDataModel objects to store the path
        List<NodeDataModel> path = new()
    {
        endNode
    };

        // Initialize a variable to store the current node
        NodeDataModel currentNode = endNode;

        // Iterate through the nodes until the starting node is reached
        while (currentNode != null)
        {
            // Add the previous node to the path list
            path.Add(currentNode.previousNode);

            // Update the current node to the previous node
            currentNode = currentNode.previousNode;
        }

        // Reverse the path list to get the path from starting node to end node
        path.Reverse();
        Debug.Log($"Path Length: {path.Count}");
        // Return the calculated path
        return path;
    }


    /// <summary>
    /// Gets the node from the grid bvy value
    /// </summary>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    /// <returns></returns>
    public NodeDataModel GetNode(int x, int y)
    {
        return grid.GetValue(x, y);
    }




    /// <summary>
    /// This uses the formula:
    /// h = abs (current_cell.x – goal.x) + abs (current_cell.y – goal.y)
    /// </summary>
    /// <param name="current"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private int Calculate1ManhattanDistanceHeuristicCost(NodeDataModel current, NodeDataModel goal)
    {
        return Mathf.RoundToInt(Mathf.Abs(current.Column - goal.Column) + Mathf.Abs(current.Row - goal.Row));
    }



    /// <summary>
    /// This uses the formula:
    /// h = D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)
    /// where D is length of each node(usually = 1) and D2 is diagonal distance between each node (usually = sqrt(2) ). 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private int CalculateEuclideanlDistanceHeuristicCost(NodeDataModel current, NodeDataModel goal)
    {
        // Debug.Log($"Node Current EDHC: {current}");
        // Debug.Log($"Node Goal EDHC: {goal}");
        return Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(current.Column - goal.Column, 2) + Mathf.Pow(current.Row - goal.Row, 2)));
    }



    /// <summary>
    /// This uses the formula:
    /// h = sqrt ( (current_cell.x – goal.x)2 + 
    ///            (current_cell.y – goal.y)2 )
    /// </summary>
    /// <param name="current"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private int CalculateDiagonalDistanceHeuristicCost(NodeDataModel current, NodeDataModel goal)
    {
        int xDistance = Mathf.Abs(current.Column - goal.Column);
        int yDistance = Mathf.Abs(current.Row - goal.Row);

        return MOVE_STRAIGHT_COST * (xDistance + yDistance) + (MOVE_DIAGONAL_COST - 2 * MOVE_STRAIGHT_COST) * Mathf.Min(xDistance, yDistance);
    }



    /// <summary>
    /// Gets the node with the Lowest Fcose 
    /// </summary>
    /// <param name="pathNodes">list of nodes in the open list</param>
    /// <returns></returns>
    private NodeDataModel GetLowestFCostNode(List<NodeDataModel> pathNodes)
    {
        NodeDataModel node = pathNodes.OrderBy(n => n.FCost).FirstOrDefault();
        return node;
    }

}