using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    GameObject playerOneFighter;
    GameObject playerTwoFighter;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.FighterSpawnerID(this);
    }

    public void InputFighterData(GameObject fighter, string playerTag)
    {
        if(playerTag == "Player1")
        {
            playerOneFighter = fighter;
        }
        else if (playerTag == "Player2")
        {
            playerTwoFighter = fighter;
        }
    }

    public void SpawnFighters()
    {
        GameObject playerOne = Instantiate(playerOneFighter, new Vector3(0, 0, -9), Quaternion.identity);
        GameObject playerTwo = Instantiate(playerTwoFighter, new Vector3(0, 0, 9), Quaternion.identity);
        playerOne.GetComponent<PlayerControl>().FindOpponent();
        playerTwo.GetComponent<PlayerControl>().FindOpponent();
    }
}
