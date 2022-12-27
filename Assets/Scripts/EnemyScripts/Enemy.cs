using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // For now
    private new ParticleSystem particleSystem;

    protected GameController gameController;
    protected Animator animator;
    protected float speed;
    protected float existanceTime = 10.0f;

    protected int health = 1;
    protected float deathTime = 1.0f;

    protected virtual void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        Destroy(this.gameObject, existanceTime); // Delete after
    }
    
    protected virtual void Update()
    {
        Movement();
    }

    // Moves Enemy object in runtimeon the screen
    protected abstract void Movement();

    public void Damage(int damage)
    {
        health -= damage;
        DamageAnimation();

        if (health <= 0)
        {
            Kill();
        }
    }

    public void Damage()
    {
        health--;
        DamageAnimation();

        if (health <= 0)
        {
            Kill();
        }
    }

    protected void DamageAnimation()
    {
        // animator.SetBool("IsDamaged", true);
        particleSystem.Play();
    }

    protected virtual void Kill()
    {
        animator.SetBool("IsKilled", true);
    }

    public int GetCurHealth()
    {
        return health;
    }
}
