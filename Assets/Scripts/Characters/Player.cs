using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

[RequireComponent(typeof(PlayerInputControl))]
public class Player : MonoBehaviour
{

    [Range(0.1f, 1f)]
    public float speed = 0.5f;

    public bool gameStart = false;



    /// <summary>
    /// The New Player input 
    /// </summary>
    public PlayerInputControl PlayerInput;


    /// <summary>
    /// The Node currently plays is starting from
    /// </summary>
    public BasicNode CurrentNode;


    /// <summary>
    /// The Node currently plays is starting from
    /// </summary>
    public BasicNode moveToNode;

    /// <summary>
    /// Stores the information when player is in the junction
    /// </summary>
    private Vector2 movementInput;


    /// <summary>
    /// Stores the input user pressed before reaching junction
    /// </summary>
    private Vector2 newMovementInput;


    /// <summary>
    /// Player moving direction
    /// </summary>
    private NodeDirectionEnum playerMovingDirection;

    #region  Mono Methods
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        PlayerInput.InputActions.Player.Movement.started += OnPressInputStarted;
    }


    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        PlayerInput.InputActions.Player.Movement.started -= OnPressInputStarted;
    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (transform.position == moveToNode.transform.position && gameStart)
        {
            CheckForNextNode(reachedJunction: true);
        }
    }


    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        MovementAndRotation();
    }

    #endregion Mono Methods

    #region Methods


    /// <summary>
    /// Checks if there is a node in player travelling direction
    /// </summary>
    private void CheckForNextNode(bool reachedJunction = false)
    {
        NodeData data;
        SetPlayerMovingDirection(newMovementInput);

        // if (reachedJunction)
        // {
        //     // when the player reahes a junction it makes the new junction to current
        //     var PreviousNode = CurrentNode;
        //     CurrentNode = moveToNode;


        //     // now it finds new target from the juntion
        //     data = CurrentNode.junctionNeighborsData.FirstOrDefault(node => node.nodeDirection == playerMovingDirection);

        //     // moves towards the new target
        //     if (data != null)
        //     {
        //         movementInput = newMovementInput;
        //         moveToNode = data.node;
        //     }
        //     else
        //     {
        //         //  if no next node was found, we fetch the initial direction back
        //         SetPlayerMovingDirection(movementInput);

        //         //checks for the new junciton on the same direction it is facing
        //         data = CurrentNode.junctionNeighborsData.FirstOrDefault(node => node.nodeDirection == playerMovingDirection);

        //         // save the data, no need for new input, as it's the same
        //         if (data != null)
        //         {
        //             moveToNode = data.node;
        //         }
        //         else
        //         {
        //             // there no data is found, then it's a dead end
        //             movementInput = Vector2.zero;
        //         }
        //     }

        // }
        // else
        // {
        //     // tries to get the Current node direction from the newNode. 
        //     //? Why we do this?
        //     // To go back to the alternate way only
        //     data = moveToNode.junctionNeighborsData.FirstOrDefault(node => node.nodeDirection == playerMovingDirection);

        //     //if the data is the current node, set data and move
        //     if (data != null && data.node == CurrentNode)
        //     {
        //         // we swap the data
        //         (moveToNode, CurrentNode) = (CurrentNode, moveToNode);
        //         movementInput = newMovementInput;
        //     }

        // }
    }


    /// <summary>
    /// The movement and rotation of the player into 4 directions
    /// </summary>
    private void MovementAndRotation()
    {
        if (movementInput != null)
            transform.localPosition += 0.5f * Time.deltaTime * (Vector3)movementInput;
        if (movementInput == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            transform.localScale = new(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);

        }
        else if (movementInput == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            transform.localScale = new(Mathf.Abs(transform.localScale.x), -Mathf.Abs(transform.localScale.y), transform.localScale.z);

        }
        else if (movementInput == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);

        }
        else if (movementInput == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
    }


    protected void SetPlayerMovingDirection(Vector2 vector2)
    {
        if (vector2 == Vector2.up)
        {
            playerMovingDirection = NodeDirectionEnum.up;
        }
        else if (vector2 == Vector2.down)
        {
            playerMovingDirection = NodeDirectionEnum.down;
        }
        else if (vector2 == Vector2.left)
        {
            playerMovingDirection = NodeDirectionEnum.left;
        }
        else if (vector2 == Vector2.right)
        {
            playerMovingDirection = NodeDirectionEnum.right;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnStartMove()
    {

    }

    #endregion  Methods


    #region Input Methods
    /// <summary>
    /// The input value when the key is pressed only
    /// </summary>
    /// <param name="context"></param>
    private void OnPressInputStarted(InputAction.CallbackContext context)
    {
        newMovementInput = context.ReadValue<Vector2>();
        Debug.Log($"Input: {newMovementInput}");

        CheckForNextNode(gameStart ? false : true);

        if (!gameStart) gameStart = true;

    }
    #endregion  Input Methods


    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.tag);

    }

}
