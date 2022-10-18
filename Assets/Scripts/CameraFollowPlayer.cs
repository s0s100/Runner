using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public bool isCameraRunning;
    private static float zDistance = -10.0f;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameController controller = FindObjectOfType<GameController>();
        float moveSpeed = controller.moveSpeeed;

        float xDistance = transform.position.x + (moveSpeed * Time.deltaTime);
        Vector3 newVector = new Vector3(xDistance, 0, zDistance);
        this.transform.position = newVector;
    }
}
