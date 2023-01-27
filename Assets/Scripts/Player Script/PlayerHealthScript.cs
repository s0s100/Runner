using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthScript : MonoBehaviour
{
    private const int START_HEALTH = 3;
    private const float MIN_SPRITE_TRANSPARENCY = 0.5f;
    private const float TRANSPARENCY_CHANGE_INCREMENT = 5.0f;

    private UIController uiController;
    private TMP_Text healthText;

    private float invulnerabilityTime = 1.0f;
    private int curHealth = START_HEALTH;

    private LevelGenerator levelGenerator;
    private GameController gameController;
    private SpriteRenderer playerSpriteRenderer;
    private float curInvulnerability = 0.0f;
    private bool isIncreasingTransparency = false;

    private Animator animator;

    [SerializeField]
    private GameObject bloodObject;

    private void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        uiController = FindObjectOfType<UIController>();
        healthText = uiController.GetHealthText();
        healthText.text = curHealth.ToString();
    }

    private void Update()
    {
        InvulnerabilityControl();
    }

    // Removes one hearth
    public void GetDamage()
    {
        if (curInvulnerability <= 0)
        {
            CreateBlood();

            animator.SetBool("IsDamaged", true);
            isIncreasingTransparency = false;
            curInvulnerability = invulnerabilityTime;
            curHealth--;
            healthText.text = curHealth.ToString();

            if (curHealth == 0)
            {
                gameController.GameDefeat();
                PlayerKillAnimation();
            }
        }
    }

    private void CreateBlood()
    {
        GameObject newBlood = Instantiate(bloodObject);
        newBlood.transform.position = transform.position;
        newBlood.transform.parent = levelGenerator.GetAnimationParent().transform;
        newBlood.GetComponent<ParticleSystem>().Play();
    }

    private void PlayerKillAnimation()
    {
        animator.SetBool("IsKilled", true);
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
        Color currentColor = playerSpriteRenderer.color;

        if (curInvulnerability <= 0.0f)
        {
            animator.SetBool("IsDamaged", false);
            // currentColor.a = 1.0f;
            currentColor.g = 1.0f;
            currentColor.b = 1.0f;
        } else
        {
            if (isIncreasingTransparency)
            {
                // currentColor.a += Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.g += Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.b += Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;

                if (currentColor.a >= 1)
                    isIncreasingTransparency = false;
            } else
            {
                // currentColor.a -= Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.g -= Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;
                currentColor.b -= Time.deltaTime * TRANSPARENCY_CHANGE_INCREMENT;

                if (currentColor.a <= MIN_SPRITE_TRANSPARENCY)
                    isIncreasingTransparency = true;
            }
        }
        playerSpriteRenderer.color = currentColor;
    }
}
