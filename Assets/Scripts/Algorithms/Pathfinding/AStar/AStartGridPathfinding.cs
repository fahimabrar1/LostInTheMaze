using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class AStartGridPathfinding
{
    /// <href ="https://www.geeksforgeeks.org/a-search-algorithm/">
    public const int MOVE_STRAIGHT_COST = 10;

    public const int MOVE_DIAGONAL_COST = 20;


    [Tooltip("Nodes queued up for searching")]
    public List<Node> openList;

    [Tooltip("Nodes that have alreadty been searched")]
    public List<Node> closedList;

    /// <summary>
    /// The Grid system
    /// </summary>
    public GridGeneric<Node> grid;

    public AStartGridPathfinding(int width, int height)
    {
        grid = new(width, height, 10, (x, y) => new Node(x, y));


        // for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        // {
        //     for (int y = 0; y < grid.gridArray.GetLength(1); y++)
        //     {
        //         Debug.Log($"X: {grid.gridArray[x, y].XPosition}, Y: {grid.gridArray[x, y].YPosition}, ");
        //     }
        // }
    }


    /// <summary>
    /// The Algorithm that finds the shortest path from start to end
    /// </summary>
    /// <param name="startX">The starting x point of the node</param>
    /// <param name="startY">The starting y point of the node</param>
    /// <param name="endX">The end x point of the node</param>
    /// <param name="endY">The end y point of the node</param>
    /// <returns></returns>
    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        closedList = new();
        Node startNode = grid.GetValue(startX, startY);
        Node endNode = grid.GetValue(endX, endY);
        openList = new() { startNode };
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                Node node = grid.GetValue(x, y);
                node.GCost = int.MaxValue;
                node.CalculateFCost();
                node.previousNode = null;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDiagonalDistanceHeuristicCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                // Finish Search
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            List<Node> neighbours = Get8DirectionalNeighbours(currentNode);
            foreach (Node neighbour in neighbours)
            {
                // Ignore neighbour if in close list
                if (closedList.Contains(neighbour) || neighbour == null) continue;
                Debug.Log($"Neighbour {neighbour}");
                int gCost = currentNode.GCost + CalculateEuclideanlDistanceHeuristicCost(currentNode, neighbour);
                if (gCost < neighbour.GCost)
                {
                    neighbour.previousNode = currentNode;
                    neighbour.GCost = gCost;
                    neighbour.HCost = CalculateDiagonalDistanceHeuristicCost(currentNode, neighbour);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }

                }
            }
        }

        return null;
    }


    /// <summary>
    /// Gets the 8 direcitonal neighbour of a node in the grid
    /// </summary>
    /// <param name="currentNode">The inital from of thich the surrounding  nodes will be gotten</param>
    /// <returns>The 8 neighbour or successor</returns>
    private List<Node> Get8DirectionalNeighbours(Node currentNode)
    {
        List<Node> neighboursList = new();
        if (currentNode.XPosition - 1 >= 0)
        {
            // Left Node
            neighboursList.Add(GetNode(currentNode.XPosition - 1, currentNode.YPosition));
            // Left Down
            if (currentNode.YPosition - 1 >= 0) neighboursList.Add(GetNode(currentNode.XPosition - 1, currentNode.YPosition - 1));
            // Left Up
            if (currentNode.YPosition + 1 >= 0) neighboursList.Add(GetNode(currentNode.XPosition - 1, currentNode.YPosition + 1));
        }

        if (currentNode.XPosition + 1 < grid.GetWidth())
        {
            // Right Node
            neighboursList.Add(GetNode(currentNode.XPosition + 1, currentNode.YPosition));
            // Right Down
            if (currentNode.YPosition - 1 >= 0) neighboursList.Add(GetNode(currentNode.XPosition + 1, currentNode.YPosition - 1));
            // Right Up
            if (currentNode.YPosition + 1 >= 0) neighboursList.Add(GetNode(currentNode.XPosition + 1, currentNode.YPosition + 1));
        }

        // Up
        if (currentNode.YPosition + 1 < grid.Getheight()) neighboursList.Add(GetNode(currentNode.XPosition, currentNode.YPosition + 1));

        // Down
        if (currentNode.YPosition - 1 >= 0) neighboursList.Add(GetNode(currentNode.XPosition, currentNode.YPosition - 1));


        Debug.Log($"Found: {neighboursList.Count} Nodes");
        return neighboursList;
    }


    /// <summary>
    /// Calculates the final path tracing back to starting node
    /// </summary>
    /// <param name="endNode">the final destination node</param>
    /// <returns>the path node list</returns>
    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new()
        {
            endNode
        };

        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }


    /// <summary>
    /// Gets the node from the grid bvy value
    /// </summary>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    /// <returns></returns>
    public Node GetNode(int x, int y)
    {
        return grid.GetValue(x, y);
    }




    /// <summary>
    /// This uses the formula:
    /// h = D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)
    /// where D is length of each node(usually = 1) and D2 is diagonal distance between each node (usually = sqrt(2) ). 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private int CalculateEuclideanlDistanceHeuristicCost(Node current, Node goal)
    {
        Debug.Log($"Node Current EDHC: {current}");
        Debug.Log($"Node Goal EDHC: {goal}");
        return Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(current.XPosition - goal.XPosition, 2) + Mathf.Pow(current.YPosition - goal.YPosition, 2)));
    }



    /// <summary>
    /// This uses the formula:
    /// h = sqrt ( (current_cell.x – goal.x)2 + 
    ///            (current_cell.y – goal.y)2 )
    /// where D is length of each node(usually = 1) and D2 is diagonal distance between each node (usually = sqrt(2) ). 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private int CalculateDiagonalDistanceHeuristicCost(Node current, Node goal)
    {
        int xDistance = Mathf.Abs(current.XPosition - goal.XPosition);
        int yDistance = Mathf.Abs(current.YPosition - goal.YPosition);

        return MOVE_STRAIGHT_COST * (xDistance + yDistance) + (MOVE_DIAGONAL_COST - 2 * MOVE_STRAIGHT_COST) * Mathf.Min(xDistance, yDistance);
    }



    /// <summary>
    /// Gets the node with the Lowest Fcose 
    /// </summary>
    /// <param name="pathNodes">list of nodes in the open list</param>
    /// <returns></returns>
    private Node GetLowestFCostNode(List<Node> pathNodes)
    {
        Node node = pathNodes.OrderBy(n => n.FCost).FirstOrDefault();
        return node;
    }

}