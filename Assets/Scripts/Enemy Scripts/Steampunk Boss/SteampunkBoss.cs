using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteampunkBoss : Enemy
{
    PlayerDataScreen dataScreen;

    public static int REWARD_AMOUNT = 10;

    [SerializeField]
    private int startingHealth = 10;
    private const float REQUIRED_PLAYER_Y_DIFFERENCE = 1.0f;

    private GameObject player;

    protected override void Awake()
    {
        base.Awake();
        health = startingHealth;
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {
        player = FindObjectOfType<LevelGenerator>().GetPlayer();
        dataScreen = FindObjectOfType<PlayerDataScreen>();
        NotifyDataScreen();
    }

    protected override void Movement()
    {
        this.transform.position += speed * Vector3.right * Time.deltaTime;
        FollowPlayerYPosition();
    }

    private void FollowPlayerYPosition()
    {
        float playerYPos = player.transform.position.y;
        float bossYPos = this.transform.position.y;

        if (playerYPos + REQUIRED_PLAYER_Y_DIFFERENCE + SteampunkBossMove.Y_SHIFT <  bossYPos)
        {
            // Move boss in Y direction in half of Move speed
            transform.position += Vector3.down * speed * Time.deltaTime / 2;
        } else if (playerYPos - REQUIRED_PLAYER_Y_DIFFERENCE + SteampunkBossMove.Y_SHIFT > bossYPos)
        {
            transform.position += Vector3.up * speed * Time.deltaTime / 2;
        }
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
        return (float) health / (float) startingHealth;
    }
}
