using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

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
            // Calculate row value
            int Col = Mathf.RoundToInt(Mathf.Abs((node.transform.position.x - offset.x) / cellSize));
            int Row = Mathf.RoundToInt(Mathf.Abs((node.transform.position.y - offset.y) / cellSize));
            NodeDataModel model = new()
            {
                Column = Col,
                Row = Row,
                CellSize = cellSize,
                isWalkable = node.model.isWalkable,
            };
            node.model = model;

            Debug.Log($"Row: {Row}, Col: {Col}, model Col/Row: {model.Column}/{model.Row}");
            gridPathfinding.grid.SetValue(Row, Col, model);

            node.Get4Neighbours();
        }

        // foreach (var item in gridPathfinding.grid.gridArray)
        // {
        //     Debug.Log($"{item}");
        // }

        Debug.Log(gridPathfinding.grid.gridArray[14, 0]);
    }



    // public override List<NodeContainer> GetDestinationNodes(Vector3 start, Vector3 target)
    // {
    //     return base.GetDestinationNodes(start, target);
    // }

}
