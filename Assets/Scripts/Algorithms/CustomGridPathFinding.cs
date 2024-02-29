using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomGridPathFinding : PathfindingTesting
{


    public PacNode nodeToCalculateOffset;




    public override void Start()
    {


        cellSize = (nodes[0].transform.position.y - nodeToCalculateOffset.transform.position.y) / (rows - 1);
        gridPathfinding = new(rows, columns, cellSize);
        offset = nodeToCalculateOffset.transform.position;
        foreach (var node in nodes)
        {
            var model = node.model;
            // Calculate row value
            int col = Mathf.Abs(Mathf.FloorToInt((node.transform.position.x - offset.x) / cellSize));
            int row = Mathf.Abs(Mathf.FloorToInt((node.transform.position.y - offset.y) / cellSize));
            model.XPosition = row;
            model.YPosition = col;
            model.CellSize = cellSize;
            node.model = model;

            Debug.Log($"X {row}, Y: {col}");
            gridPathfinding.grid.gridArray[row, col] = node.model;

            node.Get4Neighbours();
        }
    }



    public override List<NodeContainer> GetDestinationNodes(Vector3 start, Vector3 target)
    {
        return base.GetDestinationNodes(start, target);
    }

}
