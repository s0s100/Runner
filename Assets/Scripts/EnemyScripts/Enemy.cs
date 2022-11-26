using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected GameController gameController;
    protected float speed;
    protected float existanceTime = 10.0f;

    protected virtual void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        Destroy(this.gameObject, existanceTime); // Delete after
    }
    
    protected virtual void Update()
    {
        Movement();
    }

    // Moves Enemy object in runtimeon the screen
    protected abstract void Movement();
}
