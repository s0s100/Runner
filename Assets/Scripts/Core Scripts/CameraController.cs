using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static Vector2 DEFAULT_POSITION = new Vector3(0.0f, 0.0f, Z_CAMERA_DISTANCE);

    private const float Z_CAMERA_DISTANCE = -10.0f;
    private const float MIN_Y_DIST_REQUIRED = 0.2f; // Distance between player and camera required to move camera
    private const float DEFAULT_CAMERA_ACCELERATION = 0.0025f;
    private const float CAMERA_SPEED_LIMIT = 0.1f;
    private const float Y_SHIFT_BETWEEN_PLAYER = 0.5f; // Difference between camera center and player

    private GameObject player;
    private BackgroundController backgroundController;
    private GameController gameController;
    private LevelGenerator levelGenerator;

    private float xSpeed;
    private float ySpeed;

    // Position to stop camera at
    private float xStopPosition = float.MaxValue;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        backgroundController = FindObjectOfType<BackgroundController>();
        player = levelGenerator.GetPlayer();

        xSpeed = gameController.GetGameSpeed();
        enabled = false;
    }
    
    void FixedUpdate()
    {
        if (!ShouldStopCamera())
        {
            Movement();
        }
    }

    private bool ShouldStopCamera()
    {
        if (transform.position.x >= xStopPosition)
        {
            backgroundController.SetZeroSpeed();
            return true;            
        }

        return false;
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
            if (ySpeed > CAMERA_SPEED_LIMIT)
            {
                ySpeed = CAMERA_SPEED_LIMIT;
            }

            float newYPose = transform.position.y + ySpeed; 
            return newYPose;
        } else
        {
            ySpeed = 0;
            return transform.position.y;
        }
    }

    // Move camera to specified position and stop camera
    public void SetStopPosition(float xPosition)
    {
        xStopPosition = xPosition;
    }

    public void ResetCamera()
    {
        transform.position = DEFAULT_POSITION;
        xStopPosition = float.MaxValue;
    }
}
