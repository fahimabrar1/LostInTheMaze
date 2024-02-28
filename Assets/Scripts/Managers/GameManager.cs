using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public PathfindingTesting pathfindingTesting;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
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


    async public void SpawnPlayer()
    {
        await Task.Delay(3000);
        var Obj = Instantiate(Player).GetComponent<Player_V2>();
        var a = pathfindingTesting.nodes.Find((node) => node.model.XPosition == 0 && node.model.YPosition == 0);
        Obj.CurrentNode = a;
        Obj.moveToNode = a;
    }
}
