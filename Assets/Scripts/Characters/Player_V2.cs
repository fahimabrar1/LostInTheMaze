using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

using FahimsUtils;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerInputControl))]
public class Player_V2 : MonoBehaviour
{

    [Range(0.1f, 10f)]
    public float speed = 0.5f;

    public bool gameStart = false;



    /// <summary>
    /// The New Player input 
    /// </summary>
    public PlayerInputControl PlayerInput;


    /// <summary>
    /// The Node player has already visited
    /// </summary>
    public Node PreviousNode;


    /// <summary>
    /// The Node currently plays is starting from
    /// </summary>
    public Node CurrentNode;


    /// <summary>
    /// The Node players wanted to visit
    /// </summary>
    public Node moveToNode;

    /// <summary>
    /// Stores the information when player is in the junction
    /// </summary>
    private Vector2 movementInput;


    /// <summary>
    /// Stores the input user pressed before reaching junction
    /// </summary>
    private Vector2 newMovementInput;

    /// <summary>
    /// Stores the input user pressed before reaching junction
    /// </summary>
    public List<NodeContainer> pathQueue;


    /// <summary>
    /// Player moving direction
    /// </summary>
    private NodeDirectionEnum playerMovingDirection;

    #region  Mono Methods


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        PlayerInput.InputActions.Player.Movement.started += OnPressInputStarted;
        PlayerInput.InputActions.Player.MouseMovement.started += OnLeftClickInputStarted;
    }


    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        PlayerInput.InputActions.Player.Movement.started -= OnPressInputStarted;
        PlayerInput.InputActions.Player.MouseMovement.started -= OnLeftClickInputStarted;
    }




    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        pathQueue = new();
    }



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Vector3.Distance(transform.position, moveToNode.transform.position) < 0.01f && gameStart)
        {
            Debug.Log($"Found Node Calling");
            CheckForNextNode();
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
    /// Checks for the next node in the path based on the player's movement input.
    /// </summary>
    /// <param name="reachedJunction">Optional parameter indicating if the player has reached a junction.</param>
    private void CheckForNextNode()
    {
        // Create a new NodeContainer object to store the data of the current and next nodes.
        NodeContainer data;

        // Update the player's moving direction based on the new movement input.
        SetPlayerMovingDirection(newMovementInput);

        // Store the previous node in a variable.
        PreviousNode = CurrentNode;

        // Update the current node to the next node in the path.
        CurrentNode = moveToNode;

        if (pathQueue.Count != 0)
        {
            NodeContainer container = pathQueue[0];
            pathQueue.RemoveAt(0);
            // if (container.node == moveToNode)
            // {
            //     CheckForNextNode();
            //     return;
            // }
            Debug.Log($"Found Node Current: {CurrentNode}");
            Debug.Log($"Found Node Current: {container.node} {CurrentNode.GetDirectionVectorOfNodeDirection(CurrentNode.GetDirectionToNeighbour(container.node.transform.position))}");
            Debug.Log($"Found Node Current: {container.node} {container.nodeDirection}");

            movementInput = CurrentNode.GetDirectionVectorOfNodeDirection(CurrentNode.GetDirectionToNeighbour(container.node.transform.position));
            moveToNode = container.node;
            return;
        }

        // Find the next node in the path based on the player's moving direction.
        data = CurrentNode.neighbours.FirstOrDefault(node => node.nodeDirection == playerMovingDirection);

        // If a next node is found, update the movement input and move to the next node.
        if (data != null)
        {
            movementInput = newMovementInput;
            moveToNode = data.node;
        }
        // If no next node is found, reset the movement input to zero.
        else
        {
            SetPlayerMovingDirection(movementInput);

            // Try to find a next node again by checking the neighbours of the current node.
            data = CurrentNode.neighbours.FirstOrDefault(node => node.nodeDirection == playerMovingDirection);

            // If a next node is still found, update the moveToNode variable.
            if (data != null)
            {
                moveToNode = data.node;
            }
            // If no next node is found, keep the movement input as zero.
            else
            {
                movementInput = Vector2.zero;
            }
        }
    }


    /// <summary>
    /// The movement and rotation of the player into 4 directions
    /// </summary>
    private void MovementAndRotation()
    {
        // Check if there is any movement input
        if (movementInput != null)
        {
            // Move the player in the direction of the movement input
            transform.localPosition += speed * Time.deltaTime * (Vector3)movementInput;
        }

        // Check if the movement input is up
        if (movementInput == Vector2.up)
        {
            // Rotate the player to face up
            transform.rotation = Quaternion.Euler(0, 0, 90);

            // Flip the player horizontally
            transform.localScale = new(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
        // Check if the movement input is down
        else if (movementInput == Vector2.down)
        {
            // Rotate the player to face down
            transform.rotation = Quaternion.Euler(0, 0, -90);

            // Flip the player vertically
            transform.localScale = new(Mathf.Abs(transform.localScale.x), -Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
        // Check if the movement input is left
        else if (movementInput == Vector2.left)
        {
            // Rotate the player to face left
            transform.rotation = Quaternion.Euler(0, 0, 0);

            // Flip the player horizontally
            transform.localScale = new(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
        // Check if the movement input is right
        else if (movementInput == Vector2.right)
        {
            // Rotate the player to face right
            transform.rotation = Quaternion.Euler(0, 0, 0);

            // Flip the player horizontally
            transform.localScale = new(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
    }


    /// <summary>
    /// Sets the player's moving direction based on the provided vector2.
    /// </summary>
    /// <param name="vector2">The vector2 representing the player's moving direction.</param>
    protected void SetPlayerMovingDirection(Vector2 vector2)
    {
        // If the vector2 represents up movement, set the player's moving direction to up.
        if (vector2 == Vector2.up)
        {
            playerMovingDirection = NodeDirectionEnum.up;
        }
        // If the vector2 represents down movement, set the player's moving direction to down.
        else if (vector2 == Vector2.down)
        {
            playerMovingDirection = NodeDirectionEnum.down;
        }
        // If the vector2 represents left movement, set the player's moving direction to left.
        else if (vector2 == Vector2.left)
        {
            playerMovingDirection = NodeDirectionEnum.left;
        }
        // If the vector2 represents right movement, set the player's moving direction to right.
        else if (vector2 == Vector2.right)
        {
            playerMovingDirection = NodeDirectionEnum.right;
        }
    }
    #endregion  Methods


    #region Input Methods
    /// <summary>
    /// The input value when the key is pressed only
    /// </summary>
    /// <param name="context"></param>
    private void OnPressInputStarted(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Read the input value as a Vector2
            newMovementInput = context.ReadValue<Vector2>();
            Debug.Log($"Input: {newMovementInput}");
            // pathQueue.Clear();

            // Check if the game has started or if the new input direction is opposite to the current input direction
            if (!gameStart || IsOppositeDirection(newMovementInput, movementInput))
                CheckForNextNode();

            // If the game hasn't started yet, set the gameStart flag to true
            if (!gameStart) gameStart = true;
        }
    }

    private void OnLeftClickInputStarted(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // try
            // {
            var input = Mouse.current.position.ReadValue();
            var worldInput = UtilsClass.GetMouseWorldPositionWithZ(input);
            pathQueue = new();
            pathQueue = GameManager.instance.GetDestinationNodes(transform.position, worldInput);
            // If the game hasn't started yet, set the gameStart flag to true
            if (!gameStart) gameStart = true;

            CheckForNextNode();
            // }
            // catch (Exception)
            // {
            //     Debug.LogWarning("Failed to click");
            // }

        }

    }



    /// <summary>
    /// Check if two input vectors are opposite directions
    /// </summary>
    /// <param name="vector1">First input vector</param>
    /// <param name="vector2">Second input vector</param>
    /// <returns>True if the vectors are opposite directions, false otherwise</returns>
    private bool IsOppositeDirection(Vector2 vector1, Vector2 vector2)
    {
        // Normalize the vectors
        Vector2 normalizedVector1 = vector1.normalized;
        Vector2 normalizedVector2 = vector2.normalized;

        // Calculate the dot product
        float dotProduct = Vector2.Dot(normalizedVector1, normalizedVector2);

        // Check if the dot product is close to -1
        return Mathf.Approximately(dotProduct, -1);
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
