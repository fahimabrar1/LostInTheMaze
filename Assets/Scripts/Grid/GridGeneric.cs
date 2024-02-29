using System;
using UnityEngine;

public class GridGeneric<TGridObj>
{

    // Define the number of rows in the grid
    public int rows;

    // Define the number of columns in the grid
    public int columns;

    // Define the size of each cell in the grid
    public int cellSize;

    // Define a 2D array to store the grid objects
    // The array is of type TGridObj[,] and represents a 2D grid
    // Each element in the array represents a cell in the grid
    public TGridObj[,] gridArray;




    /// <summary>
    /// Initializes a new instance of the <see cref="GridGeneric{TGridObj}"/> class.
    /// </summary>
    /// <param name="rows">The number of rows in the grid.</param>
    /// <param name="columns">The number of columns in the grid.</param>
    /// <param name="celLSize">The size of each cell in the grid.</param>
    /// <param name="setGridObject">A function that sets the value of each cell in the grid based on its row and column indices.</param>
    public GridGeneric(int rows, int columns, int celLSize, Func<int, int, TGridObj> setGridObject)
    {
        // Store the number of rows, columns, and cell size in private fields.
        this.rows = rows;
        this.columns = columns;
        this.cellSize = celLSize;

        // Create a new 2D array to store the grid objects.
        gridArray = new TGridObj[rows, columns];

        // Iterate through each row and column in the grid.
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                // Set the value of the current cell using the provided function.
                gridArray[x, y] = setGridObject(x, y);
            }
        }
    }


    #region Methods
    /// <summary>
    /// Gets the world position with the cell size
    /// </summary>
    /// <param name="x">X coordinate of the grid</param>
    /// <param name="y">Y coordinate of the grid</param>
    /// <returns>The Cell coordinate according to the cell size in world space</returns>
    public Vector3 GetWorldCellPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }


    /// <summary>
    /// Sets the value for the cell usign Coordinates
    /// </summary>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    /// <param name="value">Data to be stored</param>
    public void SetValue(int x, int y, TGridObj value)
    {
        if (ValidateCoordinate(x, y))
            gridArray[x, y] = value;
    }


    /// <summary>
    /// Sets the value for the cell usign world coordinate
    /// </summary>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    /// <param name="value">Data to be stored</param>
    public void SetValue(Vector3 worldPos, TGridObj value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetValue(x, y, value);
    }


    /// <summary>
    /// Returns the value of x and y to the original grid number
    /// </summary>
    /// <param name="worldPos">World position of the cell</param>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.RoundToInt(worldPos.x / cellSize);
        y = Mathf.RoundToInt(worldPos.y / cellSize);
    }


    /// <summary>
    /// Returns the value of the cell
    /// </summary>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    /// <returns>The Cell Data</returns>
    public TGridObj GetValue(int x, int y)
    {
        if (ValidateCoordinate(x, y))
            return gridArray[x, y];
        else
            // Return default data when it is not valid, e.g: 
            // int type: 0
            // bool type: false
            // custom class type: null instance
            return default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public TGridObj GetValue(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetValue(x, y);
    }


    /// <summary>
    /// Validate the cordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool ValidateCoordinate(int x, int y) => x >= 0 && y >= 0 && x < rows && y < columns;

    #endregion

    #region Gizmo Draw Grids

    /// <summary>
    /// Draws the grid line and center diomond block
    /// </summary>
    /// <param name="vec"></param>
    public void DrawGridLines(Vector3 vec, float delay)
    {
        float innerGapSize = cellSize / 10f;
        float outerGapSize = cellSize / 2f;

        //Inner        
        Debug.DrawLine(vec + new Vector3(0, innerGapSize), vec + new Vector3(-innerGapSize, 0), Color.yellow, delay);
        Debug.DrawLine(vec + new Vector3(-innerGapSize, 0), vec + new Vector3(0, -innerGapSize), Color.yellow, delay);
        Debug.DrawLine(vec + new Vector3(0, -innerGapSize), vec + new Vector3(innerGapSize, 0), Color.yellow, delay);
        Debug.DrawLine(vec + new Vector3(innerGapSize, 0), vec + new Vector3(0, innerGapSize), Color.yellow, delay);

        // Outer
        Debug.DrawLine(vec + new Vector3(-outerGapSize, outerGapSize), vec + new Vector3(outerGapSize, outerGapSize), Color.white, delay);
        Debug.DrawLine(vec + new Vector3(outerGapSize, outerGapSize), vec + new Vector3(outerGapSize, -outerGapSize), Color.white, delay);
        Debug.DrawLine(vec + new Vector3(outerGapSize, -outerGapSize), vec + new Vector3(-outerGapSize, -outerGapSize), Color.white, delay);
        Debug.DrawLine(vec + new Vector3(-outerGapSize, -outerGapSize), vec + new Vector3(-outerGapSize, outerGapSize), Color.white, delay);
    }

    internal int GetWidth()
    {
        return rows;
    }

    internal int Getheight()
    {
        return columns;
    }
    #endregion

}