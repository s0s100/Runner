using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float moveSpeeed = 10; // Increases every 100 meters
    public PlayerMovement playerMovement;
    public CameraFollowPlayer cameraFollowPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Enable update functions
        if (Input.anyKey)
        {
            playerMovement.enabled = true;
            playerMovement.enablePlayerAnimations();
            cameraFollowPlayer.enabled = true;
        }
    }
}
