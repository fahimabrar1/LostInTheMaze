using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Configuration")]
    [Tooltip("The width of the grid")]
    public float gridWidth = 5f;

    [Tooltip("The height of the grid")]
    public float gridHeight = 5f;

    [Tooltip("The number of columns in the grid")]
    public int columns = 5;

    [Tooltip("The number of rows in the grid")]
    public int rows = 5;

    [Tooltip("The gap between each node")]
    public float nodeGap = 0.2f;

    [Header("Node Prefab")]
    public GameObject nodePrefab;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Calculate the width and height of each cell
        float cellWidth = gridWidth / columns;
        float cellHeight = gridHeight / rows;

        var nodeSpriteRenderer = nodePrefab.transform.GetChild(0).GetComponent<SpriteRenderer>();
        // Calculate the scale factor for the node sprite
        float scaleX = cellWidth / nodeSpriteRenderer.bounds.size.x;
        float scaleY = cellHeight / nodeSpriteRenderer.bounds.size.y;

        // Loop through each row and column to instantiate nodes
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Calculate the position for the current node
                float xPos = x * (cellWidth + nodeGap) - (gridWidth / 2f) + (cellWidth / 2f);
                float yPos = y * (cellHeight + nodeGap) - (gridHeight / 2f) + (cellHeight / 2f);
                Vector3 nodePosition = transform.position + new Vector3(xPos, yPos, 0f);

                // Instantiate the node
                GameObject node = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
                node.transform.parent = transform; // Set the node's parent to this GameObject

                // Set the scale of the node to fit within the grid cell
                node.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            }
        }
    }
}