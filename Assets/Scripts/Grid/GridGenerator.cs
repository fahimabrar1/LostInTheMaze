using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Grid grid = new(4, 2, 10);
        GridGeneric<Node> grid = new(4, 2, 10);
    }
}