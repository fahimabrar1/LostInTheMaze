using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }

    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        InputActions = new();
        PlayerActions = InputActions.Player;
    }


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        InputActions.Enable();
    }


    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        InputActions.Disable();
    }


}
