using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCreator : MonoBehaviour
{
    [SerializeField] GameObject gameManagerPrefab;
    [SerializeField] GameObject musicControllerPrefab;
    GameObject gameManager;
	// Use this for testing only
	void Awake ()
    {
		if(GameObject.Find("GameManager") == null)
        {
            gameManager = Instantiate(gameManagerPrefab);
            gameManager.name = "GameManager";

            GameObject musicController = Instantiate(musicControllerPrefab);
            musicController.name = "MusicController";
        }
	}
}
