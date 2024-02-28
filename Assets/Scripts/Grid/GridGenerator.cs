using UnityEngine;

public class GridGenerator : MonoBehaviour
{


    [Tooltip("The size of the grid cell")]
    public int cellSize = 5;

    [Tooltip("The number of columns in the grid")]
    public int columns = 5;

    [Tooltip("The number of rows in the grid")]
    public int rows = 5;


    [Header("Node Prefab")]
    public GameObject nodePrefab;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Create a new grid with the specified number of rows, columns, and cell size
        // The third parameter is a function that takes the x and y coordinates of a cell and returns a new NodeDataModel object
        GridGeneric<NodeDataModel> grid = new(rows, columns, cellSize, (x, y) => new NodeDataModel(x, y));
    }
}