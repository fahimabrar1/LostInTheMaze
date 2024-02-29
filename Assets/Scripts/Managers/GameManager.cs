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

    public PacNode spawnPosition;
    public PathfindingTesting pathfindingTesting;
    public CustomGridPathFinding customGridPathFinding;


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

        try
        {
            pathfindingTesting = FindAnyObjectByType<PathfindingTesting>();
        }
        catch (System.Exception)
        { }

        try
        {
            customGridPathFinding = FindAnyObjectByType<CustomGridPathFinding>();
        }
        catch (System.Exception)
        { }


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
        Player_V2 player;
        if (spawnPosition != null)
            player = Instantiate(Player, spawnPosition.transform.position, Quaternion.identity).GetComponent<Player_V2>();
        else
            player = Instantiate(Player).GetComponent<Player_V2>();

        // Set the player's current node to the starting node.
        player.CurrentNode = spawnPosition;

        // Set the player's target node to the starting node.
        player.moveToNode = spawnPosition;
    }





}
