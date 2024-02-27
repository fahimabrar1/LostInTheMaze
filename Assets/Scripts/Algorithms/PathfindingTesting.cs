using UnityEngine;
using FahimsUtils;
using System.Collections.Generic;
public class PathfindingTesting : MonoBehaviour
{

    [Tooltip("The pathfinding grid system")]
    private AStartGridPathfinding gridPathfinding;


    // Start is called before the first frame update
    void Start()
    {
        gridPathfinding = new(10, 10);
        InvokeRepeating("DrawGridGizmos", 0.0f, 0.1f);
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
            List<Node> path = gridPathfinding.FindPath(0, 0, x, y);
            if (path.Count > 0)
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
                    catch (System.Exception)
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
