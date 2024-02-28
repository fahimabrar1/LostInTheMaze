using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance { get; private set; }

    public GameObject Player;
    public PathfindingTesting pathfindingTesting;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        Application.targetFrameRate = 60;
        pathfindingTesting = FindAnyObjectByType<PathfindingTesting>();
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        SpawnPlayer();
    }


    /// <summary>
    /// Spawns a player at the starting position.
    /// </summary>
    /// <remarks>
    /// This method uses a coroutine to delay the spawning process for 3 seconds.
    /// The player is instantiated from the 'Player' prefab and its 'Player_V2' component is accessed.
    /// The player's current node and target node are set to the starting node (0, 0).
    /// </remarks>
    public async void SpawnPlayer()
    {
        // Delay the spawning process for 3 seconds using a coroutine.
        await Task.Delay(3000);

        // Instantiate the player from the 'Player' prefab and get its 'Player_V2' component.
        var Obj = Instantiate(Player).GetComponent<Player_V2>();

        // Find the starting node (0, 0) in the pathfindingTesting nodes list.
        var a = pathfindingTesting.nodes.Find((node) => node.model.XPosition == 0 && node.model.YPosition == 0);

        // Set the player's current node to the starting node.
        Obj.CurrentNode = a;

        // Set the player's target node to the starting node.
        Obj.moveToNode = a;
    }





}
