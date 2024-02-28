using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

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
    /// The Node currently plays is starting from
    /// </summary>
    public Node CurrentNode;


    /// <summary>
    /// The Node currently plays is starting from
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
        if (Vector3.Distance(transform.position, moveToNode.transform.position) < 0.01f && gameStart)
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
        NodeContainer data;
        SetPlayerMovingDirection(newMovementInput);

        // when the player reahes a junction it makes the new junction to current
        var PreviousNode = CurrentNode;
        CurrentNode = moveToNode;

        // now it finds new target from the juntion
        data = CurrentNode.neighbours.FirstOrDefault(node => node.nodeDirection == playerMovingDirection);

        // moves towards the new target
        if (data != null)
        {
            movementInput = newMovementInput;
            moveToNode = data.node;
        }
        else
        {

            SetPlayerMovingDirection(movementInput);

            //checks for the new junciton on the same direction it is facing
            data = CurrentNode.neighbours.FirstOrDefault(node => node.nodeDirection == playerMovingDirection);

            // save the data, no need for new input, as it's the same
            if (data != null)
            {
                moveToNode = data.node;
            }
            else
            {
                // there no data is found, then it's a dead end
                movementInput = Vector2.zero;
            }
        }
    }


    /// <summary>
    /// The movement and rotation of the player into 4 directions
    /// </summary>
    private void MovementAndRotation()
    {
        if (movementInput != null)
            transform.localPosition += speed * Time.deltaTime * (Vector3)movementInput;
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

        if (!gameStart || IsOppositeDirection(newMovementInput, movementInput))
            CheckForNextNode(true);

        if (!gameStart) gameStart = true;

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
