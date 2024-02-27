using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStartGridPathfinding
{
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
        startNode.HCost = CalculateHeuristicCost(startNode, endNode);
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

            foreach (Node neighbour in Get8DirectionalNeighbours(currentNode))
            {
                // Ignore neighbour if in close list
                if (closedList.Contains(neighbour)) continue;

                int gCost = currentNode.GCost + CalculateHeuristicCost(currentNode, neighbour);
                if (gCost < neighbour.GCost)
                {
                    neighbour.previousNode = currentNode;
                    neighbour.GCost = gCost;
                    neighbour.HCost = CalculateHeuristicCost(currentNode, neighbour);
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



    public Node GetNode(int x, int y)
    {
        return grid.GetValue(x, y);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int CalculateHeuristicCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.XPosition - b.XPosition);
        int yDistance = Mathf.Abs(a.YPosition - b.YPosition);
        int remainingDistance = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remainingDistance;
    }


    private Node GetLowestFCostNode(List<Node> pathNodes)
    {
        Node node = pathNodes[0];

        Node n = pathNodes.OrderBy(n => n.FCost).FirstOrDefault();
        Debug.Log($"Node For Lowest FC: {n}");
        return n;
    }

}