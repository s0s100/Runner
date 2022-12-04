using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    private float rotationSpeed = 10.0f;

    private float curZRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        bool isReverse = Random.value < 0.5;
        if (isReverse)
        {
            rotationSpeed = -rotationSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        curZRotation += rotationSpeed * Time.deltaTime;

        Quaternion lastZRotation = transform.rotation;
        lastZRotation.z = curZRotation;
        transform.rotation = lastZRotation;
    }
}
