using UnityEngine;
using FahimsUtils;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
public class PathfindingTesting : MonoBehaviour
{

    [Tooltip("The pathfinding grid system")]
    private AStartGridPathfinding gridPathfinding;


    [Tooltip("The size of the grid cell")]
    public int cellSize = 5;

    [Tooltip("The number of columns in the grid")]
    public int columns = 5;

    [Tooltip("The number of rows in the grid")]
    public int rows = 5;


    [Header("Node Prefab")]

    public GameObject nodePrefab;

    [Header("Node List")]
    public List<Node> nodes;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        nodes = new();
        gridPathfinding = new(rows, columns, cellSize);
        GenerateGrid();
        InvokeRepeating("DrawGridGizmos", 0.0f, 0.1f);
    }


    private void GenerateGrid()
    {
        Debug.Log(gridPathfinding.grid.gridArray.Length);
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                var obj = Instantiate(nodePrefab, cellSize * new Vector2(x, y), Quaternion.identity, transform);
                obj.name = $"Node {x} {y}";
                // Setting the Node data
                Node node = obj.GetComponent<Node>();
                nodes.Add(node);
                node.model = gridPathfinding.grid.gridArray[x, y];
                node.Initialize();
            }
        }

        foreach (var node in nodes)
        {
            node.Get4Neighbours();
        }
    }



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int x, y;
            Vector3 location = UtilsClass.GetMouseWorldPosition();
            Debug.Log($"Location: {location}");

            gridPathfinding.grid.GetXY(location, out x, out y);
            List<NodeDataModel> path = gridPathfinding.FindPath(0, 0, x, y);
            if (path != null && path.Count > 0)
            {
                Debug.Log($"Path Len: {path.Count}");
                for (int i = path.Count - 1; i > 0; i--)
                {
                    try
                    {
                        Debug.Log($"Path X: {path[i].XPosition}, Path Y: {path[i].YPosition}");
                        Debug.Log($"Path Prev X: {path[i].previousNode.XPosition}, Path Prev  Y: {path[i].previousNode.YPosition}");
                        Debug.DrawLine(gridPathfinding.grid.GetWorldCellPosition(path[i].XPosition, path[i].YPosition), gridPathfinding.grid.GetWorldCellPosition(path[i].previousNode.XPosition, path[i].previousNode.YPosition), Color.green, 1);
                    }
                    catch (Exception)
                    {
                    }

                }
            }
        }
    }


    /// <summary>
    /// Draw Gizmo ever delay seconds
    /// </summary>
    private void DrawGridGizmos()
    {
        for (int x = 0; x < gridPathfinding.grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridPathfinding.grid.gridArray.GetLength(1); y++)
            {
                gridPathfinding.grid.DrawGridLines(gridPathfinding.grid.GetWorldCellPosition(x, y), 0.1f);
            }
        }
    }

}
