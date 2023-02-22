using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Displaying damage
    private const float MIN_SPRITE_TRANSPARENCY = 0.5f;
    private const float TRANSPARENCY_CHANGE_INCREMENT = 7.5f;

    private SpriteRenderer spriteRenderer;
    private float invulnerabilityTime = 1.0f;
    private float curInvulnerability = 0.0f;
    private bool isIncreasingTransparency = false;

    [SerializeField]
    private GameObject particleSystemObject;

    protected GameController gameController;
    protected Animator animator;
    protected float speed;
    protected float existanceTime = 10.0f;

    protected int health = 1;

    protected virtual void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Destroy(this.gameObject, existanceTime); // Delete after
    }
    
    protected virtual void Update()
    {
        Movement();
        InvulnerabilityControl();
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
        GameObject particles = Instantiate(particleSystemObject);
        particles.transform.position = transform.position;
        ParticleSystem system = particles.GetComponent<ParticleSystem>();

        isIncreasingTransparency = false;
        curInvulnerability = invulnerabilityTime;
    }

    protected virtual void Kill()
    {
        animator.SetBool("IsKilled", true);
    }

    public int GetCurHealth()
    {
        return health;
    }



    // Makes object red-blinking if invulnerable
    private void InvulnerabilityControl()
    {
        if (curInvulnerability > 0.0f)
        {
            curInvulnerability -= Time.deltaTime;
            InvulnerabilityDisplaying();
        }
    }

    // Called if player is invulnerable
    private void InvulnerabilityDisplaying()
    {
        Color currentColor = spriteRenderer.color;

        if (curInvulnerability <= 0.0f)
        {
            animator.SetBool("IsDamaged", false);
            // currentColor.a = 1.0f;
            currentColor.g = 1.0f;
            currentColor.b = 1.0f;
        }
        else
        {
            if (isIncreasingTransparency)
            {
                // currentColor.a += Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.g += Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.b += Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;

                if (currentColor.a >= 1)
                    isIncreasingTransparency = false;
            }
            else
            {
                // currentColor.a -= Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.g -= Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.b -= Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;

                if (currentColor.a <= MIN_SPRITE_TRANSPARENCY)
                    isIncreasingTransparency = true;
            }
        }
        spriteRenderer.color = currentColor;
    }
}
