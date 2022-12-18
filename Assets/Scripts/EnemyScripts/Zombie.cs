using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private const float TIME_BEFORE_AWAKENING = 1.5f;

    private Animator animator;
    private GameObject player;

    private new BoxCollider2D collider;
    private bool isAttacking = false;

    protected override void Awake()
    {
        existanceTime = 25.0f;
        base.Awake();
        speed = (gameController.GetGameSpeed() / 2); // Half of the player speed

        player = GameObject.FindGameObjectWithTag("Player");
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        animator.SetTrigger("IsAwakening");
    }

    // Move towards player and don't fall
    protected override void Movement()
    {
        
        // First check distance between player and zombie
        float xZombiePos = this.transform.position.x;
        float xPlayerPos = player.transform.position.x;
        float xDiff = xZombiePos - xPlayerPos; // If positive, go left

        Vector3 newPosition = transform.position;
        Quaternion rotation = transform.rotation;
        if (xDiff > 0)
        {
            newPosition += speed * Time.deltaTime * Vector3.left;
            rotation.Set(0, 180, 0, 0);
        } else
        {
            newPosition += speed * Time.deltaTime * Vector3.right;
            rotation.Set(0, 0, 0, 0);
        }
        transform.SetPositionAndRotation(newPosition, rotation);
    }

    private void Start()
    {
        this.enabled = false;
        SetIgnorePlayerCollider(true);
        // AwakeZombie();
    }

    public void AwakeZombie()
    {
        SetIgnorePlayerCollider(false);
        this.enabled = true;
        isAttacking = true;
    }

    //private void AwakeZombie()
    //{
    //    StartCoroutine(WaitBeforeMovement());
    //}

    //private IEnumerator WaitBeforeMovement()
    //{
    //    yield return new WaitForSeconds(TIME_BEFORE_AWAKENING);
    //    SetIgnorePlayerCollider(false);
    //    this.enabled = true;
    //    isAttacking = true;
    //}

    private float GetAnimationLength(string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                float length = clip.length;
                Debug.Log("Clip length: " + length);
                return length;
            }
        }

        Debug.Log("Clip was not found");
        return 0.0f;
    }

    private void SetIgnorePlayerCollider(bool isIgnoring)
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider, playerCollider, isIgnoring);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player" && isAttacking)
    //    {
    //        PlayerHealthScript playerHealthScript =
    //            collision.gameObject.GetComponent<PlayerHealthScript>();

    //        playerHealthScript.GetDamage();
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isAttacking)
        {
            PlayerHealthScript playerHealthScript =
                collision.gameObject.GetComponent<PlayerHealthScript>();

            animator.SetBool("IsAttacking", true);

            playerHealthScript.GetDamage();
        }
    }

    protected override void Kill()
    {
        base.Kill();
        isAttacking = false;
        SetIgnorePlayerCollider(true);
        enabled = false;
    }
}
