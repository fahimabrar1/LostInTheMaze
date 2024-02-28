using System;
using UnityEngine;

public class Grid
{
    // The width of the grid in cells.
    public int width;

    // The height of the grid in cells.
    public int height;

    // The size of each cell in the grid.
    public int cellSize;

    // The 2D array representing the grid of cells.
    public int[,] gridArray;



    /// <summary>
    /// Constructor for the Grid class.
    /// </summary>
    /// <param name="width">The width of the grid in cells.</param>
    /// <param name="height">The height of the grid in cells.</param>
    /// <param name="cellSize">The size of each cell in the grid.</param>
    public Grid(int width, int height, int cellSize)
    {
        // Store the width, height, and cell size of the grid.
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        // Create a 2D array to represent the grid of cells.
        gridArray = new int[width, height];

        // Iterate through each cell in the grid.
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                // Draw the grid lines for the current cell.
                DrawGridLines(GetWorldCellPosition(x, y));
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
    /// Sets the value for the cell
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetValue(int x, int y, int value)
    {
        if (ValidateCoordinate(x, y))
            gridArray[x, y] = value;
    }


    /// <summary>
    /// Returns the value of x and y to the original grid number
    /// </summary>
    /// <param name="worldPos">World position of the cell</param>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPos.x / cellSize);
        y = Mathf.FloorToInt(worldPos.y / cellSize);
    }


    /// <summary>
    /// Returns the valur of a cell
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetValue(int x, int y)
    {
        if (ValidateCoordinate(x, y))
            return gridArray[x, y];
        return -1;
    }

    /// <summary>
    /// Returns the value of x and y to the original grid number
    /// </summary>
    /// <param name="worldPos">World position of the cell</param>
    /// <param name="x">X-Value in the grid</param>
    /// <param name="y">Y-Value in the grid</param>
    public int GetValue(Vector3 worldPos)
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
    private bool ValidateCoordinate(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    #endregion

    #region Gizmo Draw Grids

    /// <summary>
    /// Draws the grid line and center diomond block
    /// </summary>
    /// <param name="vec"></param>
    private void DrawGridLines(Vector3 vec)
    {
        float innerGapSize = cellSize / 10f;
        float outerGapSize = cellSize / 2f;

        //Inner        
        Debug.DrawLine(vec + new Vector3(0, innerGapSize), vec + new Vector3(-innerGapSize, 0), Color.green, 10);
        Debug.DrawLine(vec + new Vector3(-innerGapSize, 0), vec + new Vector3(0, -innerGapSize), Color.green, 10);
        Debug.DrawLine(vec + new Vector3(0, -innerGapSize), vec + new Vector3(innerGapSize, 0), Color.green, 10);
        Debug.DrawLine(vec + new Vector3(innerGapSize, 0), vec + new Vector3(0, innerGapSize), Color.green, 10);


        // Outer
        Debug.DrawLine(vec + new Vector3(-outerGapSize, outerGapSize), vec + new Vector3(outerGapSize, outerGapSize), Color.white, 10);
        Debug.DrawLine(vec + new Vector3(outerGapSize, outerGapSize), vec + new Vector3(outerGapSize, -outerGapSize), Color.white, 10);
        Debug.DrawLine(vec + new Vector3(outerGapSize, -outerGapSize), vec + new Vector3(-outerGapSize, -outerGapSize), Color.white, 10);
        Debug.DrawLine(vec + new Vector3(-outerGapSize, -outerGapSize), vec + new Vector3(-outerGapSize, outerGapSize), Color.white, 10);
    }
    #endregion

}