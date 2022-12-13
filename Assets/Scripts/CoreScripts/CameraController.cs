using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float Z_CAMERA_DISTANCE = -10.0f;
    // Distance between player and camera required to start moving camera
    private const float MIN_Y_DIST_REQUIRED = 0.2f;
    private const float DEFAULT_CAMERA_ACCELERATION = 0.0025f;
    private const float SPEED_LIMIT = 0.1f;
    private const float Y_SHIFT_BETWEEN_PLAYER = 0.5f; // 

    private GameObject player;
    private GameController gameController;
    private LevelGenerator levelGenerator;

    private float xSpeed;
    private float ySpeed;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        player = levelGenerator.GetPlayer();

        xSpeed = gameController.GetGameSpeed();
        enabled = false;
    }
    
    void FixedUpdate()
    {
        if (!gameController.IsDefeated())
        {
            Movement();
        }
    }

    private void Movement()
    {
        Vector3 newPos = new Vector3(CalculateXPos(), CalculateYPos(), Z_CAMERA_DISTANCE);
        transform.position = newPos;
    }

    private float CalculateXPos()
    {
        float newXPose = transform.position.x + (xSpeed * Time.deltaTime);
        return newXPose;
    }

    private float CalculateYPos()
    {
        float minY = levelGenerator.GetMinYPos();
        float camPlayerDiff = player.transform.position.y - transform.position.y + Y_SHIFT_BETWEEN_PLAYER;

        bool isSpeedChanging = (transform.position.y >= minY || camPlayerDiff > 0)
         && Mathf.Abs(camPlayerDiff) > MIN_Y_DIST_REQUIRED;
        if (isSpeedChanging)
        {
            ySpeed += DEFAULT_CAMERA_ACCELERATION * camPlayerDiff;
            if (ySpeed > SPEED_LIMIT)
            {
                ySpeed = SPEED_LIMIT;
            }

            float newYPose = transform.position.y + ySpeed; 
            return newYPose;
        } else
        {
            ySpeed = 0;
            return transform.position.y;
        }
        
    }
}
