using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbours;


    public GameObject OpenCell;
    public GameObject ClosedCell;

    public NodeDataModel model;


    #region Init Methods
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        model = new();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void GetNeighbour()
    {
        GetNeighbours();
    }

    #endregion Init Methods


    #region Node Logic Methods

    /// <summary>
    /// Calls to get neighbour on it's 4 direction
    /// </summary>
    private void GetNeighbours()
    {
        //If the collider of the object hit is not NUll
        AddNeighbour(Vector2.up);
        AddNeighbour(Vector2.down);
        AddNeighbour(Vector2.left);
        AddNeighbour(Vector2.right);
    }


    /// <summary>
    /// Adds it's neighbour and ignore self
    /// </summary>
    /// <param name="dir">Direction where the ray will be cast</param>
    private void AddNeighbour(Vector2 dir)
    {
        float maxDistance = .2f;
        RaycastHit2D[] hits;

        hits = Physics2D.RaycastAll(transform.position, dir, maxDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.name != gameObject.name)
            {
                Debug.DrawRay(transform.position, dir * maxDistance, Color.green, 1);
                neighbours.Add(
                    hit.transform.GetComponent<Node>()
                );
            }
        }
    }




    public NodeDirectionEnum GetDirectionToNeighbour(Vector3 neighbourPosition)
    {
        Vector3 offset = neighbourPosition - transform.position;

        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            if (offset.x > 0)
                return NodeDirectionEnum.right;
            else
                return NodeDirectionEnum.left;
        }
        else
        {
            if (offset.y > 0)
                return NodeDirectionEnum.up;
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