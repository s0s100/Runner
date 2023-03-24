using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDownZone : MonoBehaviour
{
    [SerializeField]
    private float downViewDist = 1.0f;

    CameraController cameraController;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cameraController.SetCameraOffset(downViewDist);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cameraController.ZeroCameraOffset();
        }
    }
}
