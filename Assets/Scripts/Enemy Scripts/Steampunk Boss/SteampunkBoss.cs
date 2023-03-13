using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkBoss : Enemy
{
    PlayerDataScreen dataScreen;

    private static int STARTING_HEALTH = 10;

    protected override void Awake()
    {
        base.Awake();
        health = STARTING_HEALTH;
        animator = transform.GetChild(0).GetComponent<Animator>();
        speed = gameController.GetGameSpeed();
    }

    private void Start()
    {
        dataScreen = FindObjectOfType<PlayerDataScreen>();
        NotifyDataScreen();
    }

    protected override void Movement()
    {
        this.transform.position += speed * Vector3.right * Time.deltaTime;
    }

    public override void Damage()
    {
        health--;
        SetDamageAnimation();
        UpdateHealthBar();

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

    private void NotifyDataScreen()
    {
        dataScreen.SetToBossState();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        dataScreen.UpdateScoreBar(GetHealthProportion());
    }

    private float GetHealthProportion()
    {
        return (float) health / (float) STARTING_HEALTH;
    }
}
