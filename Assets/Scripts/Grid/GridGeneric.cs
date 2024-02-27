using System;
using UnityEngine;

public class GridGeneric<TGridObj>
{
    public int width;
    public int height;
    public int celLSize;
    public TGridObj[,] gridArray;

    public GridGeneric(int width, int height, int celLSize, Func<int, int, TGridObj> setGridObject)
    {
        this.width = width;
        this.height = height;
        this.celLSize = celLSize;

        gridArray = new TGridObj[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
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
        return new Vector2(x, y) * celLSize;
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
        x = Mathf.FloorToInt(worldPos.x / celLSize);
        y = Mathf.FloorToInt(worldPos.y / celLSize);
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
    private bool ValidateCoordinate(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    #endregion

    #region Gizmo Draw Grids

    /// <summary>
    /// Draws the grid line and center diomond block
    /// </summary>
    /// <param name="vec"></param>
    public void DrawGridLines(Vector3 vec, float delay)
    {
        float innerGapSize = celLSize / 10f;
        float outerGapSize = celLSize / 2f;

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
        return width;
    }

    internal int Getheight()
    {
        return height;
    }
    #endregion

}