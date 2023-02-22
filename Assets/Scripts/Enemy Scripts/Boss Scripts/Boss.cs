using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    protected override void Awake()
    {
        base.Awake();
        speed = gameController.GetGameSpeed();
    }
    
    protected override void Update()
    {
        base.Update();
    }

    protected override void Movement()
    {
        transform.position += Vector3.right *speed * Time.deltaTime;
    }
}
