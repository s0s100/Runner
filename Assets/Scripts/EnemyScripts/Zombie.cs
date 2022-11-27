using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        speed = (gameController.GetGameSpeed() / 2); // Half of the player speed
        animator = GetComponent<Animator>();
        animator.SetTrigger("IsAwakening");
    }
    
    protected override void Update()
    {
        base.Update();
    }

    // Called every frame
    protected override void Movement()
    {

    }

    private void Start()
    {
        AwakeZombie();
        this.enabled = false;
    }

    private void AwakeZombie()
    {
        // Animation length for now
        // Maybe 
        //float clipTime = GetAnimationLength("ZombieAwake");

        float clipTime = 2.0f;
        StartCoroutine(ChangeAnimation(clipTime));
        this.enabled = true;
    }

    private IEnumerator ChangeAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger("IsMoving");
    }

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
}
