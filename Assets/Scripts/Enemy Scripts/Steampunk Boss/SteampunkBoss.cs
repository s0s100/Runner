using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkBoss : Enemy
{
    private static int STARTING_HEALTH = 10;

    protected override void Awake()
    {
        base.Awake();
        health = STARTING_HEALTH;
        animator = transform.GetChild(0).GetComponent<Animator>();
        speed = gameController.GetGameSpeed();
    }

    protected override void Movement()
    {
        this.transform.position += speed * Vector3.right * Time.deltaTime;
    }

    public override void Damage()
    {
        health--;
        SetDamageAnimation();

        if (health <= 0)
        {
            Kill();
        }
    }

    private void SetDamageAnimation()
    {
        bool isAttackOne = Random.value < 0.5f;
        if (isAttackOne)
        {
            animator.SetTrigger("IsDamaged1");
        } else
        {
            animator.SetTrigger("IsDamaged2");
        }
    }
}
