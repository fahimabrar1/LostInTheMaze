using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGridPathFinding : MonoBehaviour
{

    public int Rows = 21;
    public int Cols = 15;
    public float CellSize = 0.16f;

    public PacNode nodeToCalculateOffset;

    public Vector2 offset;

    public List<Node> nodes;


    public AStartGridPathfinding aStartGrid;

    // Start is called before the first frame update
    void Start()
    {
        aStartGrid = new(Rows, Cols, 0.16f);
        CellSize = (nodes[0].transform.position.y - nodeToCalculateOffset.transform.position.y) / (Rows - 1);
        offset = nodeToCalculateOffset.transform.position;
        foreach (var node in nodes)
        {
            var model = node.model;
            // Calculate row value
            int col = Mathf.Abs(Mathf.FloorToInt((node.transform.position.x - offset.x) / CellSize));
            int row = Mathf.Abs(Mathf.FloorToInt((node.transform.position.y - offset.y) / CellSize));
            model.XPosition = row;
            model.YPosition = col;
            model.CellSize = CellSize;
            node.model = model;

            Debug.Log($"X {row}, Y: {col}");
            aStartGrid.grid.gridArray[row, col] = node.model;

            node.Get4Neighbours();
        }
    }

}
