using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float Z_CAMERA_DISTANCE = -10.0f;
    // Distance between player and camera required to start moving camera
    private const float REQUIRED_Y_DISTANCE = 0.5f;
    private const float DEFAULT_CAMERA_ACCELERATION = 0.1f;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameController gameController;

    private float xSpeed;
    private float ySpeed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = FindObjectOfType<GameController>();

        xSpeed = gameController.moveSpeeed;
        enabled = false;
    }
    
    void Update()
    {
        Movement();
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
        // If positive, camera is higher
        float camPlayerDiff = transform.position.y - player.transform.position.y;
        if (Mathf.Abs(camPlayerDiff) > REQUIRED_Y_DISTANCE )
        {
            if (camPlayerDiff > 0)
            {
                ySpeed -= DEFAULT_CAMERA_ACCELERATION * Time.deltaTime;
            } else
            {
                ySpeed += DEFAULT_CAMERA_ACCELERATION * Time.deltaTime;
            }
            
            float newYPose = transform.position.y + (ySpeed * Time.deltaTime);

            return newYPose;
        } else
        {
            ySpeed = 0;
            return transform.position.y;
        }
    }
}
