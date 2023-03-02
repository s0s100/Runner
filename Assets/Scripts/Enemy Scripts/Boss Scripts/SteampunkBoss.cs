using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SteampunkBossAction
{
    LazerPlayer,
    LazerArea,
    LazerTop,
    MeeleAttack
}

public class SteampunkBoss : Enemy
{
    [SerializeField]
    private GameObject lazerObject;
    [SerializeField]
    private GameObject fallGenerationObjects;

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
