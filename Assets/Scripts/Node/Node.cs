using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{


    public List<NodeData> neighbours;


    public SpriteRenderer OpenCell;
    public SpriteRenderer ClosedCell;
    public bool isClosedCell;



    #region Init Methods

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void GetNeigh()
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
        AddNeighbours(Vector2.up);
        AddNeighbours(Vector2.down);
        AddNeighbours(Vector2.left);
        AddNeighbours(Vector2.right);
    }


    /// <summary>
    /// Adds it's neighbour and ignore self
    /// </summary>
    /// <param name="dir">Direction where the ray will be cast</param>
    private void AddNeighbours(Vector2 dir)
    {
        float maxDistance = .2f;
        RaycastHit2D[] hits;

        hits = Physics2D.RaycastAll(transform.position, dir, maxDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.name != gameObject.name)
            {
                Debug.DrawRay(transform.position, dir * maxDistance, Color.green, 1);
                neighbours.Add(new()
                {
                    node = hit.transform.GetComponent<Node>()
                });
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
}


[Serializable]
public class NodeData
{
    public Node node;
    public NodeDirectionEnum nodeDirection;
}