using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Null,
        Setup,
        Preparation,
        Fight
    };

    private static GameManager instance = null;

    private GameState gameState = GameState.Setup;
    private List<Door> doors;
    private List<Door> tempDoors;   // temporary list for when the user wnats to add a door marker while fighting
    private int maxFloor = -1;
    private int currentFloor = 0;


    void Start()
    {
        if (instance != null)
        {
            Debug.LogError("GameManagerInstance Already Exists !");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    static public GameState GetState()
    {
        if (instance == null)
            return GameState.Null;

        return instance.gameState;
    }

    static public void ConfirmSetUp()
    {
        if (instance != null)
            instance._FinalizeSetUp();
    }
    private void _FinalizeSetUp()
    {
        if (doors.Count == 0)
        {
            Debug.Log("No door marker detected");
            return;
        }
        gameState = GameState.Preparation;
        //...
    }

    public static void RegisterDoor(Door door)
    {
        if (instance != null)
            instance._RegisterDoor(door);
    }
    private void _RegisterDoor(Door door)
    {
        if (gameState == GameState.Fight)
            tempDoors.Add(door);
        else
            doors.Add(door);
    }
}
