using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkBoss : MonoBehaviour
{
    private float speed;
    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        speed = gameController.GetGameSpeed();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        this.transform.position += speed * Vector3.right * Time.deltaTime;
    }
}
