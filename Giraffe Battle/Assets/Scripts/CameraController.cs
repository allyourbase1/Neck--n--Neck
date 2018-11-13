using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject playerOne;
    [SerializeField] GameObject playerTwo;
    [SerializeField] float cameraMinDist = 4.5f;
    [SerializeField] float cameraYOffset = .6f;
    Camera fightCam;
    float playerDistance;
    Vector3 playersMidpoint;
    const float camBaseYOffset = 3.75f; // I don't know why it's 3.75, but it works.

    void Start()
    {
        fightCam = Camera.main;
        // Used to calculate cam y position
        playerOne = GameObject.Find("PlayerOne");
        playerTwo = GameObject.Find("PlayerTwo");

        GetPlayerPositionalData();
        SetCamera();
    }

    private void GetPlayerPositionalData()
    {
        // Calculate absolute distance between two players
        playerDistance = Mathf.Abs(playerOne.transform.position.z - playerTwo.transform.position.z);
        // Determine midpoint between two players to set camera x position
        playersMidpoint = new Vector3 (transform.position.x, playerOne.transform.position.y, (playerOne.transform.position.z + playerTwo.transform.position.z) / 2);
    }

    private void SetCamera()
    {
        fightCam.transform.position = new Vector3(transform.position.x, playersMidpoint.y + (fightCam.orthographicSize - camBaseYOffset) - cameraYOffset, playersMidpoint.z);

        // Stops the camera from zooming in once a certain threshold is met
        if (playerDistance / 2 > cameraMinDist)
        {
            fightCam.orthographicSize = playerDistance / 2;
        }
        else
        {
            fightCam.orthographicSize = cameraMinDist;
        }
    }

    // Camera position is updated to keep camera in between two players and to keep players the same distance from the bottom of the screen regardless of zoom.
    void LateUpdate()
    {
        GetPlayerPositionalData();
        SetCamera();
    }
}
