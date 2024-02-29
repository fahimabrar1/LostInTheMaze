using System;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class NodeDataModel
{
    [Header("A* data")]

    [Tooltip("X-coordinate of the node.")]
    public int Column;

    [Tooltip("Y-coordinate of the node.")]
    public int Row;

    [Tooltip("CellSize of the of the node.")]
    public float CellSize;

    [Tooltip("Walking Distance from Start node")]
    public int GCost;

    [Tooltip("Heuristic Cost to reach End node")]
    public int HCost;

    [Tooltip("Combination of GCost and HCost")]
    public int FCost;


    [Tooltip("The Previous Node, uses it to trace back to find the final path")]
    public NodeDataModel previousNode;

    [Header("Game Varaibles")]
    public bool isWalkable;
    public bool isJunction;


    public NodeDataModel() { }


    public NodeDataModel(int XPosition, int YPosition)
    {
        this.Column = XPosition;
        this.Row = YPosition;
    }

    public NodeDataModel(int XPosition, int YPosition, float cellSize, bool isWalkable)
    {
        this.Column = XPosition;
        this.Row = YPosition;
        this.CellSize = cellSize;
        this.isWalkable = isWalkable;
    }

    /// <summary>
    /// Calculates the fcost
    /// </summary>
    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }
}

