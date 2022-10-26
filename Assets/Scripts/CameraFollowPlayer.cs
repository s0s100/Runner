using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private const float Z_CAMERA_DISTANCE = -10.0f;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        enabled = false;
    }
    
    void Update()
    {
        float moveSpeed = gameController.moveSpeeed;
        float xDistance = transform.position.x + (moveSpeed * Time.deltaTime);
        Vector3 newVector = new Vector3(xDistance, 0, Z_CAMERA_DISTANCE);
        this.transform.position = newVector;
    }
}
