using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : Enemy
{
    private const float DISTANCE_BETWEEN_CENTER = 4.0f;

    private PlayerController playerObject;

    private void Start()
    {
        playerObject = FindObjectOfType<PlayerController>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Movement()
    {
        this.transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
