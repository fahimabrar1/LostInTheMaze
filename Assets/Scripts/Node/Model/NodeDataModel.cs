using System;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class NodeDataModel
{
    [Tooltip("X-coordinate of the node.")]
    public int XPosition;

    [Tooltip("Y-coordinate of the node.")]
    public int YPosition;

    [Tooltip("CellSize of the of the node.")]
    public int CellSize;

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


    public NodeDataModel() { }


    public NodeDataModel(int XPosition, int YPosition)
    {
        this.XPosition = XPosition;
        this.YPosition = YPosition;
    }

    public NodeDataModel(int XPosition, int YPosition, int cellSize, bool isWalkable)
    {
        this.XPosition = XPosition;
        this.YPosition = YPosition;
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

