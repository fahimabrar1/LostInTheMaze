using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Node : MonoBehaviour
{


    [Tooltip("A list of neighboring nodes to this node.")]
    public List<Node> neighbours = new();

    [Tooltip("The open cell game object used to represent a walkable node.")]
    public GameObject OpenCell;

    [Tooltip("Declare a public GameObject variable to represent the closed cell in the game")]
    public GameObject ClosedCell;

    [Tooltip("Create an instance of the NodeDataModel class to store data related to the game's nodes")]
    public NodeDataModel model = new();


    #region Init Methods


    #endregion Init Methods


    #region Node Logic Methods

    /// <summary>
    /// Calls to get neighbour on it's 4 direction
    /// </summary>
    public void Get4Neighbours()
    {
        if (!model.isWalkable) return;
        //If the collider of the object hit is not NUll
        AddNeighbour(Vector2.up);
        AddNeighbour(Vector2.down);
        AddNeighbour(Vector2.left);
        AddNeighbour(Vector2.right);
    }



    /// <summary>
    /// Adds the neighbouring nodes of the current node in the specified direction.
    /// </summary>
    /// <param name="dir">Direction where the ray will be cast.</param>
    private void AddNeighbour(Vector2 dir)
    {
        // Define the maximum distance for the raycast.
        float maxDistance = model.CellSize;

        // Perform the raycast in the specified direction.
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, maxDistance);

        // Iterate through each hit in the array.
        foreach (RaycastHit2D hit in hits)
        {
            // If the hit's collider is not null and the hit's game object is not the current node's game object...
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                // Get the Node component of the hit's transform.
                Node node = hit.transform.GetComponent<Node>();

                // If the neighbouring node's model is walkable...
                if (node.model.isWalkable)
                {
                    // Draw a blue ray from the current node to the neighbouring node.
                    Debug.DrawRay(transform.position, dir * maxDistance, Color.blue, 1);

                    // If the neighbouring node is not already in the current node's list of neighbours...
                    if (!neighbours.Contains(node))
                    {
                        // Add the neighbouring node to the list of neighbours.
                        neighbours.Add(node);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Determines the direction to the neighbouring node based on the position of the neighbour.
    /// </summary>
    /// <param name="neighbourPosition">The position of the neighbouring node.</param>
    /// <returns>The direction to the neighbouring node.</returns>
    public NodeDirectionEnum GetDirectionToNeighbour(Vector3 neighbourPosition)
    {
        // Calculate the offset between the current node and the neighbouring node
        Vector3 offset = neighbourPosition - transform.position;

        // Check if the absolute difference between the x-coordinates is greater than the absolute difference between the y-coordinates
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            // If the x-coordinate of the offset is positive, the neighbour is to the right
            if (offset.x > 0)
                return NodeDirectionEnum.right;
            // Otherwise, the neighbour is to the left
            else
                return NodeDirectionEnum.left;
        }
        else
        {
            // If the y-coordinate of the offset is positive, the neighbour is above
            if (offset.y > 0)
                return NodeDirectionEnum.up;
            // Otherwise, the neighbour is below
            else
                return NodeDirectionEnum.down;
        }
    }

    #endregion Node Logic Methods


    #region Math Methods

    /// <summary>
    /// Returns the distance from the current node to the targetVector
    /// </summary>
    /// <param name="targetVector">The target vector</param>
    /// <returns>Distance</returns>
    private float GetDistanceBetweenTarget(Vector2 targetVector)
    {
        return Vector2.Distance(targetVector, transform.position);
    }

    #endregion Math Methods

    #region 
    public NodeDataModel GetNodeDataModel()
    {
        return model;
    }





    internal void Initialize()
    {
        if (!model.isWalkable)
        {
            ClosedCell.SetActive(true);
            OpenCell.SetActive(false);
        }
        else
        {
            ClosedCell.SetActive(false);
            OpenCell.SetActive(true);
        }

        //Settings the sprite to the cell size
        var nodeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        float scaleX = model.CellSize / nodeSpriteRenderer.bounds.size.x;
        float scaleY = model.CellSize / nodeSpriteRenderer.bounds.size.y;
        transform.GetChild(0).localScale = new Vector3(scaleX, scaleY, 0);

        var nodeSpriteRenderer2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
        scaleX = model.CellSize / nodeSpriteRenderer2.bounds.size.x;
        scaleY = model.CellSize / nodeSpriteRenderer2.bounds.size.y;
        transform.GetChild(1).localScale = new Vector3(scaleX, scaleY, 0);

    }
    #endregion Basic Methods
}