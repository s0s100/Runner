using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : Enemy
{
    private const float DISTANCE_BETWEEN_CENTER = 4.0f;

    private PlayerController playerObject;

    protected override void Awake()
    {
        base.Awake();
        Destroy(this.gameObject, existanceTime);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Movement()
    {
        transform.position += speed * Vector3.right * Time.deltaTime;
    }

    private void Start()
    {
        playerObject = FindObjectOfType<PlayerController>();
    }
}
